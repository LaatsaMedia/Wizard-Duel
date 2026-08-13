using UnityEngine;
using UnityEngine.UI;

public class CustomPackAccessorySlot : MonoBehaviour, IInspectionProvider
{
    [Header("Slot")]
    [SerializeField] private AccessoryMastery mastery;

    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    private Accessory accessory;
    private CustomAccessorySelectionPanel selectionPanel;
    private CustomPackEditor customPackEditor;

    public void SetCustomPackEditor(CustomPackEditor customPackEditor)
    {
        this.customPackEditor = customPackEditor;
    }

    public AccessoryMastery Mastery => mastery;
    public Accessory Accessory => accessory;
    public bool CanInspect => accessory != null;

    public void Setup(Accessory accessory)
    {
        this.accessory = accessory;

        if (accessory == null)
        {
            icon.sprite = null;
            return;
        }

        icon.sprite = accessory.icon;
    }

    public InspectionData GetInspectionData()
    {
        if (accessory == null)
            return new InspectionData();

        return AccessoryInspectionBuilder.Build(accessory);
    }

    public void Clear()
    {
        accessory = null;
        icon.sprite = null;
    }

    public void SetMastery(AccessoryMastery mastery)
    {
        this.mastery = mastery;
    }

    public void SetSelectionPanel(
        CustomAccessorySelectionPanel selectionPanel)
    {
        this.selectionPanel = selectionPanel;
    }

    public void OnClick()
    {
        if (selectionPanel == null)
            return;

        if (customPackEditor != null)
            customPackEditor.gameObject.SetActive(false);

        selectionPanel.Open(this);
    }
}