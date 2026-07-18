using UnityEngine;

public enum OnHitModifierType
{
    Burn,
    Slow,
    DamageMultiplier
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