using UnityEngine;

[CreateAssetMenu(menuName = "Inspection/On Hit Modifier Library")]
public class OnHitModifierLibrary : ScriptableObject
{
    [SerializeField]
    private OnHitModifierDefinition[] modifiers;

    public ModifierDefinition Get(OnHitModifierType modifier)
    {
        foreach (OnHitModifierDefinition entry in modifiers)
        {
            if (entry.Modifier == modifier)
                return entry.Definition;
        }

        return null;
    }
}