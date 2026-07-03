using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    private StatusEffectController statusEffects;
    private KnockbackReceiver knockback;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float retreatCheckDistance = 2f;
    [SerializeField] private LayerMask wallLayer;

    [Header("References")]
    [SerializeField] private Transform aimPivot;
    private Transform target;

    [Header("AI")]
    [SerializeField] private float jumpHeightDifference = 1.5f;
    [SerializeField] private float preferredDistance = 7f;
    [SerializeField] private float distanceTolerance = 1.5f;
    [SerializeField] private float decisionInterval = 1.5f;

    private float nextDecisionTime;
    private float desiredHorizontal;

    private bool canDoubleJump;
    private bool usedDoubleJump;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        statusEffects = GetComponent<StatusEffectController>();
        knockback = GetComponent<KnockbackReceiver>();
    }

    private void FindTarget()
    {
        if (MatchManager.Instance.CurrentPlayer == null)
            return;

        target = MatchManager.Instance.CurrentPlayer.transform;
    }

    private void Update()
    {
        if (!MatchManager.RoundActive)
            return;

        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateAimPivot();
    }

    private void FixedUpdate()
    {
        if (!MatchManager.RoundActive)
            return;

        if (target == null)
        {
            FindTarget();
            return;
        }

        Move();
    }

    private void Move()
    {
        if (knockback != null && knockback.IsKnockedBack)
            return;
            
        // Root prevents movement only.
        if (statusEffects.IsRooted ||
            statusEffects.IsFrozen ||
            statusEffects.IsStunned)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        if (IsGrounded())
        {
            usedDoubleJump = false;
        }

        float distance = Vector2.Distance(transform.position, target.position);

        if (Time.time >= nextDecisionTime)
        {
            nextDecisionTime = Time.time + decisionInterval;

            bool cornered = IsCornered();

            // Too far away -> move closer.
            if (distance > preferredDistance + distanceTolerance)
            {
                desiredHorizontal =
                    Mathf.Sign(target.position.x - transform.position.x);
            }
            // Too close.
            else if (distance < preferredDistance - distanceTolerance)
            {
                if (!cornered)
                {
                    desiredHorizontal =
                        -Mathf.Sign(target.position.x - transform.position.x);
                }
                else
                {
                    float roll = Random.value;

                    if (roll < 0.4f)
                    {
                        desiredHorizontal = 0f;
                    }
                    else
                    {
                        desiredHorizontal =
                            Mathf.Sign(target.position.x - transform.position.x);
                    }
                }
            }
            // Preferred distance.
            else
            {
                float roll = Random.value;

                if (!cornered)
                {
                    if (roll < 0.5f)
                    {
                        desiredHorizontal = 0f;
                    }
                    else
                    {
                        desiredHorizontal =
                            -Mathf.Sign(target.position.x - transform.position.x);
                    }
                }
                else
                {
                    if (roll < 0.5f)
                    {
                        desiredHorizontal = 0f;
                    }
                    else
                    {
                        desiredHorizontal =
                            Mathf.Sign(target.position.x - transform.position.x);
                    }
                }
            }
        }

        rb.linearVelocity = new Vector2(
            desiredHorizontal * moveSpeed,
            rb.linearVelocity.y);

        float heightDifference =
            target.position.y - transform.position.y;

        if (heightDifference > jumpHeightDifference)
        {
            if (IsGrounded())
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    jumpForce);
            }
            else if (canDoubleJump && !usedDoubleJump)
            {
                usedDoubleJump = true;

                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    jumpForce);
            }
        }
    }

    private bool IsCornered()
    {
        float retreatDirection =
            -Mathf.Sign(target.position.x - transform.position.x);

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.right * retreatDirection,
            retreatCheckDistance,
            wallLayer);

        if (!hit.collider)
            return false;

        float distanceToPlayer =
            Vector2.Distance(transform.position, target.position);

        float retreatNeeded =
            Mathf.Max(0f, preferredDistance - distanceToPlayer);

        return hit.distance < retreatNeeded;
    }

    private void RotateAimPivot()
    {
        Vector2 direction = target.position - aimPivot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        aimPivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundRadius);
    }

    #region DOUBLE JUMP

    public void EnableDoubleJump()
    {
        canDoubleJump = true;
    }

    #endregion
}