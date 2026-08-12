using UnityEngine;

public class CustomAccessorySelectionPanel : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private AccessoryDatabase accessoryDatabase;

    [Header("UI")]
    [SerializeField] private Transform content;

    [Header("Prefab")]
    [SerializeField] private PackAccessoryEntry accessoryEntryPrefab;

    [Header("Editor")]
    [SerializeField] private CustomPackEditor editor;

    private CustomPackAccessorySlot targetSlot;

    public void Open(CustomPackAccessorySlot slot)
    {
        targetSlot = slot;

        gameObject.SetActive(true);

        Populate();
    }

    private void Populate()
    {
        Clear();

        if (targetSlot == null)
            return;

        if (accessoryDatabase == null)
            return;

        foreach (Accessory accessory in accessoryDatabase.accessories)
        {
            if (accessory == null)
                continue;

            if (accessory.Mastery != targetSlot.Mastery)
                continue;

            if (editor.CustomPack.accessories.Contains(accessory) &&
                accessory != targetSlot.Accessory)
            {
                continue;
            }

            PackAccessoryEntry entry =
                Instantiate(accessoryEntryPrefab, content);

            entry.Setup(accessory);
            entry.SetupSelection(this);
        }
    }

    public void SelectAccessory(Accessory accessory)
    {
        if (accessory == null)
            return;

        if (targetSlot == null)
            return;

        editor.ReplaceAccessory(targetSlot, accessory);

        targetSlot.Setup(accessory);

        gameObject.SetActive(false);
    }

    private void Clear()
    {
        if (content == null)
            return;

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}