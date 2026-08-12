using UnityEngine;
using UnityEngine.UI;

public class CustomPackSpellSlot : MonoBehaviour
{
    [Header("Slot")]
    [SerializeField] private SpellMastery mastery;

    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    private CustomSpellSelectionPanel selectionPanel;

    public SpellMastery Mastery => mastery;
    public Spell Spell => spell;

    private Spell spell;

    public void Setup(Spell spell)
    {
        this.spell = spell;

        if (spell == null)
        {
            icon.sprite = null;
            return;
        }

        icon.sprite = spell.icon;
    }

    public void Clear()
    {
        spell = null;
        icon.sprite = null;
    }

    public void SetMastery(SpellMastery mastery)
    {
        this.mastery = mastery;
    }

    public void SetSelectionPanel(CustomSpellSelectionPanel selectionPanel)
    {
        this.selectionPanel = selectionPanel;
    }

    public void OnClick()
    {
        if (selectionPanel == null)
            return;

        selectionPanel.Open(this);
    }
}