using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class IceShardProjectile : SpellBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 5f;

    //[Header("Status Effect")]
    //[SerializeField] private StatusEffect slowEffect;
    //[SerializeField] private float slowDuration = 1f;

    [Header("VFX")]
    [SerializeField] private GameObject impactPrefab;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == caster)
            return;

        if (other.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
        }

        ApplySlowness(other);

        OnDestroyed();
    }

    private void ApplySlowness(Collider2D target)
    {
        //if (target.TryGetComponent(out StatusEffectController statusEffects))
        //{
        //    statusEffects.ApplyEffect(slowEffect, slowDuration);
        //}
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