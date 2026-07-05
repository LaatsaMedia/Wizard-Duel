using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArcaneBolt : SpellBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed = 25f;
    [SerializeField] private float damage = 3f;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Initialize(GameObject caster, float direction)
    {
        base.Initialize(caster, direction);

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

        OnDestroyed();
    }

    private void OnDestroyed()
    {
        // VFX

        Destroy(gameObject);
    }
}