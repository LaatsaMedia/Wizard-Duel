using UnityEngine;

public class WaterBurst : SpellBehaviour
{
    [Header("Movement")]
    [SerializeField] private float horizontalForce = 8f;
    [SerializeField] private float verticalForce = 10f;
    [SerializeField] private float moveLock = 0.35f;

    [Header("Combat")]
    [SerializeField] private float damage = 8f;
    [SerializeField] private float radius = 1.5f;
    [SerializeField] private LayerMask hitLayers;

    [Header("Effects")]
    [SerializeField] private GameObject burstEffect;

    public override void Initialize(
        GameObject caster,
        Spell spell,
        float castDirection)
    {
        base.Initialize(caster, spell, castDirection);

        Burst();

        Destroy(gameObject);
    }

    private void Burst()
    {
        if (!caster.TryGetComponent(out Rigidbody2D rb))
            return;

        float direction = castDirection;

        if (caster.TryGetComponent(out PlayerController player))
        {
            if (Mathf.Abs(player.HorizontalInput) > 0.01f)
            {
                direction =
                    Mathf.Sign(player.HorizontalInput);
            }
        }

        if (caster.TryGetComponent(out KnockbackReceiver knockback))
        {
            Vector2 force = new Vector2(
                direction * horizontalForce,
                verticalForce);

            knockback.ApplyKnockback(
                force,
                moveLock);
        }

        if (burstEffect != null)
        {
            Instantiate(
                burstEffect,
                caster.transform.position,
                Quaternion.identity);
        }

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                caster.transform.position,
                radius,
                hitLayers);

        foreach (Collider2D hit in hits)
        {
            if (!hit.TryGetComponent(out Health health))
                continue;

            if (health.gameObject == caster)
                continue;

            SpellEffects.DealDamage(
                caster,
                Spell,
                health,
                damage);

            if (hit.attachedRigidbody != null)
            {
                Vector2 knockDirection =
                    (hit.transform.position -
                    caster.transform.position).normalized;

                hit.attachedRigidbody.AddForce(
                    knockDirection * 5f +
                    Vector2.up * 3f,
                    ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(
            transform.position,
            radius);
    }
}