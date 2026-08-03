using UnityEngine;

[CreateAssetMenu(menuName = "Inspection/Passive Stat Library")]
public class PassiveStatLibrary : ScriptableObject
{
    [SerializeField] private PassiveStatDefinition[] definitions;

    public PassiveStatDefinition Get(PassiveModifierType stat)
    {
        foreach (PassiveStatDefinition definition in definitions)
        {
            if (definition.Stat == stat)
                return definition;
        }

        return null;
    }
}