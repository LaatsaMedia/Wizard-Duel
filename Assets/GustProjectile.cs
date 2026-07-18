using UnityEngine;

public class GustProjectile : SpellBehaviour
{
    public override bool IsGroundSpell => true;

    [Header("Movement")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 2f;

    [Header("Damage")]
    [SerializeField] private float damage = 5f;

    [Header("Knockback")]
    [SerializeField] private float horizontalKnockback = 2f;
    [SerializeField] private float verticalKnockback = 8f;
    [SerializeField] private float knockbackDuration = 0.2f;

    [Header("Collision")]
    [SerializeField] private LayerMask wallLayer;

    [Header("VFX")]
    [SerializeField] private GameObject impactPrefab;

    private float direction;
    private bool destroyed;

    private void Start()
    {
        direction = Mathf.Sign(transform.right.x);

        // Keep sprite upright.
        transform.rotation = Quaternion.identity;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (destroyed)
            return;

        if (caster == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position +=
            Vector3.right * direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (destroyed)
            return;

        // Hit a wall.
        if (((1 << other.gameObject.layer) & wallLayer) != 0)
        {
            DestroyProjectile();
            return;
        }

        // Hit a player/enemy.
        if (other.TryGetComponent(out Health health))
        {
            SpellEffects.DealDamage(
                caster,
                Spell,
                health,
                damage);

            if (other.TryGetComponent(
                out KnockbackReceiver knockback))
            {
                Vector2 force = new Vector2(
                    direction * horizontalKnockback,
                    verticalKnockback);

                knockback.ApplyKnockback(
                    force,
                    knockbackDuration);
            }

            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        if (destroyed)
            return;

        destroyed = true;

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