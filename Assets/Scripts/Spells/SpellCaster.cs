using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class SpellCaster : MonoBehaviour
{
    private StatusEffectController statusEffects;
    public Mana mana;
    [SerializeField] private WizardBuild build;
    private UltimateCharge ultimateCharge;
    public WizardBuild Build => build;

    [Header("Spell Slots")]
    [SerializeField] private SpellSlot primarySpell;
    [SerializeField] private SpellSlot secondarySpell;
    [SerializeField] private SpellSlot thirdSpell;
    [SerializeField] private UltimateSlot ultimateSlot;

    public SpellSlot PrimarySpell => primarySpell;
    public SpellSlot SecondarySpell => secondarySpell;
    public SpellSlot ThirdSpell => thirdSpell;
    public UltimateSlot UltimateSlot => ultimateSlot;

    [SerializeField] private Transform spellSpawn;
    public Transform SpellSpawn => spellSpawn;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundOffset = 0.05f;
    [SerializeField] private float groundCastDistance = 30f;

    private void Awake()
    {
        mana = GetComponent<Mana>();
        statusEffects = GetComponent<StatusEffectController>();
        ultimateCharge = GetComponent<UltimateCharge>();
    }

    public void ApplyBuild(WizardBuild build)
    {
        this.build = build;

        primarySpell.spell = build.primarySpell;
        secondarySpell.spell = build.secondarySpell;
        thirdSpell.spell = build.thirdSpell;
        ultimateSlot.spell = build.ultimateSpell;

        ultimateCharge.ResetCharge();

        primarySpell.cooldownRemaining = 0f;
        secondarySpell.cooldownRemaining = 0f;
        thirdSpell.cooldownRemaining = 0f;

        primarySpell.activeRecastSpell = null;
        secondarySpell.activeRecastSpell = null;
        thirdSpell.activeRecastSpell = null;
    }

    private void Update()
    {
        TickCooldown(primarySpell);
        TickCooldown(secondarySpell);
        TickCooldown(thirdSpell);

        if (Input.GetKeyDown(KeyCode.M))
        {
            ultimateCharge.AddCharge(100);
        }
    }

    public void CastPrimary()
    {
        if(!MatchManager.RoundActive)
            return;
        Cast(primarySpell);
    }

    public void CastSecondary()
    {
        if(!MatchManager.RoundActive)
            return;
        Cast(secondarySpell);
    }

    public void CastThird()
    {
        if(!MatchManager.RoundActive)
            return;
        Cast(thirdSpell);
    }

    public void CastUltimate()
    {
        if (!MatchManager.RoundActive)
            return;

        if (!ultimateCharge.IsReady)
            return;

        if (statusEffects.IsFrozen ||
            statusEffects.IsStunned)
            return;

        if (ultimateSlot.spell == null)
            return;

        if (CastUltimateSpell())
        {
            ultimateCharge.UseUltimate();
        }
    }

    private bool CastUltimateSpell()
    {
        if (!mana.TrySpendMana(ultimateSlot.spell.manaCost))
            return false;

        if (ultimateSlot.spell.castSFX != null)
        {
            AudioManager.Instance.Play(
                ultimateSlot.spell.castSFX);
        }

        float direction =
            Mathf.Sign(spellSpawn.right.x);

        Vector3 spawnPosition =
            spellSpawn.position;

        GameObject spellObject = Instantiate(
            ultimateSlot.spell.spellPrefab,
            spawnPosition,
            spellSpawn.rotation);

        if (spellObject.TryGetComponent(out SpellBehaviour spellBehaviour))
        {
            spellBehaviour.Initialize(
                gameObject,
                direction);
        }

        return true;
    }

    private void TickCooldown(SpellSlot slot)
    {
        if (slot.cooldownRemaining > 0f)
            slot.cooldownRemaining -= Time.deltaTime;
    }

    private void Cast(SpellSlot slot)
    {
        if (statusEffects.IsFrozen ||
            statusEffects.IsStunned)
            return;

        if (slot.spell == null)
            return;

        // Recast this slot's active spell.
        if (slot.activeRecastSpell != null)
        {
            if (slot.activeRecastSpell.Recast())
            {
                slot.activeRecastSpell = null;
                return;
            }
        }

        if (slot.cooldownRemaining > 0f)
            return;

        if (!mana.TrySpendMana(slot.spell.manaCost))
            return;

        if (slot.spell.castSFX != null)
        {
            AudioManager.Instance.Play(slot.spell.castSFX);
        }

        float direction = Mathf.Sign(spellSpawn.right.x);

        Vector3 spawnPosition = spellSpawn.position;

        if (slot.spell.spellPrefab.TryGetComponent(
            out SpellBehaviour prefabSpell) &&
            prefabSpell.IsGroundSpell)
        {
            spawnPosition = GetGroundSpawnPosition(direction);
        }

        GameObject spellObject = Instantiate(
            slot.spell.spellPrefab,
            spawnPosition,
            spellSpawn.rotation);

        if (spellObject.TryGetComponent(out SpellBehaviour spellBehaviour))
        {
            spellBehaviour.Initialize(gameObject, direction);

            if (spellObject.TryGetComponent(out IceShardProjectile iceShard))
            {
                if (build.HasAccessoryEffect(AccessoryEffect.FreezeOnIceShard))
                {
                    iceShard.EnableFreeze(
                        4,
                        4f,
                        1f);
                }
            }

            if (spellBehaviour.SupportsRecast)
            {
                slot.activeRecastSpell = spellBehaviour;
            }
        }

        slot.cooldownRemaining = slot.spell.cooldown;
    }

    private Vector3 GetGroundSpawnPosition(float direction)
{
    float x = spellSpawn.position.x + groundCastDistance * direction;

    Vector3 rayOrigin = new Vector3(
        x,
        100f,
        0f);

    Debug.DrawRay(
        rayOrigin,
        Vector2.down * 200f,
        Color.red,
        5f);

    RaycastHit2D hit = Physics2D.Raycast(
        rayOrigin,
        Vector2.down,
        200f,
        groundLayer);

    if (hit)
    {
        return new Vector3(
            hit.point.x,
            hit.point.y + groundOffset,
            0f);
    }
    return spellSpawn.position;
}
}