using UnityEngine;

[CreateAssetMenu(menuName = "Wizard Duel/Pack")]
public class PackDefinition : ScriptableObject
{
    [Header("General")]

    [SerializeField] private string packName;
    public string PackName => packName;

    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    [TextArea]
    [SerializeField] private string description;
    public string Description => description;
}