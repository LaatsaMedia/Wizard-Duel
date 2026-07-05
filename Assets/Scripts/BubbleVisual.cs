using UnityEngine;

public class BubbleVisual : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float maxFloatHeight = 1.5f;

    [Header("VFX")]
    [SerializeField] private GameObject popEffectPrefab;

    private StatusEffectController statusEffects;
    private Transform target;
    private Rigidbody2D targetRb;

    private Vector3 offset;
    private float currentHeight;
    private float startY;

    public void Initialize(StatusEffectController statusEffects)
    {
        this.statusEffects = statusEffects;

        target = statusEffects.transform;
        targetRb = target.GetComponent<Rigidbody2D>();

        startY = target.position.y;

        offset = transform.position - target.position;
    }

    private void Update()
    {
        if (statusEffects == null || !statusEffects.IsStunned)
        {
            Pop();
            return;
        }

        if (currentHeight < maxFloatHeight)
        {
            float movement = floatSpeed * Time.deltaTime;

            currentHeight += movement;
            currentHeight = Mathf.Min(currentHeight, maxFloatHeight);

            if (targetRb != null)
            {
                targetRb.MovePosition(
                    targetRb.position + Vector2.up * movement);
            }
        }

        Vector3 bubblePosition = target.position;
        bubblePosition.y += offset.y;

        transform.position = bubblePosition;
    }

    private void Pop()
    {
        if (popEffectPrefab != null)
        {
            Instantiate(
                popEffectPrefab,
                transform.position,
                Quaternion.identity);
        }

        Destroy(gameObject);
    }
}