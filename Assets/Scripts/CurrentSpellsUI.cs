using UnityEngine;
using UnityEngine.UI;

public class CurrentSpellsUI : MonoBehaviour
{
    [SerializeField] private Image primarySpell;
    [SerializeField] private Image secondarySpell;
    [SerializeField] private Image thirdSpell;

    [SerializeField] private Sprite emptySlotSprite;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        WizardBuild build = RunManager.Instance.PlayerBuild;

        SetSpell(primarySpell, build.primarySpell);
        SetSpell(secondarySpell, build.secondarySpell);
        SetSpell(thirdSpell, build.thirdSpell);
    }

    private void SetSpell(Image image, Spell spell)
    {
        if (spell == null)
        {
            image.sprite = emptySlotSprite;
            return;
        }

        image.sprite = spell.icon;
    }
}