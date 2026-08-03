using UnityEngine;

public enum AccessoryMastery
{
    Common,
    Rare,
    Epic
}

[CreateAssetMenu(menuName = "Accessories/Accessory")]
public class Accessory : ScriptableObject
{
    [SerializeField] private AccessoryMastery mastery;
    public AccessoryMastery Mastery => mastery;

    public string accessoryName;
    public Sprite icon;
    public string description;

    [Header("Inspection")]
    [TextArea]
    public string note;

    [Header("Passive")]
    [SerializeField] private PassiveModifier[] passiveModifiers;
    public PassiveModifier[] PassiveModifiers => passiveModifiers;

    [Header("On Hit")]
    [SerializeField] private OnHitModifier[] onHitModifiers;
    public OnHitModifier[] OnHitModifiers => onHitModifiers;

    [Header("Special")]
    [SerializeField] private AccessoryEffect[] specialEffects;
    public AccessoryEffect[] SpecialEffects => specialEffects;

    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;
}