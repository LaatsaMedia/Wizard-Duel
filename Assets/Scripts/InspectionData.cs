using System.Collections.Generic;
using UnityEngine;

public class InspectionData
{
    public Sprite Icon;

    public string Title;
    public string Subtitle;
    public string Description;


    public List<InspectionModifier> Modifiers = new();

    public readonly List<InspectionTag> Tags = new();

    public readonly List<InspectionStat> Stats = new();
    public readonly List<InspectionNote> Notes = new();
}