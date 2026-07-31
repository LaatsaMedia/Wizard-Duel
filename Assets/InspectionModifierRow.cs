using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectionModifierRow : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text value;

    public void Setup(InspectionModifier modifier)
    {
        icon.sprite = modifier.Icon;

        string valueText;

        if (modifier.IsPercentage)
        {
            float percent = (modifier.Value - 1f) * 100f;

            string sign = percent > 0f ? "+" :
                          percent < 0f ? "-" : "";

            valueText = $"{sign}{Mathf.Abs(percent):F0}%";
        }
        else
        {
            string sign = modifier.Value > 0f ? "+" :
                          modifier.Value < 0f ? "-" : "";

            valueText = $"{sign}{Mathf.Abs(modifier.Value):F0}";
        }

        if (modifier.Duration > 0f)
        {
            valueText += $"/{modifier.Duration:F0}s";
        }

        value.text = valueText;
    }
}