using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FireWall : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Damage")]
    [SerializeField] private float damagePerSecond = 20f;

    private readonly List<Health> targets = new();

    private Vector2 moveDirection;
    private bool stopped;

    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private void Update()
    {
        if (!stopped)
        {
            transform.position +=
                (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
        }

        float damage = damagePerSecond * Time.deltaTime;

        // Make a copy so the original list can safely change
        Health[] currentTargets = targets.ToArray();

        foreach (Health health in currentTargets)
        {
            if (health == null)
                continue;

            health.TakeDamage(damage, false);
        }

        // Clean up any destroyed references
        targets.RemoveAll(h => h == null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out FireWall fireWall))
        {
            stopped = true;
            fireWall.StopMoving();
            return;
        }

        if (other.TryGetComponent(out Health health))
        {
            if (!targets.Contains(health))
            {
                targets.Add(health);
            }
        }
    }

    public void StopMoving()
    {
        stopped = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Health health))
        {
            targets.Remove(health);
        }
    }

    private void OnDisable()
    {
        targets.Clear();
    }
}