using System.Collections.Generic;
using UnityEngine;

public class FlameBurst : SpellBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damage = 35f;

    [Header("Knockback")]
    [SerializeField] private float enemyKnockbackForce = 12f;
    [SerializeField] private float enemyKnockbackDuration = 0.2f;
    [SerializeField] private float upwardKnockback = 0.45f;

    [Header("Self Recoil")]
    [SerializeField] private float selfKnockbackForce = 8f;
    [SerializeField] private float selfKnockbackDuration = 0.15f;
    [SerializeField] private float selfUpwardKnockback = 0.15f;

    [Header("Lifetime")]
    [SerializeField] private float lifetime = 0.2f;

    private Health casterHealth;
    private Vector2 fireDirection;

    private readonly HashSet<Health> hitTargets = new();

    public override void Initialize(GameObject caster, Spell spell, float castDirection)
    {
        base.Initialize(caster, spell, castDirection);

        casterHealth = caster.GetComponent<Health>();

        // Determine fire direction
        if (casterHealth.Team == Team.Player)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            fireDirection = ((Vector2)mouseWorld - (Vector2)caster.transform.position).normalized;
        }
        else
        {
            // Enemy aims at the player
            fireDirection = (
                MatchManager.Instance.CurrentPlayer.transform.position -
                caster.transform.position).normalized;
        }

        // Position and rotate flame
        transform.position = caster.transform.position;

        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Apply recoil
        if (caster.TryGetComponent(out KnockbackReceiver receiver))
        {
            Vector2 recoilDirection = -fireDirection;

            // Small upward lift
            recoilDirection.y += selfUpwardKnockback;
            recoilDirection.Normalize();

            receiver.ApplyKnockback(
                recoilDirection * selfKnockbackForce,
                selfKnockbackDuration);
        }

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Health health))
            return;

        if (health == casterHealth)
            return;

        if (health.Team == casterHealth.Team)
            return;

        if (hitTargets.Contains(health))
            return;

        hitTargets.Add(health);

        SpellEffects.DealDamage(
            caster,
            Spell,
            health,
            damage);

        if (other.TryGetComponent(out KnockbackReceiver receiver))
        {
            Vector2 knockbackDirection = fireDirection;

            // Add some lift
            knockbackDirection.y += upwardKnockback;
            knockbackDirection.Normalize();

            receiver.ApplyKnockback(
                knockbackDirection * enemyKnockbackForce,
                enemyKnockbackDuration);
        }
    }

    private void OnDisable()
    {
        hitTargets.Clear();
    }
}