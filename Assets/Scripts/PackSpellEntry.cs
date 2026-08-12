using UnityEngine;
using UnityEngine.UI;

public class PackSpellEntry : MonoBehaviour, IInspectionProvider
{
    [Header("UI")]
    [SerializeField] private Image icon;

    private Spell spell;

    [Header("Selection")]
    [SerializeField] private Button button;

    private CustomSpellSelectionPanel selectionPanel;

    public bool CanInspect => spell != null;

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

    public void SetupSelection(CustomSpellSelectionPanel selectionPanel)
    {
        this.selectionPanel = selectionPanel;

        if (button != null)
            button.onClick.AddListener(OnSelectionClicked);
    }

    private void OnSelectionClicked()
    {
        if (selectionPanel == null)
            return;

        selectionPanel.SelectSpell(spell);
    }
}