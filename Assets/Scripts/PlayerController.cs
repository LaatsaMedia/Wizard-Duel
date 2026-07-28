using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private StatusEffectController statusEffects;
    private KnockbackReceiver knockback;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 10f;
    private MovementController movement;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("References")]
    [SerializeField] private Transform aimPivot;
    [SerializeField] private Transform visualRoot;
    [SerializeField] private GameObject arm;

    private Rigidbody2D rb;
    private Camera cam;

    private float horizontal;
    private Vector2 mousePosition;
    private bool jumpPressed;

    private bool canDoubleJump;
    private bool usedDoubleJump;

    public float HorizontalInput => horizontal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        statusEffects = GetComponent<StatusEffectController>();
        knockback = GetComponent<KnockbackReceiver>();
        movement = GetComponent<MovementController>();
    }

    private void Update()
    {
        if (!MatchManager.RoundActive)
            return;

        // Still allow aiming while rooted.
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        RotateAimPivot();

        // Root prevents movement and jumping only.
        if (statusEffects.IsRooted ||
            statusEffects.IsFrozen ||
            statusEffects.IsStunned)
        {
            horizontal = 0f;
            jumpPressed = false;
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.W) && !statusEffects.IsGrounded)
        {
            if (IsGrounded())
            {
                jumpPressed = true;
            }
            else if (canDoubleJump && !usedDoubleJump)
            {
                jumpPressed = true;
                usedDoubleJump = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (knockback != null && knockback.IsKnockedBack)
            return;
            
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

        rb.linearVelocity = new Vector2(
            horizontal * moveSpeed * movement.MovementMultiplier,
            rb.linearVelocity.y);

        if (jumpPressed)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                0f);

            rb.AddForce(
                Vector2.up * jumpForce,
                ForceMode2D.Impulse);

            jumpPressed = false;
        }
    }

private void RotateAimPivot()
{
    Vector2 direction = mousePosition - (Vector2)aimPivot.position;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    bool facingLeft = direction.x < 0f;

    // Flip body
    visualRoot.localScale = new Vector3(
        facingLeft ? -1f : 1f,
        1f,
        1f);

    // Rotate arm towards mouse
    aimPivot.rotation = Quaternion.Euler(0f, 0f, angle);

    // Flip the arm when facing left
    if (facingLeft)
    {
        arm.transform.localEulerAngles = new Vector3(0f, 180f, 180f);
    }
    else
    {
        arm.transform.localEulerAngles = Vector3.zero;
    }
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