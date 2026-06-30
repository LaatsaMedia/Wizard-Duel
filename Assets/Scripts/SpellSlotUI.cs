using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlotUI : MonoBehaviour
{
    [SerializeField] private Image spellIcon;
    [SerializeField] private TMP_Text manaCostText;

    [SerializeField] private Image cooldownBar;
    [SerializeField] private GameObject offCooldown;

    private SpellSlot slot;

    public void Initialize(SpellSlot slot)
    {
        this.slot = slot;

        spellIcon.sprite = slot.spell.icon;
        manaCostText.text = Mathf.CeilToInt(slot.spell.manaCost).ToString();
    }

    private void Update()
    {
        if (slot == null || slot.spell == null)
            return;

        bool coolingDown = slot.cooldownRemaining > 0f;

        offCooldown.SetActive(!coolingDown);

        cooldownBar.fillAmount = slot.cooldownRemaining / slot.spell.cooldown;
    }
}