using UnityEngine;

public static class SpellInspectionBuilder
{
    public static InspectionData Build(Spell spell)
    {
        InspectionData data = new();

        data.Icon = spell.icon;
        data.Title = spell.spellName;
        data.Subtitle = $"{spell.Element} • {spell.Mastery}";
        data.Description = spell.description;

        data.Tags.Add(new InspectionTag(null, spell.Element.ToString()));
        data.Tags.Add(new InspectionTag(null, spell.Category.ToString()));
        data.Tags.Add(new InspectionTag(null, spell.Mastery.ToString()));

        return data;
    }
}