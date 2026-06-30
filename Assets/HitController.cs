using System.Collections;
using UnityEngine;

public class HitController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer bodySprite;
    [SerializeField] private Sprite hitSprite;
    [SerializeField] private Transform bodyTransform;

    private UIShakeEffect healthBarShake;

    [Header("Sprite Flash")]
    [SerializeField] private float flashDuration = 0.06f;

    [Header("Body Shake")]
    [SerializeField] private float shakeDuration = 0.08f;
    [SerializeField] private float shakeStrength = 0.08f;
    [SerializeField] private float shakeFrequency = 0.015f;

    private Sprite originalSprite;
    private Vector3 originalLocalPosition;

    private Coroutine flashRoutine;
    private Coroutine shakeRoutine;

    private void Awake()
    {
        if (bodySprite != null)
            originalSprite = bodySprite.sprite;

        if (bodyTransform != null)
            originalLocalPosition = bodyTransform.localPosition;
    }

    public void SetHealthBarShake(UIShakeEffect shake)
    {
        healthBarShake = shake;
    }

    public void PlayHitFeedback()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
            bodyTransform.localPosition = originalLocalPosition;
        }

        flashRoutine = StartCoroutine(FlashRoutine());
        shakeRoutine = StartCoroutine(ShakeRoutine());

        healthBarShake?.Shake();
    }

    private IEnumerator FlashRoutine()
    {
        if (bodySprite == null || hitSprite == null)
            yield break;

        bodySprite.sprite = hitSprite;

        yield return new WaitForSeconds(flashDuration);

        bodySprite.sprite = originalSprite;

        flashRoutine = null;
    }

    private IEnumerator ShakeRoutine()
    {
        float timer = 0f;

        while (timer < shakeDuration)
        {
            float progress = timer / shakeDuration;
            float currentStrength = Mathf.Lerp(shakeStrength, 0f, progress);

            bodyTransform.localPosition =
                originalLocalPosition +
                (Vector3)(Random.insideUnitCircle * currentStrength);

            timer += shakeFrequency;

            yield return new WaitForSeconds(shakeFrequency);
        }

        bodyTransform.localPosition = originalLocalPosition;

        shakeRoutine = null;
    }
}