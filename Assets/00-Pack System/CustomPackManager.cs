using System.Collections.Generic;
using UnityEngine;

public class CustomPackManager : MonoBehaviour
{
    public static CustomPackManager Instance { get; private set; }

    [Header("Custom Pack UI")]
    [SerializeField] private Transform content;
    [SerializeField] private CustomPackSelectionEntry entryPrefab;

    [Header("Scene References")]
    [SerializeField] private PackPreviewPanel packPreviewPanel;
    [SerializeField] private CustomPackEditor customPackEditor;
    [SerializeField] private PackSelectionUI packSelectionUI;
    
    [Header("Create Pack")]
    [SerializeField] private RectTransform createNewPackButton;

    private readonly List<CustomPackData> customPacks = new();

    public IReadOnlyList<CustomPackData> CustomPacks => customPacks;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        LoadSavedPacks();
    }

    private void LoadSavedPacks()
    {
        if (CustomPackSaveSystem.Instance == null)
            return;

        List<CustomPackData> loadedPacks =
            CustomPackSaveSystem.Instance.LoadPacks();

        foreach (CustomPackData pack in loadedPacks)
        {
            AddPackWithoutSaving(pack);
        }
    }

    public void AddPack(CustomPackData pack)
    {
        if (pack == null)
            return;

        if (!customPacks.Contains(pack))
        {
            customPacks.Add(pack);
            CreateEntry(pack);
        }

        Save();
    }

    public void DeletePack(CustomPackData pack)
    {
        if (pack == null)
            return;

        if (!customPacks.Contains(pack))
            return;

        customPacks.Remove(pack);

        Save();
    }

    private void AddPackWithoutSaving(CustomPackData pack)
    {
        if (pack == null)
            return;

        if (customPacks.Contains(pack))
            return;

        customPacks.Add(pack);

        CreateEntry(pack);
    }

    private void Save()
    {
        if (CustomPackSaveSystem.Instance == null)
            return;

        CustomPackSaveSystem.Instance.SavePacks(customPacks);
    }

    private void CreateEntry(CustomPackData pack)
    {
        if (entryPrefab == null || content == null)
            return;

        CustomPackSelectionEntry entry =
            Instantiate(entryPrefab, content);

        entry.Setup(
            pack,
            packPreviewPanel,
            customPackEditor,
            packSelectionUI);

        if (createNewPackButton != null)
            createNewPackButton.SetAsLastSibling();
    }
}