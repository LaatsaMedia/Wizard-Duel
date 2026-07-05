using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BubblePrisonProjectile : SpellBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float damage = 0f;
    [SerializeField] private float lifetime = 5f;

    [Header("Status Effect")]
    [SerializeField] private StatusEffect stunEffect;
    [SerializeField] private float stunDuration = 1.5f;

    [Header("VFX")]
    [SerializeField] private GameObject impactPrefab;
    [SerializeField] private GameObject bubbleVisualPrefab;

    [SerializeField] private float rotationSpeed = 100f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.linearVelocity = transform.right * speed;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == caster)
            return;

        if (other.TryGetComponent(out Health health))
        {
            // Don't affect teammates.
            if (caster.TryGetComponent(out Health casterHealth) &&
                health.Team == casterHealth.Team)
            {
                return;
            }

            if (damage > 0f)
            {
                health.TakeDamage(damage);
            }
        }

        if (other.TryGetComponent(out StatusEffectController statusEffects))
        {
            statusEffects.ApplyEffect(
                stunEffect,
                stunDuration);

            if (bubbleVisualPrefab != null)
            {
                GameObject bubble = Instantiate(
                    bubbleVisualPrefab,
                    other.transform.position,
                    Quaternion.identity);

                bubble.GetComponent<BubbleVisual>().Initialize(statusEffects);
            }
        }

        OnDestroyed();
    }

    private void OnDestroyed()
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