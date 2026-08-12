using UnityEngine;
using UnityEngine.UI;

public class CustomPackAccessorySlot : MonoBehaviour
{
    [Header("Slot")]
    [SerializeField] private AccessoryMastery mastery;

    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    private Accessory accessory;
    private CustomAccessorySelectionPanel selectionPanel;

    public AccessoryMastery Mastery => mastery;
    public Accessory Accessory => accessory;

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
        {
            Debug.LogError(
                "CustomPackAccessorySlot: Selection Panel is NULL.");
            return;
        }

        Debug.Log(
            $"Opening accessory selection for {mastery}");

        selectionPanel.Open(this);
    }
}