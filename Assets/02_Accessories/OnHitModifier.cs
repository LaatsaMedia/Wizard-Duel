using UnityEngine;

public enum OnHitModifierType
{
    // =========================
    // Damage
    // =========================

    DamageMultiplier,
    FlatDamage,

    CurrentHealthDamage,
    MissingHealthDamage,
    MaxHealthDamage,

    TrueDamage,                 // Ignores resistances (future)
    ExecuteThreshold,           // Bonus if target below X%

    // =========================
    // Status Effects
    // =========================

    Burn,
    Poison,
    Bleed,

    Slow,
    Freeze,
    Root,
    Stun,
    Silence,

    Blind,
    Confuse,

    // =========================
    // Healing
    // =========================

    LifeSteal,
    HealOnHit,
    HealPercentMissingHealth,
    HealPercentMaxHealth,

    // =========================
    // Mana
    // =========================

    ManaRestoreOnHit,
    ManaBurn,
    ManaSteal,

    // =========================
    // Cooldowns
    // =========================

    CooldownReductionOnHit,
    CooldownResetChance,

    // =========================
    // Movement
    // =========================

    Knockback,
    Pull,
    Launch,

    // =========================
    // Spell Behaviour
    // =========================

    Pierce,
    Ricochet,
    SplitProjectile,
    Chain,
    EchoCast,

    // =========================
    // Debuffs
    // =========================

    ReduceHealing,
    ReduceManaRegen,
    ReduceSpellDamage,
    ReduceMoveSpeed,

    // =========================
    // Buff Stealing
    // =========================

    Dispel,
    StealBuff,

    // =========================
    // Economy
    // =========================

    UltimateCharge,

    // =========================
    // Summons
    // =========================

    SpawnOrb,
    SpawnExplosion,
    SpawnMeteor,

    // =========================
    // Misc
    // =========================

    MarkTarget,
    Curse,
    Hex
}

[System.Serializable]
public class OnHitModifier
{
    [SerializeField]
    private SpellElement element;

    public SpellElement Element => element;

    [SerializeField]
    private OnHitModifierType modifier;

    public OnHitModifierType Modifier => modifier;

    [SerializeField]
    private float value;

    public float Value => value;

    [SerializeField]
    private float duration;

    public float Duration => duration;
}