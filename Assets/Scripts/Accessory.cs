using UnityEngine;

public enum AccessoryMastery
{
    Common,
    Rare,
    Epic
}

public enum AccessoryEffect
{
    ManaRegen,
    MaxMana,
    MaxHealth,
    MoveSpeed,
    SpellDamage,
    CooldownRecovery,
    DoubleJump,
    FreezeOnIceShard
}

[CreateAssetMenu(menuName = "Accessories/Accessory")]
public class Accessory : ScriptableObject
{
    [SerializeField] private AccessoryMastery mastery;
    public AccessoryMastery Mastery => mastery;

    public string accessoryName;
    public Sprite icon;
    public string description;

    [Header("Effects")]
    [SerializeField] private AccessoryModifier[] modifiers;
    public AccessoryModifier[] Modifiers => modifiers;
}