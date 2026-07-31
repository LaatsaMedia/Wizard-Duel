using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectionNoteRow : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text valueText;

    public void Setup(InspectionNote note)
    {
        icon.sprite = note.Icon;
        valueText.text = note.Text;
    }

    public void Setup(InspectionModifier modifier)
    {
        icon.sprite = modifier.Icon;

        if (modifier.Duration > 0)
        {
            valueText.text = modifier.IsPercentage
                ? $"{modifier.Value}% / {modifier.Duration}"
                : $"{modifier.Value} / {modifier.Duration}";
        }
        else
        {
            valueText.text = modifier.IsPercentage
                ? $"{modifier.Value}%"
                : modifier.Value.ToString();
        }
    }
}