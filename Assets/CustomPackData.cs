using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wizard Duel/Custom Pack")]
public class CustomPackData : ScriptableObject
{
    [Header("Identity")]
    public string packName;
    public Sprite icon;

    [TextArea]
    public string description;

    [Header("Contents")]
    public List<Spell> spells = new();
    public List<Accessory> accessories = new();

    public bool IsComplete
    {
        get
        {
            if (spells == null || accessories == null)
                return false;

            if (CountSpells(SpellMastery.Apprentice) != 5)
                return false;

            if (CountSpells(SpellMastery.Adept) != 4)
                return false;

            if (CountSpells(SpellMastery.Master) != 3)
                return false;

            if (CountSpells(SpellMastery.Archmage) != 2)
                return false;

            if (CountAccessories(AccessoryMastery.Common) != 6)
                return false;

            if (CountAccessories(AccessoryMastery.Rare) != 6)
                return false;

            if (CountAccessories(AccessoryMastery.Epic) != 4)
                return false;

            return true;
        }
    }

    private int CountSpells(SpellMastery mastery)
    {
        int count = 0;

        foreach (Spell spell in spells)
        {
            if (spell != null && spell.Mastery == mastery)
                count++;
        }

        return count;
    }

    private int CountAccessories(AccessoryMastery mastery)
    {
        int count = 0;

        foreach (Accessory accessory in accessories)
        {
            if (accessory != null && accessory.Mastery == mastery)
                count++;
        }

        return count;
    }
}