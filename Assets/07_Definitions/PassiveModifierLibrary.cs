using UnityEngine;

[CreateAssetMenu(menuName = "Inspection/Passive Modifier Library")]
public class PassiveModifierLibrary : ScriptableObject
{
    [SerializeField]
    private PassiveModifierDefinition[] modifiers;

    public ModifierDefinition Get(PassiveModifierType modifier)
    {
        foreach (PassiveModifierDefinition entry in modifiers)
        {
            if (entry.Modifier == modifier)
                return entry.Definition;
        }

        return null;
    }
}