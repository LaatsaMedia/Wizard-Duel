using UnityEngine;

[CreateAssetMenu(menuName = "Inspection/Status Effect Library")]
public class StatusEffectLibrary : ScriptableObject
{
    [SerializeField] private StatusEffectDefinition[] definitions;

    public StatusEffectDefinition Get(StatusEffectType effect)
    {
        foreach (StatusEffectDefinition definition in definitions)
        {
            if (definition.Effect == effect)
                return definition;
        }

        return null;
    }
}