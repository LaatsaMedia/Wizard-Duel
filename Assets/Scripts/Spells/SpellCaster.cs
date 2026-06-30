using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public Mana mana;

    [Header("Spell Slots")]
    [SerializeField] private SpellSlot primarySpell;
    [SerializeField] private SpellSlot secondarySpell;
    [SerializeField] private SpellSlot thirdSpell;

    public SpellSlot PrimarySpell => primarySpell;
    public SpellSlot SecondarySpell => secondarySpell;
    public SpellSlot ThirdSpell => thirdSpell;

    [SerializeField] private Transform spellSpawn;

    private void Awake()
    {
        mana = GetComponent<Mana>();
    }

    public void ApplyBuild(WizardBuild build)
    {
        primarySpell.spell = build.primarySpell;
        secondarySpell.spell = build.secondarySpell;
        thirdSpell.spell = build.thirdSpell;

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

    private void TickCooldown(SpellSlot slot)
    {
        if (slot.cooldownRemaining > 0f)
            slot.cooldownRemaining -= Time.deltaTime;
    }

    private void Cast(SpellSlot slot)
    {
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

        GameObject spellObject = Instantiate(
            slot.spell.spellPrefab,
            spellSpawn.position,
            spellSpawn.rotation);

        if (spellObject.TryGetComponent(out SpellBehaviour spellBehaviour))
        {
            spellBehaviour.Initialize(gameObject);

            if (spellBehaviour.SupportsRecast)
            {
                slot.activeRecastSpell = spellBehaviour;
            }
        }

        slot.cooldownRemaining = slot.spell.cooldown;
    }
}