using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text effectName;

    public void Initialize(StatusEffect effect)
    {
        icon.sprite = effect.icon;
        effectName.text = effect.displayName;
    }
}