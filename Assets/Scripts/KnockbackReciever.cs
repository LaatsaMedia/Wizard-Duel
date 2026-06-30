using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockbackReceiver : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool IsKnockedBack => knockbackTimer > 0f;
    private float knockbackTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.fixedDeltaTime;
        }
    }

    public void ApplyKnockback(Vector2 force, float duration)
    {
        knockbackTimer = duration;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}