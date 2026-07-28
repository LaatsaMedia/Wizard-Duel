using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AstralBoltProjectile : SpellBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float upwardSpeed = 10f;
    [SerializeField] private float upwardTime = 0.2f;
    [SerializeField] private float lifetime = 3f;

    [SerializeField] private Vector2 followOffset = new Vector2(0f, 1.5f);
    [SerializeField] private float followSmoothness = 15f;
    private bool followingCaster = true;

    [Header("Homing")]
    [SerializeField] private float homingStrength = 40f;
    [SerializeField] private float detectionRadius = 20f;

    [Header("Damage")]
    [SerializeField] private float damage = 8f;

    [Header("VFX")]
    [SerializeField] private GameObject impactPrefab;

    [Header("SFX")]
    [SerializeField] private AudioSource travelSFX;

    private Rigidbody2D rb;

    private Transform target;
    private Vector2 targetPosition;

    private bool launched;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (travelSFX != null)
            travelSFX.Play();

        FindTarget();

        // Rise upward first.
        rb.linearVelocity = Vector2.up * upwardSpeed;

        Invoke(nameof(BeginHoming), upwardTime);

        Destroy(gameObject, lifetime);
    }

    private void BeginHoming()
    {
        followingCaster = false;
        launched = true;

        if (target != null)
            targetPosition = target.position;

        Vector2 direction =
            (targetPosition - rb.position).normalized;

        float angle =
            Mathf.Atan2(direction.y, direction.x) *
            Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0f, 0f, angle);

        rb.linearVelocity =
            direction * speed;
    }

    private void FixedUpdate()
    {
        if (followingCaster)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            Vector2 desiredPosition =
                (Vector2)caster.transform.position +
                followOffset;

            rb.MovePosition(
                Vector2.Lerp(
                    rb.position,
                    desiredPosition,
                    followSmoothness * Time.fixedDeltaTime));

            transform.Rotate(
                0f,
                0f,
                360f * Time.fixedDeltaTime);

            return;
        }
        
        if (!launched)
            return;

        if (target == null)
            return;

        float distance =
            Vector2.Distance(
                transform.position,
                target.position);

        if (distance > detectionRadius)
            return;

        Vector2 desiredDirection =
            ((Vector2)target.position - rb.position).normalized;

        float rotateAmount =
            Vector3.Cross(
                desiredDirection,
                transform.right).z;

        rb.angularVelocity =
            -rotateAmount * homingStrength;

        rb.linearVelocity =
            transform.right * speed;
    }

    private void FindTarget()
    {
        if (!caster.TryGetComponent(out Health casterHealth))
            return;

        target = casterHealth.Team == Team.Player
            ? MatchManager.Instance.CurrentEnemy.transform
            : MatchManager.Instance.CurrentPlayer.transform;
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
                    damage);
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