using System.Collections.Generic;
using UnityEngine;

public class StatsPanel : MonoBehaviour
{
    [SerializeField] private Transform statParent;
    [SerializeField] private InspectionModifierRow modifierPrefab;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        Clear();

        WizardBuild build = RunManager.Instance.PlayerBuild;

        List<InspectionModifier> modifiers =
            WizardStatsBuilder.Build(build);

        foreach (InspectionModifier modifier in modifiers)
        {
            Instantiate(modifierPrefab, statParent)
                .Setup(modifier);
        }
    }

    private void Clear()
    {
        foreach (Transform child in statParent)
        {
            Destroy(child.gameObject);
        }
    }
}