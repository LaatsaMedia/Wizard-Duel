using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Coroutine movementRoutine;

    public float MovementMultiplier { get; private set; } = 1f;

    public void ApplyMovementSlow(
        float multiplier,
        float duration)
    {
        if (movementRoutine != null)
        {
            StopCoroutine(movementRoutine);
        }

        movementRoutine = StartCoroutine(
            MovementRoutine(multiplier, duration));
    }

    private IEnumerator MovementRoutine(
        float multiplier,
        float duration)
    {
        MovementMultiplier = multiplier;

        yield return new WaitForSeconds(duration);

        MovementMultiplier = 1f;
        movementRoutine = null;
    }
}