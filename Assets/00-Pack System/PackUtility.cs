using System.Collections.Generic;
using UnityEngine;

public static class PackUtility
{
    public static List<Spell> GetSpells(
        SpellDatabase database,
        PackDefinition pack)
    {
        List<Spell> result = new();

        foreach (Spell spell in database.spells)
        {
            if (spell == null)
                continue;

            if (spell.OriginPack != pack)
                continue;

            result.Add(spell);
        }

        return result;
    }

    public static List<Accessory> GetAccessories(
    AccessoryDatabase database,
    PackDefinition pack)
    {
        List<Accessory> result = new();

        foreach (Accessory accessory in database.accessories)
        {
            if (accessory == null)
                continue;

            if (accessory.OriginPack != pack)
                continue;

            result.Add(accessory);
        }

        return result;
    }
}