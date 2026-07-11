using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image cardImage;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text type;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private GameObject costIcon;
    [SerializeField] private TMP_Text accessoryText;
    [SerializeField] private Image frameImage;

    [Header("Frame Visuals")]
    [SerializeField] private RewardMasteryVisual[] masteryVisuals;
    [SerializeField] private RewardAccessoryVisual[] accessoryVisuals;

    private Vector3 originalScale;

    private Spell spell;
    public Spell Spell => spell;

    private Accessory accessory;
    public Accessory Accessory => accessory;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void Setup(Spell spell)
    {
        this.spell = spell;

        icon.sprite = spell.icon;
        title.text = spell.spellName;
        type.text = spell.Category.ToString();
        description.text = spell.description;
        cost.text = spell.manaCost.ToString("F0");

        type.gameObject.SetActive(true);
        cost.gameObject.SetActive(true);
        costIcon.gameObject.SetActive(true);
        accessoryText.gameObject.SetActive(false);

        UpdateSpellFrame(spell.Mastery);
    }

    public void Setup(Accessory accessory)
    {
        this.accessory = accessory;

        icon.sprite = accessory.icon;
        title.text = accessory.accessoryName;
        description.text = accessory.description;

        type.gameObject.SetActive(false);
        cost.gameObject.SetActive(false);
        costIcon.gameObject.SetActive(false);
        accessoryText.gameObject.SetActive(true);

        UpdateAccessoryFrame(accessory.Mastery);
    }

    private void UpdateSpellFrame(SpellMastery mastery)
    {
        foreach (RewardMasteryVisual visual in masteryVisuals)
        {
            if (visual.mastery != mastery)
                continue;

            frameImage.sprite = visual.frameSprite;
            return;
        }
    }

    private void UpdateAccessoryFrame(AccessoryMastery mastery)
    {
        foreach (RewardAccessoryVisual visual in accessoryVisuals)
        {
            if (visual.mastery != mastery)
                continue;

            frameImage.sprite = visual.frameSprite;
            return;
        }
    }

    public void OnClick()
    {
        SetupPhase.Instance.SelectCard(this);
    }

    public void SetSelected(bool selected)
    {
        transform.localScale = selected
            ? originalScale * 1.1f
            : originalScale;
    }
}