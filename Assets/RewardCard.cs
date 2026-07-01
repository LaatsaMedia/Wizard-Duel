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

    [Header("Selection")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite selectedSprite;

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
        cost.text = $"Mana Cost: {spell.manaCost}";
    }

    public void Setup(Accessory accessory)
    {
        this.accessory = accessory;

        icon.sprite = accessory.icon;
        title.text = accessory.accessoryName;
        type.text = "Accessory";
        description.text = accessory.description;
        cost.text = "";
    }

    public void OnClick()
    {
        SetupPhase.Instance.SelectCard(this);
    }

    public void SetSelected(bool selected)
    {
        cardImage.sprite = selected
            ? selectedSprite
            : normalSprite;

        transform.localScale = selected
            ? originalScale * 1.05f
            : originalScale;
    }
}