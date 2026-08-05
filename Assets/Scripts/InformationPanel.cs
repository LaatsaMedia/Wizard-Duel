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
    [SerializeField] private Transform statParent;
    [SerializeField] private Transform modifierParent;
    [SerializeField] private Transform noteParent;

    [Header("Sections")]
    [SerializeField] private GameObject tagSection;
    [SerializeField] private GameObject statSection;
    [SerializeField] private GameObject modifierSection;
    [SerializeField] private GameObject noteSection;

    [Header("Prefabs")]
    [SerializeField] private InspectionTagRow tagPrefab;
    [SerializeField] private InspectionStatRow statPrefab;
    [SerializeField] private InspectionModifierRow modifierPrefab;
    [SerializeField] private InspectionNoteRow notePrefab;

    private void Awake()
    {
        Clear();
    }

    public void Display(InspectionData data)
    {
        Clear();

        content.SetActive(true);

        panelScroll.verticalNormalizedPosition = 1f;

        // General
        icon.gameObject.SetActive(data.Icon != null);
        titleText.gameObject.SetActive(!string.IsNullOrWhiteSpace(data.Title));
        subtitleText.gameObject.SetActive(!string.IsNullOrWhiteSpace(data.Subtitle));
        descriptionText.gameObject.SetActive(!string.IsNullOrWhiteSpace(data.Description));

        icon.sprite = data.Icon;
        titleText.text = data.Title;
        subtitleText.text = data.Subtitle;
        descriptionText.text = data.Description;

        // Sections
        tagSection.SetActive(data.Tags.Count > 0);
        statSection.SetActive(data.Stats.Count > 0);
        modifierSection.SetActive(data.Modifiers.Count > 0);
        noteSection.SetActive(data.Notes.Count > 0);

        foreach (InspectionTag tag in data.Tags)
        {
            Instantiate(tagPrefab, tagParent).Setup(tag);
        }

        foreach (InspectionStat stat in data.Stats)
        {
            Instantiate(statPrefab, statParent).Setup(stat);
        }

        foreach (InspectionModifier modifier in data.Modifiers)
        {
            Instantiate(modifierPrefab, modifierParent)
                .Setup(modifier);
        }

        foreach (InspectionNote note in data.Notes)
        {
            Instantiate(notePrefab, noteParent).Setup(note);
        }
    }

    public void Clear()
    {
        // Hide everything
        content.SetActive(false);

        icon.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
        subtitleText.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);

        tagSection.SetActive(false);
        statSection.SetActive(false);
        modifierSection.SetActive(false);
        noteSection.SetActive(false);

        // Clear values
        icon.sprite = null;
        titleText.text = "";
        subtitleText.text = "";
        descriptionText.text = "";

        ClearChildren(tagParent);
        ClearChildren(statParent);
        ClearChildren(modifierParent);
        ClearChildren(noteParent);
    }

    private void ClearChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}