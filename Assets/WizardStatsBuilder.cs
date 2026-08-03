using System.Collections.Generic;
using UnityEngine;

public static class WizardStatsBuilder
{
    public static List<InspectionModifier> Build(WizardBuild build)
    {
        List<InspectionModifier> result = new();

        BuildBaseStats(result, build);
        BuildModifiers(result, build);

        return result;
    }

    private static void BuildBaseStats(
        List<InspectionModifier> result,
        WizardBuild build)
    {
        float maxHealth = 100;
        float maxMana = 20;
        float manaRegen = 5;
        float moveSpeed = 6;

        foreach (PassiveModifier modifier in build.GetPassiveModifiers())
        {
            switch (modifier.Modifier)
            {
                case PassiveModifierType.MaxHealth:
                    maxHealth += modifier.Value;
                    break;

                case PassiveModifierType.MaxMana:
                    maxMana += modifier.Value;
                    break;

                case PassiveModifierType.ManaRegen:
                    manaRegen += modifier.Value;
                    break;

                case PassiveModifierType.MoveSpeed:
                    moveSpeed += modifier.Value;
                    break;
            }
        }

        AddBaseStat(result, PassiveModifierType.MaxHealth, maxHealth);
        AddBaseStat(result, PassiveModifierType.MaxMana, maxMana);
        AddBaseStat(result, PassiveModifierType.ManaRegen, manaRegen);
        AddBaseStat(result, PassiveModifierType.MoveSpeed, moveSpeed);
    }

    private static void BuildModifiers(
        List<InspectionModifier> result,
        WizardBuild build)
    {
        Accessory[] accessories =
        {
            build.accessory1,
            build.accessory2,
            build.accessory3,
            build.accessory4,
            build.accessory5,
            build.accessory6
        };

        foreach (Accessory accessory in accessories)
        {
            if (accessory == null)
                continue;

            // Passive modifiers
            foreach (PassiveModifier modifier in accessory.PassiveModifiers)
            {
                if (IsBaseStat(modifier.Modifier))
                    continue;

                ModifierDefinition definition =
                    InspectionManager.Instance.PassiveModifierLibrary.Get(modifier.Modifier);

                if (definition == null)
                    continue;

                result.Add(new InspectionModifier
                {
                    Icon = definition.Icon,
                    Value = modifier.Value,
                    Duration = 0f,
                    DisplayType = definition.DisplayType,
                    IsBaseStat = false
                });
            }

            // On-hit modifiers
            foreach (OnHitModifier modifier in accessory.OnHitModifiers)
            {
                ModifierDefinition definition =
                    InspectionManager.Instance.OnHitModifierLibrary.Get(modifier.Modifier);

                if (definition == null)
                    continue;

                result.Add(new InspectionModifier
                {
                    Icon = definition.Icon,
                    Value = modifier.Value,
                    Duration = modifier.Duration,
                    DisplayType = definition.DisplayType,
                    IsBaseStat = false
                });
            }
        }
    }

    private static void AddBaseStat(
        List<InspectionModifier> list,
        PassiveModifierType type,
        float value)
    {
        PassiveStatDefinition definition =
            InspectionManager.Instance.PassiveStatLibrary.Get(type);

        if (definition == null)
        {
            Debug.LogWarning($"Missing PassiveStatDefinition for {type}");
            return;
        }

        list.Add(new InspectionModifier
        {
            Icon = definition.Icon,
            Value = value,
            Duration = 0f,
            DisplayType = definition.DisplayType,
            IsBaseStat = true
        });
    }

    private static bool IsBaseStat(PassiveModifierType type)
    {
        switch (type)
        {
            case PassiveModifierType.MaxHealth:
            case PassiveModifierType.MaxMana:
            case PassiveModifierType.ManaRegen:
            case PassiveModifierType.MoveSpeed:
                return true;

            default:
                return false;
        }
    }
}