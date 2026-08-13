using System.Collections.Generic;
using UnityEngine;

public class PackPreviewPanel : MonoBehaviour
{
    [Header("Pack")]
    [SerializeField] private PackDefinition pack;

    [Header("Database")]
    [SerializeField] private SpellDatabase spellDatabase;
    [SerializeField] private AccessoryDatabase accessoryDatabase;

    [Header("Spell Prefab")]
    [SerializeField] private PackSpellEntry spellEntryPrefab;

    [Header("Accessory Prefab")]
    [SerializeField] private PackAccessoryEntry accessoryEntryPrefab;

    [Header("Spell Parents")]
    [SerializeField] private Transform apprenticeParent;
    [SerializeField] private Transform adeptParent;
    [SerializeField] private Transform masterParent;
    [SerializeField] private Transform archmageParent;

    [Header("Accessory Parent")]
    [SerializeField] private Transform accessoryParent;

    [Header("Navigation")]
    [SerializeField] private GameObject packSelectionPanel;

    [Header("Inspection")]
    [SerializeField] private InformationPanel informationPanel;

    private PackSelectionButton selectionButton;
    private CustomPackData customPack;

    public void Open(
        PackDefinition pack,
        PackSelectionButton selectionButton)
    {
        this.selectionButton = selectionButton;
        this.customPack = null;

        if (InspectionManager.Instance != null)
            InspectionManager.Instance.SetPanel(informationPanel);

        if (packSelectionPanel != null)
            packSelectionPanel.SetActive(false);

        gameObject.SetActive(true);
        ShowPack(pack);
    }

    public void OpenCustomPack(CustomPackData customPack)
    {
        this.customPack = customPack;
        this.selectionButton = null;

        if (InspectionManager.Instance != null)
            InspectionManager.Instance.SetPanel(informationPanel);

        if (packSelectionPanel != null)
            packSelectionPanel.SetActive(false);

        gameObject.SetActive(true);

        ShowCustomPack(customPack);
    }

    public void InspectPack()
    {
        ShowPack(pack);
    }

    public void ShowPack(PackDefinition pack)
    {
        this.pack = pack;

        ClearSpellEntries();
        ClearAccessoryEntries();

        if (pack == null)
            return;

        PopulateSpells();
        PopulateAccessories();
    }

    private void ShowCustomPack(CustomPackData customPack)
    {
        ClearSpellEntries();
        ClearAccessoryEntries();

        if (customPack == null)
            return;

        foreach (Spell spell in customPack.spells)
        {
            if (spell == null)
                continue;

            Transform parent = GetParentForMastery(spell.Mastery);

            if (parent == null)
                continue;

            Instantiate(
                spellEntryPrefab,
                parent)
                .Setup(spell);
        }

        foreach (Accessory accessory in customPack.accessories)
        {
            if (accessory == null)
                continue;

            Instantiate(
                accessoryEntryPrefab,
                accessoryParent)
                .Setup(accessory);
        }
    }

    private void PopulateSpells()
    {
        if (spellDatabase == null)
        {
            return;
        }

        if (spellEntryPrefab == null)
        {
            return;
        }

        if (pack == null)
        {
            return;
        }

        foreach (Spell spell in spellDatabase.spells)
        {
            if (spell == null)
                continue;

            if (spell.OriginPack != pack)
                continue;

            Transform parent = GetParentForMastery(spell.Mastery);

            if (parent == null)
            {
                continue;
            }


            Instantiate(
                spellEntryPrefab,
                parent)
                .Setup(spell);
        }
    }

    private void PopulateAccessories()
    {
        if (accessoryDatabase == null)
        {
            Debug.LogError(
                "PackPreviewPanel: Accessory Database is not assigned.");
            return;
        }

        if (accessoryEntryPrefab == null)
        {
            Debug.LogError(
                "PackPreviewPanel: Accessory Entry Prefab is not assigned.");
            return;
        }

        foreach (Accessory accessory in accessoryDatabase.accessories)
        {
            if (accessory == null)
                continue;

            if (accessory.OriginPack != pack)
                continue;

            Instantiate(
                accessoryEntryPrefab,
                accessoryParent)
                .Setup(accessory);
        }
    }

    private Transform GetParentForMastery(SpellMastery mastery)
    {
        switch (mastery)
        {
            case SpellMastery.Apprentice:
                return apprenticeParent;

            case SpellMastery.Adept:
                return adeptParent;

            case SpellMastery.Master:
                return masterParent;

            case SpellMastery.Archmage:
                return archmageParent;

            default:
                return null;
        }
    }

    private void ClearSpellEntries()
    {
        ClearParent(apprenticeParent);
        ClearParent(adeptParent);
        ClearParent(masterParent);
        ClearParent(archmageParent);
    }

    private void ClearAccessoryEntries()
    {
        ClearParent(accessoryParent);
    }

    private void ClearParent(Transform parent)
    {
        if (parent == null)
            return;

        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void ReturnToPackSelection()
    {
        Debug.Log("PackPreviewPanel: RETURN clicked.");

        if (InspectionManager.Instance != null)
            InspectionManager.Instance.Hide();

        packSelectionPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SelectCurrentPack()
    {
        Debug.Log("PackPreviewPanel: SELECT clicked.");

        // CUSTOM PACK
        if (customPack != null)
        {
            if (!customPack.IsComplete)
            {
                Debug.LogWarning(
                    "PackPreviewPanel: Custom pack is not complete.");
                return;
            }

            if (PackSelectionUI.Instance != null)
            {
                PackSelectionUI.Instance.SelectCustomPack(customPack);
            }

            packSelectionPanel.SetActive(true);
            gameObject.SetActive(false);

            return;
        }

        // BUILT-IN PACK
        if (selectionButton == null)
        {
            Debug.LogWarning(
                "PackPreviewPanel: Selection Button is NULL.");
            return;
        }

        selectionButton.Select();

        packSelectionPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}