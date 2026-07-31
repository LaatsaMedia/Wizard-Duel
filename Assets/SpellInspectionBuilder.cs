public static class SpellInspectionBuilder
{
    public static InspectionData Build(Spell spell)
    {
        InspectionData data = new();

        data.Icon = spell.icon;
        data.Title = spell.spellName;
        data.Subtitle = $"{spell.Element} {spell.Mastery}";
        data.Description = spell.description;

        data.Stats.Add(new InspectionStat(
            null,
            "Mana Cost",
            spell.manaCost.ToString()));

        data.Stats.Add(new InspectionStat(
            null,
            "Cooldown",
            spell.cooldown.ToString("0.0")));

        return data;
    }
}