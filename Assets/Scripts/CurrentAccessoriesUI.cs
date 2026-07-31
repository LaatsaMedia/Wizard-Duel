using UnityEngine;
using UnityEngine.UI;

public class CurrentAccessoriesUI : MonoBehaviour
{
    [SerializeField] private AccessorySlot accessory1;
    [SerializeField] private AccessorySlot accessory2;
    [SerializeField] private AccessorySlot accessory3;
    [SerializeField] private AccessorySlot accessory4;
    [SerializeField] private AccessorySlot accessory5;
    [SerializeField] private AccessorySlot accessory6;

    [SerializeField] private Sprite emptySlotSprite;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        WizardBuild build = RunManager.Instance.PlayerBuild;

        accessory1.Setup(build.accessory1);
        accessory2.Setup(build.accessory2);
        accessory3.Setup(build.accessory3);
        accessory4.Setup(build.accessory4);
        accessory5.Setup(build.accessory5);
        accessory6.Setup(build.accessory6);
    }

    private void SetAccessory(Image image, Accessory accessory)
    {
        if (accessory == null)
        {
            image.sprite = emptySlotSprite;
            return;
        }

        image.sprite = accessory.icon;
    }
}