using UnityEngine;

[CreateAssetMenu(menuName = "Inspection/Modifier Definition")]
public class ModifierDefinition : ScriptableObject
{
    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField] private bool isPercentage;
    public bool IsPercentage => isPercentage;
}