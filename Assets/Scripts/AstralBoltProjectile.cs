using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AstralBoltProjectile : SpellBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 24f;
    [SerializeField] private float lifetime = 2f;

    [Header("Homing")]
    [SerializeField] private float homingStrength = 180f;
    [SerializeField] private float detectionRadius = 20f;

    [Header("Damage")]
    [SerializeField] private float damage = 8f;

    [Header("VFX")]
    [SerializeField] private GameObject impactPrefab;

    [Header("SFX")]
    [SerializeField] private AudioSource travelSFX;

    private Rigidbody2D rb;
    private Transform target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (travelSFX != null)
            travelSFX.Play();

        FindTarget();

        rb.linearVelocity = transform.right * speed;

        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;

        float distance = Vector2.Distance(
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
            health.TakeDamage(damage);
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