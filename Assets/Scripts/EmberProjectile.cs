using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EmberProjectile : SpellBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 25f;
    [SerializeField] private float lifetime = 2f;

    [Header("Damage")]
    [SerializeField] private float damage = 8f;

    [Header("VFX")]
    [SerializeField] private GameObject impactPrefab;

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
        if (other.TryGetComponent(out Health health))
        {
            SpellEffects.DealDamage(
                caster,
                Spell,
                health,
                damage);
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