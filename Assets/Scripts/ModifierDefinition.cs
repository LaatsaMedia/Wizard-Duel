using UnityEngine;

public enum ModifierDisplayType
{
    Value,
    Flat,
    Percentage,
    Multiplier
}

[CreateAssetMenu(menuName = "Inspection/Modifier Definition")]
public class ModifierDefinition : ScriptableObject
{
    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField] private ModifierDisplayType displayType;
    public ModifierDisplayType DisplayType => displayType;
}