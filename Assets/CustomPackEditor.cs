using UnityEngine;
using UnityEngine.UI;

public class CustomPackEditor : MonoBehaviour
{
    public static CustomPackEditor Instance { get; private set; }

    [Header("Current Pack")]
    [SerializeField] private CustomPackData customPack;

    [Header("Identity")]
    [SerializeField] private TMPro.TMP_InputField packNameInput;
    [SerializeField] private TMPro.TMP_InputField packDescriptionInput;
    [SerializeField] private Image packIconImage;
    [SerializeField] private CustomPackIconSelectionPanel iconSelectionPanel;

    [Header("Spell Slot Prefab")]
    [SerializeField] private CustomPackSpellSlot spellSlotPrefab;

    [Header("Accessory Slot Prefab")]
    [SerializeField] private CustomPackAccessorySlot accessorySlotPrefab;

    [Header("Spell Parents")]
    [SerializeField] private Transform apprenticeParent;
    [SerializeField] private Transform adeptParent;
    [SerializeField] private Transform masterParent;
    [SerializeField] private Transform archmageParent;

    [Header("Accessory Parents")]
    [SerializeField] private Transform accessoryParent;

    [Header("Selection")]
    [SerializeField] private CustomSpellSelectionPanel spellSelectionPanel;
    [SerializeField] private CustomAccessorySelectionPanel accessorySelectionPanel;

    [Header("Save")]
    [SerializeField] private Button saveButton;

    [Header("Navigation")]
    [SerializeField] private GameObject packSelectionPanel;

    [Header("Inspection")]
    [SerializeField] private InformationPanel informationPanel;

    public CustomPackData CustomPack => customPack;
    public InformationPanel InformationPanel => informationPanel;
    private CustomPackSelectionEntry selectionEntry;

    private void Awake()
    {
        Instance = this;

        if (saveButton != null)
            saveButton.onClick.AddListener(Save);
    }

    private void Start()
    {
        PopulateSpellSlots();
        PopulateAccessorySlots();
    }

    public void Open(
        CustomPackData pack,
        CustomPackSelectionEntry selectionEntry)
    {
        customPack = pack;
        this.selectionEntry = selectionEntry;

        if (InspectionManager.Instance != null)
            InspectionManager.Instance.SetPanel(informationPanel);

        if (packSelectionPanel != null)
            packSelectionPanel.SetActive(false);

        if (packNameInput != null)
            packNameInput.text = customPack != null
                ? customPack.packName
                : "";

        if (packDescriptionInput != null)
            packDescriptionInput.text = customPack != null
                ? customPack.description
                : "";

        if (packIconImage != null)
            packIconImage.sprite = customPack != null
                ? customPack.icon
                : null;

        gameObject.SetActive(true);

        PopulateSpellSlots();
        PopulateAccessorySlots();
    }

    private void PopulateSpellSlots()
    {
        ClearSpellSlots();

        CreateSlots(
            SpellMastery.Apprentice,
            5,
            apprenticeParent);

        CreateSlots(
            SpellMastery.Adept,
            4,
            adeptParent);

        CreateSlots(
            SpellMastery.Master,
            3,
            masterParent);

        CreateSlots(
            SpellMastery.Archmage,
            2,
            archmageParent);
    }

    private void PopulateAccessorySlots()
    {
        ClearAccessorySlots();

        CreateAccessorySlots(
            AccessoryMastery.Common,
            6);

        CreateAccessorySlots(
            AccessoryMastery.Rare,
            6);

        CreateAccessorySlots(
            AccessoryMastery.Epic,
            4);
    }

    private void CreateSlots(
        SpellMastery mastery,
        int count,
        Transform parent)
    {
        if (spellSlotPrefab == null || parent == null)
            return;

        int slotIndex = 0;

        foreach (Spell spell in customPack.spells)
        {
            if (spell == null)
                continue;

            if (spell.Mastery != mastery)
                continue;

            if (slotIndex >= count)
                break;

            CustomPackSpellSlot slot =
                Instantiate(spellSlotPrefab, parent);

            slot.SetMastery(mastery);
            slot.SetSelectionPanel(spellSelectionPanel);
            slot.Setup(spell);
            slot.SetCustomPackEditor(this);

            slotIndex++;
        }

        while (slotIndex < count)
        {
            CustomPackSpellSlot slot =
                Instantiate(spellSlotPrefab, parent);

            slot.SetMastery(mastery);
            slot.SetSelectionPanel(spellSelectionPanel);
            slot.SetCustomPackEditor(this);

            slotIndex++;
        }
    }

