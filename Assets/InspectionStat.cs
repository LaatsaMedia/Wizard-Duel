using UnityEngine;

public class InspectionStat
{
    public Sprite Icon;
    public string Name;
    public string Value;

    public InspectionStat(Sprite icon, string name, string value)
    {
        Icon = icon;
        Name = name;
        Value = value;
    }
}