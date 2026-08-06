using System.Collections.Generic;
using UnityEngine;

public class CelestialDanceVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CelestialStar starPrefab;

    [Header("Orbit")]
    [SerializeField] private float orbitRadius = 0.6f;
    [SerializeField] private float orbitHeight = 0.25f;
    [SerializeField] private float orbitSpeed = 120f;

    [Header("Depth")]
    [SerializeField] private float frontScale = 1f;
    [SerializeField] private float backScale = 0.85f;

    private readonly List<CelestialStar> stars = new();
    public bool HasStars => stars.Count > 0;

    private SpriteRenderer ownerRenderer;
    private float angle;

    public void Initialize(SpriteRenderer renderer)
    {
        ownerRenderer = renderer;
    }

    public void AddStar(
        GameObject caster,
        Spell spell)
    {
        CelestialStar star =
            Instantiate(
                starPrefab,
                transform);

        star.Initialize(
            caster,
            spell,
            0f);

        stars.Add(star);
    }

    public void LaunchFirstStar(Vector3 targetPosition)
    {
        if (stars.Count == 0)
            return;

        CelestialStar star = stars[0];

        stars.RemoveAt(0);

        star.Launch(targetPosition);
    }

    private void Update()
    {
        if (transform.parent == null)
        {
            Destroy(gameObject);
            return;
        }

        angle += orbitSpeed * Time.deltaTime;

        for (int i = 0; i < stars.Count; i++)
        {
            CelestialStar star = stars[i];

            float starAngle =
                angle + (360f / stars.Count) * i;

            float radians =
                starAngle * Mathf.Deg2Rad;

            Vector3 position = new(
                Mathf.Cos(radians) * orbitRadius,
                Mathf.Sin(radians) * orbitHeight,
                0f);

            star.SetOrbitPosition(position);

            bool inFront = position.y > 0f;

            star.Renderer.sortingOrder =
                ownerRenderer.sortingOrder +
                (inFront ? 2 : -2);

            float scale =
                inFront
                ? frontScale
                : backScale;

            star.transform.localScale =
                Vector3.one * scale;
        }
    }
}