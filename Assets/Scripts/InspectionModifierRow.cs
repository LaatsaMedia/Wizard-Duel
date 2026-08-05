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

        string valueText = "";

        switch (modifier.DisplayType)
        {
            case ModifierDisplayType.Value:
            {
                valueText = modifier.Value.ToString("0.##");
                break;
            }

            case ModifierDisplayType.Flat:
            {
                if (modifier.IsBaseStat)
                {
                    valueText = modifier.Value.ToString("0.##");
                }
                else
                {
                    string sign = modifier.Value > 0f ? "+" :
                                  modifier.Value < 0f ? "-" : "";

                    valueText = $"{sign}{Mathf.Abs(modifier.Value):0.##}";
                }

                break;
            }

            case ModifierDisplayType.Percentage:
            {
                string sign = modifier.Value > 0f ? "+" :
                              modifier.Value < 0f ? "-" : "";

                valueText = $"{sign}{Mathf.Abs(modifier.Value):0.##}%";
                break;
            }

            case ModifierDisplayType.Multiplier:
            {
                float percent = (modifier.Value - 1f) * 100f;

                string sign = percent > 0f ? "+" :
                              percent < 0f ? "-" : "";

                valueText = $"{sign}{Mathf.Abs(percent):0.##}%";
                break;
            }
        }

        if (modifier.Duration > 0f)
        {
            valueText += $"/{modifier.Duration:0.##}s";
        }

        value.text = valueText;
    }
}