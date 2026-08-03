using UnityEngine;

public class InspectionModifier
{
    public Sprite Icon { get; set; }

    public float Value { get; set; }

    public float Duration { get; set; }

    public ModifierDisplayType DisplayType { get; set; }
    public bool IsBaseStat;
}