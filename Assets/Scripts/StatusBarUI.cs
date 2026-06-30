using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text valueText;

    public void UpdateBar(float current, float max)
    {
        fillImage.fillAmount = current / max;
        valueText.text = $"{Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";
    }
}