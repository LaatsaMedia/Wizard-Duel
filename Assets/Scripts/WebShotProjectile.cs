using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WebShotProjectile : SpellBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 16f;
    [SerializeField] private float lifetime = 4f;

    [Header("Damage")]
    [SerializeField] private float damage = 6f;

    [Header("Status Effect")]
    [SerializeField] private StatusEffect groundedEffect;
    [SerializeField] private float groundedDuration = 2f;
    
    [SerializeField, Range(0f, 1f)]
    private float movementSpeedMultiplier = 0.5f;

    [Header("VFX")]
    [SerializeField] private GameObject impactPrefab;
    
    [Header("Visual")]
    [SerializeField] private GameObject webVisualPrefab;
    [SerializeField] private Vector3 webVisualOffset;

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
        {
            travelSFX.Play();
        }

        rb.linearVelocity = transform.right * speed;

        Destroy(gameObject, lifetime);
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

            if (RegisterHit(health))
            {
                SpellEffects.DealDamage(
                    caster,
                    Spell,
                    health,
                    damage);
            }
        }

        if (other.TryGetComponent(out StatusEffectController statusEffects))
        {
            statusEffects.ApplyEffect(
                groundedEffect,
                groundedDuration);

            if (other.TryGetComponent(out MovementController movement))
            {
                movement.ApplyMovementSlow(
                    movementSpeedMultiplier,
                    groundedDuration);
            }

            if (webVisualPrefab != null &&
                statusEffects.visualEffectContainer != null)
            {
                GameObject web = Instantiate(
                    webVisualPrefab,
                    statusEffects.visualEffectContainer.position + webVisualOffset,
                    Quaternion.identity,
                    statusEffects.visualEffectContainer);

                Destroy(web, groundedDuration);
            }
        }

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