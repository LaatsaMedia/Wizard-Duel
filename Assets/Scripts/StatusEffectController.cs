using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform effectContainer;
    [SerializeField] private StatusEffectUI effectPrefab;

    public Transform visualEffectContainer;

    private readonly Dictionary<StatusEffectType, Coroutine> activeEffects = new();
    private readonly Dictionary<StatusEffectType, StatusEffectUI> activeUI = new();
    private readonly Dictionary<StatusEffectType, GameObject> activeVisuals = new();

    public bool IsRooted => HasEffect(StatusEffectType.Root);
    public bool IsBurning => HasEffect(StatusEffectType.Burning);
    public bool IsPoisoned => HasEffect(StatusEffectType.Poison);
    public bool IsFrozen => HasEffect(StatusEffectType.Frozen);
    public bool IsShielded => HasEffect(StatusEffectType.Shield);
    public bool IsStunned => HasEffect(StatusEffectType.Stun);
    public bool IsGrounded => HasEffect(StatusEffectType.Grounded);

    public void ApplyEffect(StatusEffect effect, float duration)
    {
        // Refresh existing effect.
        if (activeEffects.TryGetValue(effect.type, out Coroutine existingRoutine))
        {
            StopCoroutine(existingRoutine);

            if (activeUI.TryGetValue(effect.type, out StatusEffectUI existingUI))
            {
                Destroy(existingUI.gameObject);
                activeUI.Remove(effect.type);
            }
            
            if (activeVisuals.TryGetValue(effect.type, out GameObject visual))
            {
                Destroy(visual);
                activeVisuals.Remove(effect.type);
            }

            activeEffects.Remove(effect.type);
        }

        StatusEffectUI ui = Instantiate(effectPrefab, effectContainer);
        ui.Initialize(effect);

        activeUI.Add(effect.type, ui);

        if (effect.WorldEffectPrefab != null)
        {
            GameObject visual = Instantiate(
                effect.WorldEffectPrefab,
                visualEffectContainer);

            activeVisuals.Add(effect.type, visual);
        }

        Coroutine routine = StartCoroutine(
            EffectRoutine(effect.type, duration));

        activeEffects.Add(effect.type, routine);
    }

    public bool HasEffect(StatusEffectType type)
    {
        return activeEffects.ContainsKey(type);
    }

    public void RemoveEffect(StatusEffectType type)
    {
        if (activeEffects.TryGetValue(type, out Coroutine routine))
        {
            StopCoroutine(routine);
            activeEffects.Remove(type);
        }

        if (activeUI.TryGetValue(type, out StatusEffectUI ui))
        {
            Destroy(ui.gameObject);
            activeUI.Remove(type);
        }
        if (activeVisuals.TryGetValue(type, out GameObject visual))
        {
            Destroy(visual);
            activeVisuals.Remove(type);
        }
    }

    private IEnumerator EffectRoutine(
        StatusEffectType type,
        float duration)
    {
        yield return new WaitForSeconds(duration);

        RemoveEffect(type);
    }

    public bool ConsumeFrozen()
    {
        if (!HasEffect(StatusEffectType.Frozen))
            return false;

        RemoveEffect(StatusEffectType.Frozen);
        return true;
    }
}