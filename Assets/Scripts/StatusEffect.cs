using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Status Effect")]
public class StatusEffect : ScriptableObject
{
    public StatusEffectType type;

    public string displayName;

    public Sprite icon;

    [Header("Visuals")]
    [SerializeField] private GameObject worldEffectPrefab;
    public GameObject WorldEffectPrefab => worldEffectPrefab;
}