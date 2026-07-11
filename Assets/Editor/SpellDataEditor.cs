using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpellDatabase))]
public class SpellDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        SpellDatabase database = (SpellDatabase)target;

        if (GUILayout.Button("Assign All"))
        {
            AssignSpells(database);
        }

        if (GUILayout.Button("Remove All"))
        {
            database.spells = new Spell[0];

            EditorUtility.SetDirty(database);

            Debug.Log("Removed all spells.");
        }

        GUILayout.Space(10);

        GUILayout.Label("Categories", EditorStyles.boldLabel);

        if (GUILayout.Button("Assign Offensive"))
            AssignSpells(database, category: SpellCategory.Offensive);

        if (GUILayout.Button("Assign Defensive"))
            AssignSpells(database, category: SpellCategory.Defensive);

        if (GUILayout.Button("Assign Disable"))
            AssignSpells(database, category: SpellCategory.Disable);

        if (GUILayout.Button("Assign Utility"))
            AssignSpells(database, category: SpellCategory.Utility);

        if (GUILayout.Button("Assign Ultimate"))
            AssignSpells(database, ultimateOnly: true);

        GUILayout.Space(10);

        GUILayout.Label("Mastery", EditorStyles.boldLabel);

        if (GUILayout.Button("Assign Apprentice"))
            AssignSpells(database, mastery: SpellMastery.Apprentice);

        if (GUILayout.Button("Assign Adept"))
            AssignSpells(database, mastery: SpellMastery.Adept);

        if (GUILayout.Button("Assign Master"))
            AssignSpells(database, mastery: SpellMastery.Master);

        if (GUILayout.Button("Assign Archmage"))
            AssignSpells(database, mastery: SpellMastery.Archmage);

        GUILayout.Space(15);

        GUILayout.Label("Database Statistics", EditorStyles.boldLabel);

        Spell[] allSpells = AssetDatabase
            .FindAssets("t:Spell")
            .Select(guid =>
                AssetDatabase.LoadAssetAtPath<Spell>(
                    AssetDatabase.GUIDToAssetPath(guid)))
            .ToArray();

        GUILayout.Label($"Total Spells: {allSpells.Length}");

        GUILayout.Space(5);

        GUILayout.Label("Categories", EditorStyles.boldLabel);

        DrawCategoryCount(allSpells, SpellCategory.Offensive);
        DrawCategoryCount(allSpells, SpellCategory.Defensive);
        DrawCategoryCount(allSpells, SpellCategory.Utility);
        DrawCategoryCount(allSpells, SpellCategory.Disable);

        GUILayout.Label(
            $"Ultimate : {allSpells.Count(s => s.Mastery == SpellMastery.Archmage)}");

        GUILayout.Space(5);

        GUILayout.Label("Mastery", EditorStyles.boldLabel);

        DrawMasteryCount(allSpells, SpellMastery.Apprentice);
        DrawMasteryCount(allSpells, SpellMastery.Adept);
        DrawMasteryCount(allSpells, SpellMastery.Master);
        DrawMasteryCount(allSpells, SpellMastery.Archmage);
    }

    private void DrawCategoryCount(
    Spell[] spells,
    SpellCategory category)
    {
        GUILayout.Label(
            $"{category}: {spells.Count(s => s.Category == category)}");
    }

    private void DrawMasteryCount(
        Spell[] spells,
        SpellMastery mastery)
    {
        GUILayout.Label(
            $"{mastery}: {spells.Count(s => s.Mastery == mastery)}");
    }

    private void AssignSpells(
        SpellDatabase database,
        SpellCategory? category = null,
        SpellMastery? mastery = null,
        bool ultimateOnly = false)
    {
        Spell[] spells = AssetDatabase
            .FindAssets("t:Spell")
            .Select(guid =>
                AssetDatabase.LoadAssetAtPath<Spell>(
                    AssetDatabase.GUIDToAssetPath(guid)))
            .Where(spell =>
            {
                if (ultimateOnly)
                    return spell.Mastery == SpellMastery.Archmage;

                if (category.HasValue &&
                    spell.Category != category.Value)
                    return false;

                if (mastery.HasValue &&
                    spell.Mastery != mastery.Value)
                    return false;

                return true;
            })
            .OrderBy(spell => spell.Category)
            .ThenBy(spell => spell.Mastery)
            .ThenBy(spell => spell.spellName)
            .ToArray();

        database.spells = spells;

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();

        Debug.Log($"Assigned {spells.Length} spells.");
    }
}