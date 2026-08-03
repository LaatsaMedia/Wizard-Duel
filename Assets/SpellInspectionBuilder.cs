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

        data.Tags.Add(new InspectionTag(
            null,
            spell.Category.ToString()));

        AddStats(data, spell);

        if (!string.IsNullOrWhiteSpace(spell.note))
        {
            data.Notes.Add(new InspectionNote(
                null,
                spell.note));
        }

        return data;
    }

    private static void AddStats(InspectionData data, Spell spell)
    {
        // Primary stat (Damage, Heal, Shield...)
        if (spell.PrimaryValue > 0f)
        {
            data.Stats.Add(new InspectionStat(
                GetPrimaryStatIcon(spell.PrimaryStat),
                spell.PrimaryStat.ToString(),
                spell.PrimaryValue.ToString("F0")));
        }

        // Status effect (Root, Stun, Frozen...)
        if (spell.DisableEffect != StatusEffectType.None &&
            spell.DisableDuration > 0f)
        {
            StatusEffectDefinition definition =
                InspectionManager.Instance.StatusEffectLibrary.Get(spell.DisableEffect);

            if (definition != null)
            {
                data.Stats.Add(new InspectionStat(
                    definition.Icon,
                    spell.DisableEffect.ToString(),
                    $"{spell.DisableDuration:0.##}s"));
            }
        }

        // Mana Cost
        data.Stats.Add(new InspectionStat(
            InspectionManager.Instance.ManaIcon,
            "Mana Cost",
            spell.manaCost.ToString("F0")));

        // Cooldown
        data.Stats.Add(new InspectionStat(
            InspectionManager.Instance.CooldownIcon,
            "Cooldown",
            $"{spell.cooldown:0.##}s"));
    }

    private static Sprite GetPrimaryStatIcon(SpellPrimaryStat stat)
    {
        switch (stat)
        {
            case SpellPrimaryStat.Damage:
                return InspectionManager.Instance.DamageIcon;

            case SpellPrimaryStat.Heal:
                return InspectionManager.Instance.HealIcon;

            case SpellPrimaryStat.Shield:
                return InspectionManager.Instance.ShieldIcon;

            default:
                return null;
        }
    }
}