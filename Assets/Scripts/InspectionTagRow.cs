using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectionTagRow : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text text;

    public void Setup(InspectionTag tag)
    {
        icon.sprite = tag.Icon;
        text.text = tag.Text;
    }
}