using UnityEngine;

[System.Serializable]
public class OnHitModifierDefinition
{
    [SerializeField] private OnHitModifierType modifier;
    public OnHitModifierType Modifier => modifier;

    [SerializeField] private ModifierDefinition definition;
    public ModifierDefinition Definition => definition;
}