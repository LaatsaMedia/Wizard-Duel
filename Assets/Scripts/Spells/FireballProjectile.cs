using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FireballProjectile : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Explode();
    }

    private void Explode()
    {
        Vector2 center = explosionCenter.position;

        if (explosionPrefab != null)
        {
            Instantiate(
                explosionPrefab,
                center,
                Quaternion.identity);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            center,
            explosionRadius,
            hitLayers);

        foreach (Collider2D hit in hits)
        {
            Vector2 closestPoint = hit.ClosestPoint(center);

            float distance =
                Vector2.Distance(center, closestPoint);

            float t = Mathf.Clamp01(distance / explosionRadius);

            float damage =
                Mathf.Lerp(maxDamage, 0f, t);

            float knockback =
                Mathf.Lerp(maxKnockback, 0f, t);

            if (hit.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
            }

            if (hit.TryGetComponent(out KnockbackReceiver knockbackReceiver))
            {
                Vector2 direction = closestPoint - center;

                // Direct hit.
                if (direction.sqrMagnitude < 0.0001f)
                {
                    direction = transform.right;
                }

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