using UnityEngine;

public class FallingStar : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float fallSpeed = 8f;
    [SerializeField] private float maxLifetime = 5f;
    [SerializeField] private float rotationSpeed = 180f;

    [Header("Explosion")]
    [SerializeField] private float damage = 12f;
    [SerializeField] private float explosionRadius = 1.5f;
    [SerializeField] private LayerMask hitLayers;

    [Header("VFX")]
    [SerializeField] private GameObject impactPrefab;

    private bool exploded;

    private void Start()
    {
        Destroy(gameObject, maxLifetime);
    }

    private void Update()
    {
        transform.position +=
            Vector3.down * fallSpeed * Time.deltaTime;

        transform.Rotate(
            0f,
            0f,
            rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (exploded)
            return;

        Explode();
    }

    private void Explode()
    {
        exploded = true;

        if (impactPrefab != null)
        {
            Instantiate(
                impactPrefab,
                transform.position,
                Quaternion.identity);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            explosionRadius,
            hitLayers);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius);
    }
}