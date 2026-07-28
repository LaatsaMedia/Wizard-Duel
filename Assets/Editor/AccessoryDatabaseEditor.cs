using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AccessoryDatabase))]
public class AccessoryDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        AccessoryDatabase database = (AccessoryDatabase)target;

        if (GUILayout.Button("Assign All"))
        {
            AssignAccessories(database);
        }

        if (GUILayout.Button("Remove All"))
        {
            database.accessories = new Accessory[0];

            EditorUtility.SetDirty(database);

            Debug.Log("Removed all accessories.");
        }

        GUILayout.Space(10);

        GUILayout.Label("Mastery", EditorStyles.boldLabel);

        if (GUILayout.Button("Assign Common"))
            AssignAccessories(database, AccessoryMastery.Common);

        if (GUILayout.Button("Assign Rare"))
            AssignAccessories(database, AccessoryMastery.Rare);

        if (GUILayout.Button("Assign Epic"))
            AssignAccessories(database, AccessoryMastery.Epic);

        //if (GUILayout.Button("Assign Legendary"))
        //    AssignAccessories(database, AccessoryMastery.Legendary);

        GUILayout.Space(15);

        GUILayout.Label("Database Statistics", EditorStyles.boldLabel);

        Accessory[] allAccessories = AssetDatabase
            .FindAssets("t:Accessory")
            .Select(guid =>
                AssetDatabase.LoadAssetAtPath<Accessory>(
                    AssetDatabase.GUIDToAssetPath(guid)))
            .ToArray();

        GUILayout.Label($"Total Accessories: {allAccessories.Length}");

        GUILayout.Space(5);

        GUILayout.Label("Mastery", EditorStyles.boldLabel);

        DrawMasteryCount(allAccessories, AccessoryMastery.Common);
        DrawMasteryCount(allAccessories, AccessoryMastery.Rare);
        DrawMasteryCount(allAccessories, AccessoryMastery.Epic);
        //DrawMasteryCount(allAccessories, AccessoryMastery.Legendary);
    }

    private void DrawMasteryCount(
        Accessory[] accessories,
        AccessoryMastery mastery)
    {
        GUILayout.Label(
            $"{mastery}: {accessories.Count(a => a.Mastery == mastery)}");
    }

    private void AssignAccessories(
        AccessoryDatabase database,
        AccessoryMastery? mastery = null)
    {
        Accessory[] accessories = AssetDatabase
            .FindAssets("t:Accessory")
            .Select(guid =>
                AssetDatabase.LoadAssetAtPath<Accessory>(
                    AssetDatabase.GUIDToAssetPath(guid)))
            .Where(accessory =>
            {
                if (mastery.HasValue &&
                    accessory.Mastery != mastery.Value)
                    return false;

                return true;
            })
            .OrderBy(accessory => accessory.Mastery)
            .ThenBy(accessory => accessory.name)
            .ToArray();

        database.accessories = accessories;

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();

        Debug.Log($"Assigned {accessories.Length} accessories.");
    }
}