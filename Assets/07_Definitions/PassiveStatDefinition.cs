using UnityEngine;

[CreateAssetMenu(menuName = "Inspection/Passive Stat Definition")]
public class PassiveStatDefinition : ScriptableObject
{
    [SerializeField] private PassiveModifierType stat;
    public PassiveModifierType Stat => stat;

    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField] private string displayName;
    public string DisplayName => displayName;

    [SerializeField] private ModifierDisplayType displayType;
    public ModifierDisplayType DisplayType => displayType;
}