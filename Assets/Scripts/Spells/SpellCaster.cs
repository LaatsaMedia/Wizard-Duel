using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public enum AccessoryModifierType
{
    Burn,
    Slow,

    DamageMultiplier,
    ManaCostMultiplier,
    CooldownMultiplier
}

public class SpellCaster : MonoBehaviour
{
    private StatusEffectController statusEffects;
    public Mana mana;
    [SerializeField] private WizardBuild build;
    public UltimateCharge ultimateCharge;
    public WizardBuild Build => build;

    [SerializeField] private float cooldownRecovery = 0f;
    public float CooldownRecovery
    {
        get => cooldownRecovery;
        set => cooldownRecovery = value;
    }

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

    private float ignoreCooldownTimer;

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
        ultimateSlot.activeRecastSpell = null;
    }

    private void Update()
    {
        TickCooldown(primarySpell);
        TickCooldown(secondarySpell);
        TickCooldown(thirdSpell);

        if (ignoreCooldownTimer > 0f)
        {
            ignoreCooldownTimer -= Time.deltaTime;
        }

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

        if (statusEffects.IsFrozen ||
            statusEffects.IsStunned)
            return;

        if (ultimateSlot.spell == null)
            return;

        // Recast active ultimate.
        if (ultimateSlot.activeRecastSpell != null)
        {
            if (ultimateSlot.activeRecastSpell.Recast())
            {
                return;
            }
        }

        if (!ultimateCharge.IsReady)
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
                ultimateSlot.spell,
                direction);

            if (spellBehaviour.SupportsRecast)
            {
                ultimateSlot.activeRecastSpell =
                    spellBehaviour;
            }
        }

        return true;
    }

    private void TickCooldown(SpellSlot slot)
    {
        if (slot.cooldownRemaining <= 0f)
            return;

        float recoveryMultiplier =
            1f + cooldownRecovery / 100f;

        slot.cooldownRemaining -=
            Time.deltaTime * recoveryMultiplier;

        slot.cooldownRemaining =
            Mathf.Max(slot.cooldownRemaining, 0f);
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

        if (ignoreCooldownTimer <= 0f &&
            slot.cooldownRemaining > 0f)
        {
            return;
        }

        if (ShouldConsumeMana() &&
            !mana.TrySpendMana(slot.spell.manaCost))
        {
            return;
        }

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
            spellBehaviour.Initialize(
                gameObject,
                slot.spell,
                direction);

            if (spellObject.TryGetComponent(out IceShardProjectile iceShard))
            {
                if (build.HasSpecialEffect(AccessoryEffect.FreezeOnIceShard))
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

    public void IgnoreCooldowns(float duration)
    {
        ignoreCooldownTimer = duration;
    }

    #region NATURESGIFT

    private int naturesGiftCounter;
    private bool naturesGiftEnabled;

    public int NaturesGiftCounter => naturesGiftCounter;

    public bool NaturesGiftReady =>
        naturesGiftEnabled &&
        naturesGiftCounter == 4;

    public void EnableNaturesGift()
    {
        naturesGiftEnabled = true;
    }

    private bool ShouldConsumeMana()
    {
        if (!naturesGiftEnabled)
            return true;

        naturesGiftCounter++;

        if (naturesGiftCounter >= 5)
        {
            naturesGiftCounter = 0;
            return false;
        }

        return true;
    }

    #endregion
}