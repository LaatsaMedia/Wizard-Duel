using UnityEngine;

[System.Serializable]
public class AccessoryModifier
{
    [Header("Generic Modifier")]

    [SerializeField]
    private SpellElement element = SpellElement.Neutral;
    public SpellElement Element => element;

    [SerializeField]
    private AccessoryModifierType modifier;
    public AccessoryModifierType Modifier => modifier;

    [SerializeField]
    private float value;
    public float Value => value;

    [SerializeField]
    private float duration;
    public float Duration => duration;

    [Header("Special Modifier")]

    [SerializeField]
    private AccessoryEffect effect;
    public AccessoryEffect Effect => effect;
}