using UnityEngine;

[System.Serializable]
public class PassiveModifierDefinition
{
    [SerializeField]
    private PassiveModifierType modifier;

    public PassiveModifierType Modifier => modifier;

    [SerializeField]
    private ModifierDefinition definition;

    public ModifierDefinition Definition => definition;
}