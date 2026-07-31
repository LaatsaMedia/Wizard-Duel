using System.Collections.Generic;
using UnityEngine;

public class InspectionData
{
    public Sprite Icon;
    public string Title;
    public string Subtitle;
    public string Description;

    public readonly List<InspectionStat> Stats = new();
    public readonly List<string> Notes = new();
}