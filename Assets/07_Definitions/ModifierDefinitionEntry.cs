using UnityEngine;

[System.Serializable]
public class ModifierDefinitionEntry<T>
{
    [SerializeField] private T modifier;
    public T Modifier => modifier;

    [SerializeField] private ModifierDefinition definition;
    public ModifierDefinition Definition => definition;
}