using UnityEngine;
using UnityEngine.UI;

public class AccessorySlot : MonoBehaviour, IInspectionProvider
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite emptySlotSprite;

    private Accessory accessory;

    public void Setup(Accessory accessory)
    {
        this.accessory = accessory;

        icon.sprite = accessory != null
            ? accessory.icon
            : emptySlotSprite;
    }

    public bool CanInspect => accessory != null;

    public InspectionData GetInspectionData()
    {
        return AccessoryInspectionBuilder.Build(accessory);
    }
}