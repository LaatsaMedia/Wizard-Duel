using UnityEngine;

public class BlinkSpell : SpellBehaviour
{
    [Header("Blink")]
    [SerializeField] private float blinkDistance = 6f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float skinWidth = 0.1f;

    [Header("Effects")]
    [SerializeField] private GameObject departureEffect;
    [SerializeField] private GameObject arrivalEffect;

    private Camera mainCamera;

    public override void Initialize(GameObject caster, Spell spell, float castDirection)
    {
        base.Initialize(caster, spell, castDirection);

        mainCamera = Camera.main;

        Blink();

        Destroy(gameObject);
    }

    private void Blink()
    {
        Vector2 start = caster.transform.position;

        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector2 direction = ((Vector2)mouseWorld - start).normalized;

        float distanceToMouse = Vector2.Distance(start, mouseWorld);
        float travelDistance = Mathf.Min(distanceToMouse, blinkDistance);

        RaycastHit2D hit = Physics2D.Raycast(
            start,
            direction,
            travelDistance,
            obstacleLayer);

        Vector2 destination;

        if (hit.collider != null)
        {
            destination = hit.point - direction * skinWidth;
        }
        else
        {
            destination = start + direction * travelDistance;
        }

        // Departure effect
        if (departureEffect != null)
        {
            Instantiate(
                departureEffect,
                start,
                Quaternion.identity);
        }

        // Teleport
        caster.transform.position = destination;

        // Arrival effect
        if (arrivalEffect != null)
        {
            Instantiate(
                arrivalEffect,
                destination,
                Quaternion.identity);
        }
    }
}