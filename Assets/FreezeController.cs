using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StatusEffectController))]
public class FreezeController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StatusEffect frozenEffect;

    private FreezeStackVisual stackVisual;
    private StatusEffectController statusEffects;

    [Header("Settings")]
    [SerializeField] private float immuneTime = 6f;

    private bool immuneToFreeze;
    private int currentHits;

    private Coroutine resetRoutine;
    private Coroutine immunityRoutine;

    private void Awake()
    {
        statusEffects = GetComponent<StatusEffectController>();
        stackVisual = GetComponentInChildren<FreezeStackVisual>();
    }

    public void AddHit(
        int requiredHits,
        float hitWindow,
        float freezeDuration)
    {
        if (immuneToFreeze)
            return;

        currentHits++;

        // Show at most 3 orbiting shards.
        if (stackVisual != null)
        {
            stackVisual.SetStacks(
                Mathf.Min(currentHits, requiredHits - 1));
        }

        if (resetRoutine != null)
            StopCoroutine(resetRoutine);

        resetRoutine = StartCoroutine(
            ResetHits(hitWindow));

        if (currentHits < requiredHits)
            return;

        currentHits = 0;

        if (stackVisual != null)
        {
            stackVisual.SetStacks(0);
        }

        if (resetRoutine != null)
        {
            StopCoroutine(resetRoutine);
            resetRoutine = null;
        }

        statusEffects.ApplyEffect(
            frozenEffect,
            freezeDuration);

        immuneToFreeze = true;

        if (immunityRoutine != null)
            StopCoroutine(immunityRoutine);

        immunityRoutine = StartCoroutine(
            FreezeImmunityRoutine(freezeDuration));
    }

    private IEnumerator ResetHits(float hitWindow)
    {
        yield return new WaitForSeconds(hitWindow);

        currentHits = 0;

        if (stackVisual != null)
        {
            stackVisual.SetStacks(0);
        }

        resetRoutine = null;
    }

    private IEnumerator FreezeImmunityRoutine(
        float freezeDuration)
    {
        yield return new WaitForSeconds(
            freezeDuration + immuneTime);

        immuneToFreeze = false;
        immunityRoutine = null;
    }
}