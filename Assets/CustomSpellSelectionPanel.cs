using UnityEngine;

public class CustomSpellSelectionPanel : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private SpellDatabase spellDatabase;

    [Header("UI")]
    [SerializeField] private Transform content;

    [Header("Prefab")]
    [SerializeField] private PackSpellEntry spellEntryPrefab;

    [Header("Editor")]
    [SerializeField] private CustomPackEditor editor;

    private CustomPackSpellSlot targetSlot;

    public void Open(CustomPackSpellSlot slot)
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

        if (spellDatabase == null)
            return;

        foreach (Spell spell in spellDatabase.spells)
        {
            if (spell == null)
                continue;

            if (spell.Mastery != targetSlot.Mastery)
                continue;

            if (editor.CustomPack.spells.Contains(spell) &&
                spell != targetSlot.Spell)
            {
                continue;
            }

            PackSpellEntry entry =
                Instantiate(spellEntryPrefab, content);

            entry.Setup(spell);
            entry.SetupSelection(this);
        }
    }

    public void SelectSpell(Spell spell)
    {
        if (spell == null)
            return;

        if (targetSlot == null)
            return;

        editor.ReplaceSpell(targetSlot, spell);

        targetSlot.Setup(spell);

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