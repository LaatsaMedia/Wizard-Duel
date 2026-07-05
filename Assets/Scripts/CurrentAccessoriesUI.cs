using UnityEngine;
using UnityEngine.UI;

public class CurrentAccessoriesUI : MonoBehaviour
{
    [SerializeField] private Image accessory1;
    [SerializeField] private Image accessory2;
    [SerializeField] private Image accessory3;
    [SerializeField] private Image accessory4;
    [SerializeField] private Image accessory5;
    [SerializeField] private Image accessory6;

    [SerializeField] private Sprite emptySlotSprite;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        WizardBuild build = RunManager.Instance.PlayerBuild;

        SetAccessory(accessory1, build.accessory1);
        SetAccessory(accessory2, build.accessory2);
        SetAccessory(accessory3, build.accessory3);
        SetAccessory(accessory4, build.accessory4);
        SetAccessory(accessory5, build.accessory5);
        SetAccessory(accessory6, build.accessory6);
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