using UnityEngine;

public static class AccessoryInspectionBuilder
{
    private static PassiveModifierLibrary passiveLibrary;

    private static PassiveModifierLibrary PassiveLibrary
    {
        get
        {
            if (passiveLibrary == null)
            {
                passiveLibrary = Resources.Load<PassiveModifierLibrary>(
                    "Inspection/PassiveModifierLibrary");
            }

            return passiveLibrary;
        }
    }

    public static InspectionData Build(Accessory accessory)
    {
        InspectionData data = new();

        data.Icon = accessory.icon;
        data.Title = accessory.accessoryName;
        data.Subtitle = accessory.Mastery.ToString();
        data.Description = accessory.description;

        AddPassiveModifiers(data, accessory);
        AddOnHitModifiers(data, accessory);
        
        if (!string.IsNullOrWhiteSpace(accessory.note))
        {
            data.Notes.Add(new InspectionNote(
                null,
                accessory.note));
        }

        return data;
    }

    private static void AddPassiveModifiers(InspectionData data, Accessory accessory)
    {
        foreach (PassiveModifier modifier in accessory.PassiveModifiers)
        {
            ModifierDefinition definition =
                InspectionManager.Instance.PassiveModifierLibrary.Get(modifier.Modifier);

            if (definition == null)
            {
                Debug.LogWarning($"Missing ModifierDefinition for {modifier.Modifier}");
                continue;
            }

            InspectionModifier inspection = new();

            inspection.Icon = definition.Icon;
            inspection.Value = modifier.Value;
            inspection.Duration = 0f;
            inspection.DisplayType = definition.DisplayType;

            data.Modifiers.Add(inspection);
        }
    }

    private static void AddOnHitModifiers(InspectionData data, Accessory accessory)
    {
        foreach (OnHitModifier modifier in accessory.OnHitModifiers)
        {
            ModifierDefinition definition =
                InspectionManager.Instance.OnHitModifierLibrary.Get(modifier.Modifier);

            if (definition == null)
            {
                Debug.LogWarning($"Missing ModifierDefinition for {modifier.Modifier}");
                continue;
            }

            InspectionModifier inspection = new();

            inspection.Icon = definition.Icon;
            inspection.Value = modifier.Value;
            inspection.Duration = modifier.Duration;
            inspection.DisplayType = definition.DisplayType;

            data.Modifiers.Add(inspection);
        }
    }
}