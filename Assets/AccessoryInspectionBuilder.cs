public static class AccessoryInspectionBuilder
{
    public static InspectionData Build(Accessory accessory)
    {
        InspectionData data = new();

        data.Icon = accessory.icon;
        data.Title = accessory.accessoryName;
        data.Subtitle = accessory.Mastery.ToString();
        data.Description = accessory.description;

        // Passive modifiers
        // OnHit modifiers
        // Special effects

        return data;
    }
}