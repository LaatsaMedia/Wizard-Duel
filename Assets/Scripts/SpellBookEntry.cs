using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellBookEntry : MonoBehaviour, IInspectionProvider
{
    [Header("Slot")]
    [SerializeField] private BuildSlot slot;

    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text spellName;
    [SerializeField] private TMP_Text spellType;
    [SerializeField] private TMP_Text manaCost;

    [Header("Empty")]
    [SerializeField] private string emptyText = "Empty";

    [Header("Mastery Colors")]
    [SerializeField] private RewardMasteryVisual[] masteryVisuals;

    private bool selectable;
    private bool selected;

    // NEW
    private Spell spell;
    public bool CanInspect => spell != null;

    public BuildSlot Slot => slot;

    public void Setup(Spell spell)
    {
        // NEW
        this.spell = spell;

        if (spell == null)
        {
            spellName.text = emptyText;
            spellType.text = "";
            manaCost.text = "";

            spellName.color = Color.white;
            spellType.color = Color.white;

            return;
        }

        icon.sprite = spell.icon;

        spellName.text = spell.spellName;
        spellType.text = spell.Category.ToString();
        manaCost.text = spell.manaCost.ToString("F0");

        ApplyMasteryColor(spell.Mastery);
    }

    private void ApplyMasteryColor(SpellMastery mastery)
    {
        foreach (RewardMasteryVisual visual in masteryVisuals)
        {
            if (visual.mastery != mastery)
                continue;

            spellName.color = visual.textColor;
            spellType.color = visual.textColor;
            return;
        }

        spellName.color = Color.white;
        spellType.color = Color.white;
    }

    public void SetSelectable(bool value)
    {
        selectable = value;
    }

    public void OnClick()
    {
        if (!selectable)
            return;

        SetupPhase.Instance.SelectBuildSlot(slot);
    }

    // NEW
    public InspectionData GetInspectionData()
    {
        return SpellInspectionBuilder.Build(spell);
    }
}