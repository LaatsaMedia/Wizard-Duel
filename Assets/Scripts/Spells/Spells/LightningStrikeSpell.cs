using System.Collections;
using UnityEngine;

public class LightningStrikeSpell : SpellBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;

    [Header("Strike")]
    [SerializeField] private float warningDuration = 0.5f;

    [Header("Lightning")]
    [SerializeField] private Vector2 lightningHitbox = new Vector2(1f, 3f);
    [SerializeField] private float lightningDamage = 40f;

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float maxExplosionDamage = 20f;

    [Header("Prefabs")]
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private GameObject audioPrefab;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask hitLayers;

    private void Start()
    {
        Vector2 strikePosition;

        if (caster.TryGetComponent(out Health health) &&
            health.Team == Team.Player)
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            strikePosition = FindGround(mouseWorld);
        }
        else
        {
            // Enemy aims at the player's current position.
            strikePosition = FindGround(
                MatchManager.Instance.CurrentPlayer.transform.position);
        }

        StartCoroutine(Strike(strikePosition));
    }

    private IEnumerator Strike(Vector2 position)
    {
        Vector3 warningPosition = position + Vector2.up;

        GameObject warning = Instantiate(
            warningPrefab,
            warningPosition,
            Quaternion.identity);

        StartCoroutine(AnimateWarning(warning.transform));

        yield return new WaitForSeconds(warningDuration);

        Destroy(warning);

        Instantiate(
            lightningPrefab,
            position,
            Quaternion.identity);

        Instantiate(
            audioPrefab,
            position,
            Quaternion.identity);

        if (explosionEffectPrefab != null)
        {
            Instantiate(
                explosionEffectPrefab,
                position,
                Quaternion.identity);
        }

        // Lightning column
        Collider2D[] lightningHits = Physics2D.OverlapBoxAll(
            position + Vector2.up * lightningHitbox.y * 0.5f,
            lightningHitbox,
            0f,
            hitLayers);

        foreach (Collider2D hit in lightningHits)
        {
            if (hit.TryGetComponent(out Health health)){
                if (RegisterHit(health))
                {
                    SpellEffects.DealDamage(
                        caster,
                        Spell,
                        health,
                        lightningDamage);
                }
            }
        }

        // Explosion
        Collider2D[] explosionHits = Physics2D.OverlapCircleAll(
            position,
            explosionRadius,
            hitLayers);

        foreach (Collider2D hit in explosionHits)
        {
            float distance = Vector2.Distance(position, hit.transform.position);
            float t = Mathf.Clamp01(distance / explosionRadius);

            float damage = Mathf.Lerp(maxExplosionDamage, 0f, t);

            if (hit.TryGetComponent(out Health health)){
                if (RegisterHit(health))
                {
                    SpellEffects.DealDamage(
                        caster,
                        Spell,
                        health,
                        damage);
                }
            }
        }

        ScreenShake.Instance.Shake(0.5f, 0.3f);

        Destroy(gameObject);
    }

    private IEnumerator AnimateWarning(Transform warning)
    {
        float duration = 0.1f;

        float elapsed = 0f;

        Vector3 startScale = Vector3.one * 0.75f;
        Vector3 endScale = Vector3.one * 1.25f;

        warning.localScale = startScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;
            warning.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        warning.localScale = endScale;
    }

    private Vector2 FindGround(Vector2 point)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            point + Vector2.up * 20f,
            Vector2.down,
            40f,
            groundLayer);

        if (hit.collider != null)
            return hit.point;

        return point;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(
            transform.position + Vector3.up * lightningHitbox.y * 0.5f,
            lightningHitbox);
    }
}