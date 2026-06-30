using System.Text.RegularExpressions;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpellCaster spellCaster;
    [SerializeField] private Health health;

    private Transform target;

    [Header("AI")]
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float healThreshold = 0.4f;
    [SerializeField] private float castInterval = 0.25f;
    [SerializeField] private float lowHealthCastMultiplier = 1.5f;

    private float nextCastTime;

    private void Awake()
    {
        if (spellCaster == null)
            spellCaster = GetComponent<SpellCaster>();

        if (health == null)
            health = GetComponent<Health>();
    }

    private void FindTarget()
    {
        if (MatchManager.Instance.CurrentPlayer == null)
            return;

        target = MatchManager.Instance.CurrentPlayer.transform;
    }

    private void Update()
    {
        if(!MatchManager.RoundActive)
            return;
            
        if (target == null)
        {
            FindTarget();
            return;
        }

        if (Time.time < nextCastTime)
            return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackRange)
            return;

        float interval = castInterval;

        if (health.HealthPercent <= healThreshold)
            interval *= lowHealthCastMultiplier;

        nextCastTime = Time.time + interval;

        if (health.HealthPercent <= healThreshold)
            spellCaster.CastThird();

        if (spellCaster.PrimarySpell.cooldownRemaining <= 0f)
            spellCaster.CastPrimary();
        else
            spellCaster.CastSecondary();
    }
}