using UnityEngine;

//BUILDAPPLIER.CS

public enum PassiveModifierType
{
    MaxHealth,
    MaxMana,
    ManaRegen,
    MoveSpeed,
    SpellDamage,
    CooldownRecovery,
    WizardSize,
    JumpHeight,
    UltimateCharge,
    HealthRegen,
    HealthRegenPercent,
    DamageTakeManaRestore,
    DamageTakeManaRestorePercent,
}

[System.Serializable]
public class PassiveModifier
{
    [SerializeField]
    private PassiveModifierType modifier;

    public PassiveModifierType Modifier => modifier;

    [SerializeField]
    private SpellElement element;

    public SpellElement Element => element;

    [SerializeField]
    private float value;

    public float Value => value;
}