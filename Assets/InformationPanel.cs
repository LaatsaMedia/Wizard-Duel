using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanel : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private ScrollRect panelScroll;

    [Header("General")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("Content")]
    [SerializeField] private Transform tagParent;
    [SerializeField] private Transform modifierParent;
    [SerializeField] private Transform noteParent;

    [Header("Sections")]
    [SerializeField] private GameObject tagSection;
    [SerializeField] private GameObject modifierSection;
    [SerializeField] private GameObject noteSection;

    [Header("Prefabs")]
    [SerializeField] private InspectionTagRow tagPrefab;
    [SerializeField] private InspectionModifierRow modifierPrefab;
    [SerializeField] private InspectionNoteRow notePrefab;

    public void Display(InspectionData data)
    {
        Clear();

        content.SetActive(true);

        panelScroll.verticalNormalizedPosition = 1f;

        tagSection.SetActive(data.Tags.Count > 0);
        modifierSection.SetActive(data.Modifiers.Count > 0);
        noteSection.SetActive(data.Notes.Count > 0);

        icon.sprite = data.Icon;
        titleText.text = data.Title;
        subtitleText.text = data.Subtitle;
        descriptionText.text = data.Description;

        foreach (InspectionTag tag in data.Tags)
        {
            Instantiate(tagPrefab, tagParent).Setup(tag);
        }

        foreach (InspectionModifier modifier in data.Modifiers)
        {
            Instantiate(modifierPrefab, modifierParent).Setup(modifier);
        }

        foreach (InspectionNote note in data.Notes)
        {
            Instantiate(notePrefab, noteParent).Setup(note);
        }
    }

    public void Clear()
    {
        icon.sprite = null;
        titleText.text = "";
        subtitleText.text = "";
        descriptionText.text = "";

        ClearChildren(tagParent);
        ClearChildren(modifierParent);
        ClearChildren(noteParent);

        tagSection.SetActive(false);
        modifierSection.SetActive(false);
        noteSection.SetActive(false);

        content.SetActive(false);
    }

    private void ClearChildren(Transform parent)
    {
        foreach (Transform child in parent)
            Destroy(child.gameObject);
    }
}