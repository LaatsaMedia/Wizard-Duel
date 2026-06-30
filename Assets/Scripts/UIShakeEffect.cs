using System.Collections;
using UnityEngine;

public class UIShakeEffect : MonoBehaviour
{
    [Header("Default Shake")]
    [SerializeField] private float defaultDuration = 0.15f;
    [SerializeField] private float defaultStrength = 8f;
    [SerializeField] private float frequency = 0.015f;

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Coroutine shakeRoutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void Shake()
    {
        Shake(defaultDuration, defaultStrength);
    }

    public void Shake(float duration, float strength)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(
            ShakeRoutine(duration, strength));
    }

    private IEnumerator ShakeRoutine(float duration, float strength)
    {
        rectTransform.anchoredPosition = originalPosition;

        float timer = 0f;

        while (timer < duration)
        {
            float progress = timer / duration;
            float currentStrength = Mathf.Lerp(strength, 0f, progress);

            rectTransform.anchoredPosition =
                originalPosition +
                Random.insideUnitCircle * currentStrength;

            timer += frequency;

            yield return new WaitForSeconds(frequency);
        }

        rectTransform.anchoredPosition = originalPosition;
        shakeRoutine = null;
    }
}