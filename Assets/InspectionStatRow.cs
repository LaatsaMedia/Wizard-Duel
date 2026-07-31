using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectionStatRow : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text valueText;

    public void Setup(InspectionStat stat)
    {
        icon.sprite = stat.Icon;
        nameText.text = stat.Name;
        valueText.text = stat.Value;
    }
}