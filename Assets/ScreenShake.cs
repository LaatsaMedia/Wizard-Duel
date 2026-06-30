using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }

    private Vector3 originalLocalPosition;

    private float shakeTime;
    private float shakeStrength;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        originalLocalPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (shakeTime > 0f)
        {
            shakeTime -= Time.deltaTime;

            Vector2 offset = Random.insideUnitCircle * shakeStrength;

            transform.localPosition = originalLocalPosition + new Vector3(offset.x, offset.y, 0f);
        }
        else
        {
            transform.localPosition = originalLocalPosition;
        }
    }

    public void Shake(float strength, float duration)
    {
        if (strength > shakeStrength)
            shakeStrength = strength;

        if (duration > shakeTime)
            shakeTime = duration;
    }
}