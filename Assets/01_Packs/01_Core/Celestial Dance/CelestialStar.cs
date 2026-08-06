using System.Collections;
using UnityEngine;

public class CelestialStar : SpellBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private GameObject impactPrefab;

    [Header("Explosion")]
    [SerializeField] private Transform explosionCenter;
    [SerializeField] private float explosionRadius = 1.5f;
    [SerializeField] private LayerMask hitLayers;

    [Header("VFX")]
    [SerializeField] private GameObject explosionPrefab;

    private SpriteRenderer spriteRenderer;
    public SpriteRenderer Renderer => spriteRenderer;

    private Collider2D starCollider;
    private float colliderDelay = 0.1f;

    private Vector2 direction;
    private bool orbiting = true;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        starCollider = GetComponent<Collider2D>();
        starCollider.enabled = false;

        spriteRenderer =
            GetComponentInChildren<SpriteRenderer>();
    }

    public override void Initialize(
    GameObject caster,
    Spell spell,
    float castDirection)
    {
        base.Initialize(
            caster,
            spell,
            castDirection);

        Collider2D casterCollider =
            caster.GetComponent<Collider2D>();

        if (casterCollider != null &&
            starCollider != null)
        {
            Physics2D.IgnoreCollision(
                starCollider,
                casterCollider);
        }
    }

    public void SetOrbitPosition(Vector3 position)
    {
        if (!orbiting)
            return;

        transform.localPosition = position;
    }

    public void Launch(Vector3 targetPosition)
    {
        orbiting = false;

        transform.SetParent(null);

        direction =
            (targetPosition - transform.position).normalized;

        rb.linearVelocity = direction * speed;

        StartCoroutine(EnableCollider());
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(colliderDelay);
        starCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == caster)
            return;

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

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                center,
                explosionRadius,
                hitLayers);

        foreach (Collider2D hit in hits)
        {
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
        }

        ScreenShake.Instance.Shake(
        0.08f,
        0.08f);

        Destroy(gameObject);
    }
}