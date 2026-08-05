using System.Collections.Generic;
using UnityEngine;

public class PackTester : MonoBehaviour
{
    [SerializeField] private SpellDatabase spellDatabase;
    [SerializeField] private AccessoryDatabase accessoryDatabase;
    [SerializeField] private PackDefinition pack;

    private void Start()
    {
        TestSpells();
        TestAccessories();
    }

    private void TestSpells()
    {
        List<Spell> spells =
            PackUtility.GetSpells(
                spellDatabase,
                pack);

        Debug.Log($"=== {pack.PackName} Spells ===");

        foreach (Spell spell in spells)
        {
            Debug.Log($"Spell: {spell.spellName}");
        }
    }

    private void TestAccessories()
    {
        List<Accessory> accessories =
            PackUtility.GetAccessories(
                accessoryDatabase,
                pack);

        Debug.Log($"=== {pack.PackName} Accessories ===");

        foreach (Accessory accessory in accessories)
        {
            Debug.Log($"Accessory: {accessory.accessoryName}");
        }
    }
}