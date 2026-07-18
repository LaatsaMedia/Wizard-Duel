using System.Collections;
using UnityEngine;

public class StoneWall : SpellBehaviour
{
    public override bool IsGroundSpell => true;

    [Header("Launch")]
    [SerializeField] private float damage = 12f;
    [SerializeField] private float horizontalKnockback = 1f;
    [SerializeField] private float verticalKnockback = 10f;
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private Transform hitPoint;
    [SerializeField] private Vector2 hitSize = new Vector2(2f, 1f);
    [SerializeField] private LayerMask hitLayers;

    [Header("Animation")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] riseSprites;
    [SerializeField] private float riseTime = 0.6f;

    [Header("Lifetime")]
    [SerializeField] private float lifetime = 5f;

    [Header("Spawn")]
    [SerializeField] private float spawnHeight = 1f;

    [Header("Collision")]
    [SerializeField] private Collider2D wallCollider;

    private void Start()
    {
        // Start slightly below (or above) the ground.
        transform.position += Vector3.up * spawnHeight;

        // Ensure the wall is always upright.
        transform.rotation = Quaternion.identity;

        StartCoroutine(RiseRoutine());
    }

    private IEnumerator RiseRoutine()
    {
        if (wallCollider != null)
        {
            wallCollider.enabled = false;
        }

        float frameTime = riseTime / riseSprites.Length;

        for (int i = 0; i < riseSprites.Length; i++)
        {
            spriteRenderer.sprite = riseSprites[i];
            yield return new WaitForSeconds(frameTime);
        }

        if (wallCollider != null)
        {
            wallCollider.enabled = true;
        }

        LaunchTargets();

        yield return new WaitForSeconds(lifetime);

        Destroy(gameObject);
    }

    private void LaunchTargets()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            hitPoint.position,
            hitSize,
            0f,
            hitLayers);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Health health))
            {
                SpellEffects.DealDamage(
                    caster,
                    Spell,
                    health,
                    damage);
            }

            if (hit.TryGetComponent(out KnockbackReceiver knockback))
            {
                float direction =
                    Mathf.Sign(hit.transform.position.x - transform.position.x);

                if (Mathf.Abs(direction) < 0.1f)
                    direction = castDirection;

                Vector2 force = new Vector2(
                    direction * horizontalKnockback,
                    verticalKnockback);

                knockback.ApplyKnockback(
                    force,
                    knockbackDuration);
            }
        }
    }
}