using System.Collections.Generic;
using UnityEngine;

public class CustomPackIconSelectionPanel : MonoBehaviour
{
    [Header("Databases")]
    [SerializeField] private SpellDatabase spellDatabase;
    [SerializeField] private AccessoryDatabase accessoryDatabase;

    [Header("UI")]
    [SerializeField] private Transform content;

    [Header("Prefab")]
    [SerializeField] private CustomPackIconEntry iconEntryPrefab;

    private CustomPackEditor editor;

    public void Open(CustomPackEditor editor)
    {
        this.editor = editor;

        gameObject.SetActive(true);

        Populate();
    }

    private void Populate()
    {
        Clear();

        if (spellDatabase == null)
            return;

        if (accessoryDatabase == null)
            return;

        if (iconEntryPrefab == null)
            return;

        foreach (Spell spell in spellDatabase.spells)
        {
            if (spell == null)
                continue;

            if (spell.icon == null)
                continue;

            CustomPackIconEntry entry =
                Instantiate(
                    iconEntryPrefab,
                    content);

            entry.Setup(spell.icon, this);
        }

        foreach (Accessory accessory in accessoryDatabase.accessories)
        {
            if (accessory == null)
                continue;

            if (accessory.icon == null)
                continue;

            CustomPackIconEntry entry =
                Instantiate(
                    iconEntryPrefab,
                    content);

            entry.Setup(accessory.icon, this);
        }
    }

    public void SelectIcon(Sprite icon)
    {
        if (icon == null)
            return;

        if (editor == null)
            return;

        editor.SetPackIcon(icon);

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