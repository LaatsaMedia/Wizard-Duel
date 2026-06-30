using System;
using System.Collections;
using UnityEngine;

public class ScoreOrb : MonoBehaviour
{
    [SerializeField] private float travelTime = 1f;
    [SerializeField] private float curveAmount = 2f;
    [SerializeField] private float reachDistance = 0.3f;

    public void FlyTo(
        Vector3 start,
        Transform target,
        Action onReached)
    {
        transform.position = start;

        StartCoroutine(FlyRoutine(target, onReached));
    }

    private IEnumerator FlyRoutine(
        Transform target,
        Action onReached)
    {
        Vector3 start = transform.position;

        // Random Bezier control point.
        Vector3 middle = (start + target.position) * 0.5f;
        middle += (Vector3)UnityEngine.Random.insideUnitCircle * curveAmount;

        float elapsed = 0f;

        while (true)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / travelTime);

            // Ease in.
            float eased = t * t;

            // Continuously update the target position.
            Vector3 end = target.position;

            Vector3 a = Vector3.Lerp(start, middle, eased);
            Vector3 b = Vector3.Lerp(middle, end, eased);

            transform.position = Vector3.Lerp(a, b, eased);

            // Close enough to the winner.
            if (Vector3.Distance(transform.position, end) <= reachDistance)
                break;

            yield return null;
        }

        onReached?.Invoke();

        Destroy(gameObject);
    }
}