    private void ClearSpellSlots()
    {
        ClearParent(apprenticeParent);
        ClearParent(adeptParent);
        ClearParent(masterParent);
        ClearParent(archmageParent);
    }

    private void CreateAccessorySlots(
        AccessoryMastery mastery,
        int count)
    {
        if (accessorySlotPrefab == null || accessoryParent == null)
            return;

        int slotIndex = 0;

        foreach (Accessory accessory in customPack.accessories)
        {
            if (accessory == null)
                continue;

            if (accessory.Mastery != mastery)
                continue;

            if (slotIndex >= count)
                break;

            CustomPackAccessorySlot slot =
                Instantiate(
                    accessorySlotPrefab,
                    accessoryParent);

            slot.SetMastery(mastery);
            slot.SetSelectionPanel(accessorySelectionPanel);
            slot.Setup(accessory);
            slot.SetCustomPackEditor(this);

            slotIndex++;
        }

        while (slotIndex < count)
        {
            CustomPackAccessorySlot slot =
                Instantiate(
                    accessorySlotPrefab,
                    accessoryParent);

            slot.SetMastery(mastery);
            slot.SetSelectionPanel(accessorySelectionPanel);
            slot.SetCustomPackEditor(this);

            slotIndex++;
        }
    }

    public void ReplaceAccessory(
    CustomPackAccessorySlot slot,
    Accessory newAccessory)
    {
        if (customPack == null)
            return;

        if (slot == null || newAccessory == null)
            return;

        Accessory oldAccessory = slot.Accessory;

        if (oldAccessory == newAccessory)
            return;

        if (customPack.accessories.Contains(newAccessory))
            return;

        if (oldAccessory != null)
            customPack.accessories.Remove(oldAccessory);

        customPack.accessories.Add(newAccessory);
    }

    private void ClearAccessorySlots()
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

    public void AddSpell(Spell spell)
    {
        if (customPack == null)
            return;

        if (spell == null)
            return;

        if (customPack.spells.Contains(spell))
            return;

        customPack.spells.Add(spell);

        Debug.Log($"Custom Pack: Added {spell.spellName}");
    }

    public void ReplaceSpell(
    CustomPackSpellSlot slot,
    Spell newSpell)
    {
        if (customPack == null)
            return;

        if (slot == null || newSpell == null)
            return;

        Spell oldSpell = slot.Spell;

        if (oldSpell == newSpell)
            return;

        if (customPack.spells.Contains(newSpell))
            return;

        if (oldSpell != null)
            customPack.spells.Remove(oldSpell);

        customPack.spells.Add(newSpell);
    }

    public void OpenIconSelection()
    {
        if (customPack == null)
            return;

        if (iconSelectionPanel == null)
            return;

        iconSelectionPanel.Open(this);
    }

    public void SetPackIcon(Sprite icon)
    {
        if (customPack == null)
            return;

        customPack.icon = icon;

        if (packIconImage != null)
            packIconImage.sprite = icon;
    }

    public void ReturnToPackSelection()
    {
        if (packSelectionPanel != null)
            packSelectionPanel.SetActive(true);

        gameObject.SetActive(false);
    }

    public void Save()
    {
        if (customPack == null)
            return;

        if (packNameInput != null)
            customPack.packName = packNameInput.text;

        if (packDescriptionInput != null)
            customPack.description = packDescriptionInput.text;

        if (CustomPackManager.Instance != null)
        {
            CustomPackManager.Instance.AddPack(customPack);
        }

        if (selectionEntry != null)
            selectionEntry.Refresh();

        if (packSelectionPanel != null)
            packSelectionPanel.SetActive(true);
        
        gameObject.SetActive(false);
    }
}