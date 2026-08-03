using UnityEngine;

[CreateAssetMenu(menuName = "Inspection/Status Effect Definition")]
public class StatusEffectDefinition : ScriptableObject
{
    [SerializeField] private StatusEffectType effect;
    public StatusEffectType Effect => effect;

    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;
}