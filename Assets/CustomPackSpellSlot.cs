using UnityEngine;
using UnityEngine.UI;

public class CustomPackSpellSlot : MonoBehaviour, IInspectionProvider
{
    [Header("Slot")]
    [SerializeField] private SpellMastery mastery;

    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    private CustomSpellSelectionPanel selectionPanel;
    private CustomPackEditor customPackEditor;

    public void SetCustomPackEditor(CustomPackEditor customPackEditor)
    {
        this.customPackEditor = customPackEditor;
    }

    public SpellMastery Mastery => mastery;
    public Spell Spell => spell;
    public bool CanInspect => spell != null;

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

    public InspectionData GetInspectionData()
    {
        if (spell == null)
            return new InspectionData();

        return SpellInspectionBuilder.Build(spell);
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

        if (customPackEditor != null)
            customPackEditor.gameObject.SetActive(false);

        selectionPanel.Open(this);
    }
}