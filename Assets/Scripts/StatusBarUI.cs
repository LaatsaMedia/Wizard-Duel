using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text valueText;

    // Used by Mana and Ultimate bars
    public void UpdateBar(float current, float max)
    {
        fillImage.fillAmount = current / max;
        valueText.text = $"{Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";
    }

    // Used by Health bar when a Barrier may exist
    public void UpdateBar(float current, float max, float shield)
    {
        fillImage.fillAmount = current / max;

        valueText.text =
            $"{Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";

        if (shield > 0f)
        {
            valueText.text += $" (+{Mathf.CeilToInt(shield)})";
        }
    }
}