using System.Collections.Generic;
using UnityEngine;

public class WindSlashProjectile : SpellBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 25f;
    [SerializeField] private float lifetime = 0.4f;

    [Header("Damage")]
    [SerializeField] private float damage = 18f;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 2f;
    [SerializeField] private float knockbackDuration = 0.1f;

    [Header("Collision")]
    [SerializeField] private LayerMask wallLayer;

    [Header("VFX")]
    [SerializeField] private GameObject impactPrefab;

    private readonly HashSet<Health> hitTargets = new();

    private Vector2 moveDirection;

    private void Start()
    {
        moveDirection = transform.right.normalized;

        float angle = Mathf.Atan2(
            moveDirection.y,
            moveDirection.x) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0f, 0f, angle);

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (caster == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Hit a wall.
        if (((1 << other.gameObject.layer) & wallLayer) != 0)
        {
            DestroyProjectile();
            return;
        }

        // Ignore our caster.
        if (other.gameObject == caster)
            return;

        if (other.TryGetComponent(out Health health))
        {
            // Already pierced this target.
            if (!hitTargets.Add(health))
                return;

            SpellEffects.DealDamage(
                caster,
                Spell,
                health,
                damage);

            if (other.TryGetComponent(out KnockbackReceiver knockback))
            {
                Vector2 force = moveDirection * knockbackForce;

                knockback.ApplyKnockback(
                    force,
                    knockbackDuration);
            }

            // Keep flying!
        }
    }

    private void DestroyProjectile()
    {
        if (impactPrefab != null)
        {
            Instantiate(
                impactPrefab,
                transform.position,
                Quaternion.identity);
        }

        Destroy(gameObject);
    }
}