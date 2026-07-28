using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FireballProjectile : SpellBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 5f;

    [Header("Explosion")]
    [SerializeField] private Transform explosionCenter;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float maxDamage = 20f;
    [SerializeField] private float maxKnockback = 10f;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private float knockbackDuration = 0.15f;

    [Header("VFX")]
    [SerializeField] private GameObject explosionPrefab;

    [Header("SFX")]
    [SerializeField] private AudioSource travelSFX;

    [Header("Rotation")]
    [SerializeField] private float projectileRotationOffset = 0f;
    [SerializeField] private float explosionRotationOffset = 0f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (travelSFX != null)
            travelSFX.Play();

        rb.linearVelocity = transform.right * speed;

        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(
                rb.linearVelocity.y,
                rb.linearVelocity.x) * Mathf.Rad2Deg;

            rb.rotation = angle + projectileRotationOffset;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 impactPoint = other.ClosestPoint(transform.position);
        Vector2 impactDirection = (impactPoint - (Vector2)transform.position).normalized;

        // Fallback if ClosestPoint == projectile position.
        if (impactDirection.sqrMagnitude < 0.0001f)
            impactDirection = rb.linearVelocity.normalized;

        Explode(impactDirection);
    }

    private void Explode(Vector2 impactDirection)
    {
        Vector2 center = explosionCenter.position;

        if (explosionPrefab != null)
        {
            float angle = Mathf.Atan2(
                impactDirection.y,
                impactDirection.x) * Mathf.Rad2Deg;

            Instantiate(
                explosionPrefab,
                center,
                Quaternion.Euler(0f, 0f, angle + explosionRotationOffset));
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            center,
            explosionRadius,
            hitLayers);

        foreach (Collider2D hit in hits)
        {
            Vector2 closestPoint = hit.ClosestPoint(center);

            float distance = Vector2.Distance(center, closestPoint);

            float t = Mathf.Clamp01(distance / explosionRadius);

            float damage = Mathf.Lerp(maxDamage, 0f, t);
            float knockback = Mathf.Lerp(maxKnockback, 0f, t);

        if (hit.TryGetComponent(out Health health))
        {
            if (RegisterHit(health))
            {
                SpellEffects.DealDamage(
                    caster,
                    Spell,
                    health,
                    damage);
            }
        }

            if (hit.TryGetComponent(out KnockbackReceiver knockbackReceiver))
            {
                Vector2 direction = closestPoint - center;

                if (direction.sqrMagnitude < 0.0001f)
                    direction = impactDirection;

                direction.Normalize();

                knockbackReceiver.ApplyKnockback(
                    direction * knockback,
                    knockbackDuration);
            }
        }

        ScreenShake.Instance.Shake(0.15f, 0.15f);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (explosionCenter == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            explosionCenter.position,
            explosionRadius);
    }
}