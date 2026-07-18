using UnityEngine;

public enum PassiveModifierType
{
    MaxHealth,
    MaxMana,
    ManaRegen,
    MoveSpeed,
    SpellDamage,
    CooldownRecovery
}

[System.Serializable]
public class PassiveModifier
{
    [SerializeField]
    private PassiveModifierType modifier;

    public PassiveModifierType Modifier => modifier;

    [SerializeField]
    private float value;

    public float Value => value;
}