using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CustomPackSaveSystem : MonoBehaviour
{
    public static CustomPackSaveSystem Instance { get; private set; }

    [Header("Databases")]
    [SerializeField] private SpellDatabase spellDatabase;
    [SerializeField] private AccessoryDatabase accessoryDatabase;

    private const string FileName = "custom_packs.json";

    private string SavePath =>
        Path.Combine(Application.persistentDataPath, FileName);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void SavePacks(
        IReadOnlyList<CustomPackData> customPacks)
    {
        if (customPacks == null)
            return;

        CustomPackSaveFile saveFile =
            new CustomPackSaveFile();

        foreach (CustomPackData pack in customPacks)
        {
            if (pack == null)
                continue;

            CustomPackSaveData saveData =
                new CustomPackSaveData();

            saveData.packName = pack.packName;
            saveData.description = pack.description;

            foreach (Spell spell in pack.spells)
            {
                if (spell == null)
                    continue;

                saveData.spells.Add(spell.spellName);
            }

            foreach (Accessory accessory in pack.accessories)
            {
                if (accessory == null)
                    continue;

                saveData.accessories.Add(accessory.accessoryName);
            }

            saveFile.packs.Add(saveData);
        }

        string json =
            JsonUtility.ToJson(saveFile, true);

        File.WriteAllText(SavePath, json);

        Debug.Log(
            $"CustomPackSaveSystem: Saved {saveFile.packs.Count} custom packs.");
    }

    public List<CustomPackData> LoadPacks()
    {
        List<CustomPackData> loadedPacks =
            new List<CustomPackData>();

        if (!File.Exists(SavePath))
        {
            Debug.Log(
                "CustomPackSaveSystem: No save file found.");

            return loadedPacks;
        }

        string json =
            File.ReadAllText(SavePath);

        CustomPackSaveFile saveFile =
            JsonUtility.FromJson<CustomPackSaveFile>(json);

        if (saveFile == null || saveFile.packs == null)
            return loadedPacks;

        foreach (CustomPackSaveData saveData in saveFile.packs)
        {
            if (saveData == null)
                continue;

            CustomPackData pack =
                ScriptableObject.CreateInstance<CustomPackData>();

            pack.packName = saveData.packName;
            pack.description = saveData.description;

            LoadSpells(saveData, pack);
            LoadAccessories(saveData, pack);

            loadedPacks.Add(pack);
        }

        Debug.Log(
            $"CustomPackSaveSystem: Loaded {loadedPacks.Count} custom packs.");

        return loadedPacks;
    }

    private void LoadSpells(
        CustomPackSaveData saveData,
        CustomPackData pack)
    {
        if (spellDatabase == null)
            return;

        foreach (string spellName in saveData.spells)
        {
            Spell spell =
                FindSpell(spellName);

            if (spell == null)
            {
                Debug.LogWarning(
                    $"CustomPackSaveSystem: Could not find spell '{spellName}'.");
                continue;
            }

            if (!pack.spells.Contains(spell))
                pack.spells.Add(spell);
        }
    }

    private void LoadAccessories(
        CustomPackSaveData saveData,
        CustomPackData pack)
    {
        if (accessoryDatabase == null)
            return;

        foreach (string accessoryName in saveData.accessories)
        {
            Accessory accessory =
                FindAccessory(accessoryName);

            if (accessory == null)
            {
                Debug.LogWarning(
                    $"CustomPackSaveSystem: Could not find accessory '{accessoryName}'.");
                continue;
            }

            if (!pack.accessories.Contains(accessory))
                pack.accessories.Add(accessory);
        }
    }

    private Spell FindSpell(string spellName)
    {
        foreach (Spell spell in spellDatabase.spells)
        {
            if (spell == null)
                continue;

            if (spell.spellName == spellName)
                return spell;
        }

        return null;
    }

    private Accessory FindAccessory(string accessoryName)
    {
        foreach (Accessory accessory in accessoryDatabase.accessories)
        {
            if (accessory == null)
                continue;

            if (accessory.accessoryName == accessoryName)
                return accessory;
        }

        return null;
    }
}