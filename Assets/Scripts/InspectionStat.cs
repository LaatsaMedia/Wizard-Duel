using UnityEngine;

public class InspectionStat
{
    public Sprite Icon { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public InspectionStat() { }

    public InspectionStat(Sprite icon, string name, string value)
    {
        Icon = icon;
        Name = name;
        Value = value;
    }
}