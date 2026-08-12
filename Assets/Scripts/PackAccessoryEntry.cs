using UnityEngine;
using UnityEngine.UI;

public class PackAccessoryEntry : MonoBehaviour, IInspectionProvider
{
    [Header("UI")]
    [SerializeField] private Image icon;

    [Header("Selection")]
    [SerializeField] private Button button;

    private Accessory accessory;
    private CustomAccessorySelectionPanel selectionPanel;

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

    public void SetupSelection(
        CustomAccessorySelectionPanel selectionPanel)
    {
        this.selectionPanel = selectionPanel;

        if (button != null)
            button.onClick.AddListener(OnSelectionClicked);
    }

    private void OnSelectionClicked()
    {
        if (selectionPanel == null)
            return;

        selectionPanel.SelectAccessory(accessory);
    }
}