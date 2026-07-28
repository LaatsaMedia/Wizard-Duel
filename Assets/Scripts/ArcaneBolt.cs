using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArcaneBolt : SpellBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed = 25f;
    [SerializeField] private float damage = 3f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float onHitValue = 0.1f;

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
            if (RegisterHit(health))
            {
                SpellEffects.DealDamage(
                    caster,
                    Spell,
                    health,
                    damage,
                    onHitValue); // 10% effectiveness
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