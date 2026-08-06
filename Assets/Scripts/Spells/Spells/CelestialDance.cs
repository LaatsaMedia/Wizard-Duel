using UnityEngine;
using System.Collections;

public class CelestialDance : SpellBehaviour
{
    [SerializeField] private CelestialDanceVisual visualPrefab;
    private CelestialDanceVisual visual;
    [SerializeField] private Vector3 visualOffset = Vector3.up * 0.8f;

    [SerializeField] private float starInterval = 0.75f;
    [SerializeField] private float releaseDelay = 0.25f;
    [SerializeField] private float chargeDuration = 3.5f;
    [SerializeField] private int maxStars = 4;
    public override bool SupportsRecast => true;

    [SerializeField] private AnimationCurve levitationCurve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [SerializeField] private float levitationHeight = 1.5f;

    [SerializeField] private float levitationDuration = 3f;

    private bool charging = true;
    private float timer;
    private int stars;
    private float nextStarTime = 0.75f;
    

    private void Start()
    {
        if (caster.TryGetComponent(out PlayerController player))
        {
            player.StartLevitation(
                levitationCurve,
                levitationHeight,
                levitationDuration);
        }

        if (caster.TryGetComponent(out EnemyController enemy))
        {
            enemy.StartLevitation(
                levitationCurve,
                levitationHeight,
                levitationDuration);
        }

        visual = Instantiate(visualPrefab, caster.transform);
        visual.transform.localPosition = visualOffset;

        SpriteRenderer renderer =
            caster.GetComponentInChildren<SpriteRenderer>();

        visual.Initialize(renderer);
    }

    private void Update()
    {
        if (!charging)
            return;

        timer += Time.deltaTime;

        if (timer >= nextStarTime &&
            stars < maxStars)
        {
            stars++;
            nextStarTime += starInterval;

            visual.AddStar(
                caster,
                Spell);
        }

        if (timer >= chargeDuration)
        {
            Release();
        }
    }

    private void Release()
    {
        if (!charging)
            return;

        charging = false;

        if (caster.TryGetComponent(out SpellCaster spellCaster))
        {
            spellCaster.UltimateSlot.activeRecastSpell = null;
        }

        if (caster.TryGetComponent(out PlayerController player))
        {
            player.StopLevitation();
        }

        if (caster.TryGetComponent(out EnemyController enemy))
        {
            enemy.StopLevitation();
        }

        StartCoroutine(LaunchStars());
    }

    public override bool Recast()
    {
        Release();
        return true;
    }

    private IEnumerator LaunchStars()
    {
        while (visual != null &&
            visual.HasStars)
        {
            visual.LaunchFirstStar(
                GetTargetPosition());

            yield return new WaitForSeconds(
                releaseDelay);
        }

        Destroy(visual.gameObject);

        Destroy(gameObject);
    }

    private Vector3 GetTargetPosition()
    {
        if (caster.TryGetComponent(out PlayerController player))
        {
            Vector3 mouse =
                Camera.main.ScreenToWorldPoint(
                    Input.mousePosition);

            mouse.z = 0f;

            return mouse;
        }

        Health[] healths =
            FindObjectsByType<Health>(
                FindObjectsSortMode.None);

        Team myTeam =
            caster.GetComponent<Health>().Team;

        foreach (Health health in healths)
        {
            if (health.Team != myTeam)
            {
                return health.transform.position;
            }
        }

        return transform.position;
    }
}