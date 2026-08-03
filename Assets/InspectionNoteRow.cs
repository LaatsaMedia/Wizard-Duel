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

        icon.gameObject.SetActive(note.Icon != null);

        valueText.text = note.Text;
    }
}