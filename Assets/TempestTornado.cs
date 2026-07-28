using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TempestTornado : SpellBehaviour
{
    [Header("Damage")]
    [SerializeField] private float minimumDamagePerSecond = 5f;
    [SerializeField] private float maximumDamagePerSecond = 25f;
    [SerializeField] private float onHitValue = 0.1f;

    [SerializeField] private float damageTickInterval = 0.25f;
    [SerializeField] private Collider2D damageCollider;

    private float nextDamageTick;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lifetime = 8f;

    [Header("Growth")]
    [SerializeField] private float startScale = 0.5f;
    [SerializeField] private float endScale = 3f;

    [Header("Ground")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCastDistance = 2f;
    [SerializeField] private float groundOffset = 0.05f;

    [SerializeField]
    private AnimationCurve growthCurve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Pull")]
    [SerializeField] private LayerMask affectedLayers;

    [SerializeField] private float minimumPullRadius = 1.5f;
    [SerializeField] private float maximumPullRadius = 6f;

    [SerializeField] private float minimumPullForce = 3f;
    [SerializeField] private float maximumPullForce = 25f;

    [Header("Scaling")]
    [SerializeField] private Transform[] scalableObjects;

    [Header("Animation")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite[] animationFrames;

    [SerializeField]
    [Min(0.01f)]
    private float animationInterval = 0.1f;

    private int currentFrame;
    private float nextAnimationTime;

    private Rigidbody2D rb;

    private float direction;
    private float age;


    public float CurrentScalePercent { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (spriteRenderer != null &&
            animationFrames.Length > 0)
        {
            spriteRenderer.sprite = animationFrames[0];
        }

        direction = Mathf.Sign(transform.right.x);

        // Keep the tornado upright.
        transform.rotation = Quaternion.identity;

        transform.localScale =
            Vector3.one * startScale;

        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        age += Time.fixedDeltaTime;

        CurrentScalePercent =
            Mathf.Clamp01(age / lifetime);

        float growth =
            growthCurve.Evaluate(CurrentScalePercent);

        // Grow.
        float currentScale =
            Mathf.Lerp(
                startScale,
                endScale,
                growth);

        transform.localScale =
            Vector3.one * currentScale;

        foreach (Transform scalableObject in scalableObjects)
        {
            if (scalableObject == null)
                continue;

            scalableObject.localScale =
                Vector3.one * currentScale;
        }

// Move horizontally.
Vector2 nextPosition =
    rb.position +
    Vector2.right *
    direction *
    moveSpeed *
    Time.fixedDeltaTime;

// Ground follow.
RaycastHit2D hit =
    Physics2D.Raycast(
        groundCheck.position,
        Vector2.down,
        groundCastDistance,
        groundLayer);

if (hit.collider != null)
{
    float difference =
        hit.point.y +
        groundOffset -
        groundCheck.position.y;

    nextPosition.y += difference;
}

rb.MovePosition(nextPosition);

        // Pull.
        float currentRadius =
            Mathf.Lerp(
                minimumPullRadius,
                maximumPullRadius,
                growth);

        float currentPullForce =
            Mathf.Lerp(
                minimumPullForce,
                maximumPullForce,
                growth);

        float currentDamagePerSecond =
    Mathf.Lerp(
        minimumDamagePerSecond,
        maximumDamagePerSecond,
        growth);

        ApplyPull(
            currentRadius,
            currentPullForce);

        ApplyDamage(currentDamagePerSecond);

        Animate();
    }

    private void Animate()
    {
        if (spriteRenderer == null)
            return;

        if (animationFrames.Length == 0)
            return;

        if (Time.time < nextAnimationTime)
            return;

        nextAnimationTime =
            Time.time + animationInterval;

        currentFrame++;

        if (currentFrame >= animationFrames.Length)
            currentFrame = 0;

        spriteRenderer.sprite =
            animationFrames[currentFrame];
    }

    private void ApplyPull(
        float pullRadius,
        float maximumPullForce)
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(
                transform.position,
                pullRadius,
                affectedLayers);

        foreach (Collider2D collider in colliders)
        {
            if (!collider.TryGetComponent(out Health health))
                continue;

            if (!collider.TryGetComponent(out Rigidbody2D body))
                continue;

            Vector2 directionToCenter =
                (Vector2)transform.position -
                body.position;

            float distance =
                directionToCenter.magnitude;

            if (distance < 0.01f)
                continue;

            // Stronger the closer to the center.
            float strength =
                1f -
                Mathf.Clamp01(
                    distance / pullRadius);

            strength *= strength;

            body.AddForce(
                directionToCenter.normalized *
                maximumPullForce *
                strength,
                ForceMode2D.Force);
        }
    }

    private void ApplyDamage(float damagePerSecond)
    {
        if (damageCollider == null)
            return;

        if (Time.time < nextDamageTick)
            return;

        nextDamageTick =
            Time.time + damageTickInterval;

        float damage =
            damagePerSecond *
            damageTickInterval;

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(affectedLayers);
        filter.useLayerMask = true;

        Collider2D[] results = new Collider2D[32];

        int count =
            damageCollider.Overlap(filter, results);

        for (int i = 0; i < count; i++)
        {
            Collider2D collider = results[i];

            if (!collider.TryGetComponent(out Health health))
                continue;

            SpellEffects.DealDamage(
                caster,
                Spell,
                health,
                damage,
                onHitValue);
        }
    }

    private void OnDrawGizmosSelected()
    {
        float radius =
            Application.isPlaying
            ? Mathf.Lerp(
                minimumPullRadius,
                maximumPullRadius,
                CurrentScalePercent)
            : minimumPullRadius;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(
            transform.position,
            radius);
    }
}