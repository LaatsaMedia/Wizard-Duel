using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
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

    [Header("References")]
    [SerializeField] private Transform aimPivot;

    private Rigidbody2D rb;
    private Camera cam;

    private float horizontal;
    private Vector2 mousePosition;
    private bool jumpPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        statusEffects = GetComponent<StatusEffectController>();
        knockback = GetComponent<KnockbackReceiver>();
    }

    private void Update()
    {
        if (!MatchManager.RoundActive)
            return;

        // Still allow aiming while rooted.
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        RotateAimPivot();

        // Root prevents movement and jumping only.
        if (statusEffects.HasEffect(StatusEffectType.Root))
        {
            horizontal = 0f;
            jumpPressed = false;
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
            jumpPressed = true;
    }

    private void FixedUpdate()
    {
        if (knockback != null && knockback.IsKnockedBack)
            return;

        if (statusEffects.HasEffect(StatusEffectType.Root))
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }
        
        rb.linearVelocity = new Vector2(
            horizontal * moveSpeed,
            rb.linearVelocity.y);

        if (jumpPressed)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce);

            jumpPressed = false;
        }
    }

    private void RotateAimPivot()
    {
        Vector2 direction = mousePosition - (Vector2)aimPivot.position;
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
}