using System.Collections;
using UnityEngine;

public class StoneWall : SpellBehaviour
{
    public override bool IsGroundSpell => true;

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

        yield return new WaitForSeconds(lifetime);

        Destroy(gameObject);
    }
}