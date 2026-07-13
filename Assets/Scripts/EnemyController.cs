using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    private StatusEffectController statusEffects;
    private KnockbackReceiver knockback;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 10f;
    private MovementController movement;

    [Header("Movement Variety")]

    [SerializeField] private float jumpDecisionInterval = 0.8f;

    [SerializeField] [Range(0f, 1f)] private float baseJumpChance = 0.1f;

    private float nextJumpDecision;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float retreatCheckDistance = 2f;
    [SerializeField] private LayerMask wallLayer;

    [Header("Hazard")]
    [SerializeField] private LayerMask hazardLayer;
    [SerializeField] private float hazardAvoidDistance = 2f;
    private bool escapingHazard;

    [Header("References")]
    [SerializeField] private Transform aimPivot;
    private Transform target;

    [Header("AI")]
    [SerializeField] private float jumpHeightDifference = 1.5f;

    [SerializeField] private float preferredDistance = 7f;
    [SerializeField] private float distanceTolerance = 1.5f;

    [SerializeField] private float minimumDecisionInterval = 0.5f;
    [SerializeField] private float maximumDecisionInterval = 2f;

    private float aggression;
    private float decisionInterval = 1.5f;

    private float nextDecisionTime;
    private float desiredHorizontal;

    private bool canDoubleJump;
    private bool usedDoubleJump;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<MovementController>();
        statusEffects = GetComponent<StatusEffectController>();
        knockback = GetComponent<KnockbackReceiver>();
    }

    public void SetAggression(float value)
    {
        aggression = Mathf.Clamp(value, -5f, 5f);

        decisionInterval = Mathf.Lerp(
            maximumDecisionInterval,
            minimumDecisionInterval,
            Mathf.InverseLerp(-5f, 5f, aggression));
    }

    public void SetPreferredDistance(float value)
    {
        // Aggressive enemies naturally stay closer.
        value -= aggression * 1.5f;

        preferredDistance = Mathf.Clamp(
            value,
            3f,
            24f);
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
        if (knockback != null &&
            knockback.IsKnockedBack)
            return;

        if (statusEffects.IsRooted ||
            statusEffects.IsFrozen ||
            statusEffects.IsStunned)
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y);

            return;
        }

        float escapeDirection;

        if (IsNearHazard(out escapeDirection))
        {
            desiredHorizontal = escapeDirection;

            rb.linearVelocity = new Vector2(
                desiredHorizontal *
                moveSpeed *
                movement.MovementMultiplier,
                rb.linearVelocity.y);

            if (IsGrounded())
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    jumpForce);
            }

            return;
        }

        if (IsGrounded())
            usedDoubleJump = false;

        float distance =
            Vector2.Distance(
                transform.position,
                target.position);

        if (Time.time >= nextDecisionTime)
        {
            nextDecisionTime =
                Time.time + decisionInterval;

            bool cornered = IsCornered();

            if (distance > preferredDistance + distanceTolerance)
            {
                // Move towards player.
                desiredHorizontal =
                    Mathf.Sign(
                        target.position.x -
                        transform.position.x);
            }
            else if (distance < preferredDistance - distanceTolerance)
            {
                // Move away.
                if (!cornered)
                {
                    desiredHorizontal =
                        -Mathf.Sign(
                            target.position.x -
                            transform.position.x);
                }
                else
                {
                    desiredHorizontal =
                        Random.value < 0.4f
                        ? 0f
                        : Mathf.Sign(
                            target.position.x -
                            transform.position.x);
                }
            }
            else
            {
                // Hover around preferred range.
                float roll = Random.value;

                if (!cornered)
                {
                    desiredHorizontal =
                        roll < 0.5f
                        ? 0f
                        : -Mathf.Sign(
                            target.position.x -
                            transform.position.x);
                }
                else
                {
                    desiredHorizontal =
                        roll < 0.5f
                        ? 0f
                        : Mathf.Sign(
                            target.position.x -
                            transform.position.x);
                }
            }
        }

        rb.linearVelocity = new Vector2(
            desiredHorizontal *
            moveSpeed *
            movement.MovementMultiplier,
            rb.linearVelocity.y);

        if (Time.time >= nextJumpDecision)
        {
            nextJumpDecision =
                Time.time + jumpDecisionInterval;

            // More aggressive enemies jump more often.
            float jumpChance =
                baseJumpChance +
                Mathf.InverseLerp(-5f, 5f, aggression) * 0.35f;

            // Only jump while actually moving.
            if (Mathf.Abs(desiredHorizontal) > 0.1f &&
                Random.value < jumpChance)
            {
                if (IsGrounded())
                {
                    rb.linearVelocity = new Vector2(
                        rb.linearVelocity.x,
                        jumpForce);
                }
                else if (canDoubleJump &&
                        !usedDoubleJump)
                {
                    usedDoubleJump = true;

                    rb.linearVelocity = new Vector2(
                        rb.linearVelocity.x,
                        jumpForce);
                }
            }
        }

        float heightDifference =
            target.position.y -
            transform.position.y;

        if (heightDifference > jumpHeightDifference)
        {
            if (IsGrounded())
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    jumpForce);
            }
            else if (canDoubleJump &&
                     !usedDoubleJump)
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
            -Mathf.Sign(
                target.position.x -
                transform.position.x);

        RaycastHit2D hit =
            Physics2D.Raycast(
                transform.position,
                Vector2.right * retreatDirection,
                retreatCheckDistance,
                wallLayer);

        if (!hit.collider)
            return false;

        float distanceToPlayer =
            Vector2.Distance(
                transform.position,
                target.position);

        float retreatNeeded =
            Mathf.Max(
                0f,
                preferredDistance -
                distanceToPlayer);

        return hit.distance < retreatNeeded;
    }

    private bool IsNearHazard(out float escapeDirection)
    {
        escapeDirection = 0f;

        Collider2D hazard = Physics2D.OverlapCircle(
            transform.position,
            hazardAvoidDistance,
            hazardLayer);

        if (hazard == null)
            return false;

        // Always move directly away from the hazard.
        float difference =
            transform.position.x -
            hazard.bounds.center.x;

        // If we're almost perfectly aligned, keep moving in the
        // same direction instead of flipping back and forth.
        if (Mathf.Abs(difference) < 0.15f)
        {
            escapeDirection =
                desiredHorizontal == 0f
                ? 1f
                : Mathf.Sign(desiredHorizontal);
        }
        else
        {
            escapeDirection =
                Mathf.Sign(difference);
        }

        return true;
    }

    private void RotateAimPivot()
    {
        Vector2 direction =
            target.position -
            aimPivot.position;

        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x) *
            Mathf.Rad2Deg;

        aimPivot.rotation =
            Quaternion.Euler(
                0f,
                0f,
                angle);
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
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundRadius);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            hazardAvoidDistance);
    }

    #region DOUBLE JUMP

    public void EnableDoubleJump()
    {
        canDoubleJump = true;
    }

    #endregion
}