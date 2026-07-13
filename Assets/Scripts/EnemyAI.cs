using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpellCaster spellCaster;
    [SerializeField] private Health health;

    private EnemyController controller;
    private Transform target;

    private float aggression;
    private float preferredRange;
    private float averagePriority;

    [Header("AI")]
    [SerializeField] private float castInterval = 0.25f;

    private float nextCastTime;

    private void Awake()
    {
        if (spellCaster == null)
            spellCaster = GetComponent<SpellCaster>();

        if (health == null)
            health = GetComponent<Health>();

        controller = GetComponent<EnemyController>();
    }

    private void FindTarget()
    {
        if (MatchManager.Instance.CurrentPlayer == null)
            return;

        target = MatchManager.Instance.CurrentPlayer.transform;

        AnalyzeBuild();
    }

    private void AnalyzeBuild()
    {
        aggression = 0f;
        preferredRange = 0f;
        averagePriority = 0f;

        int spellCount = 0;

        AddSpell(spellCaster.PrimarySpell.Spell);
        AddSpell(spellCaster.SecondarySpell.Spell);
        AddSpell(spellCaster.ThirdSpell.Spell);
        AddSpell(spellCaster.UltimateSlot.Spell);

        if (spellCount > 0)
        {
            aggression /= spellCount;
            preferredRange /= spellCount;
            averagePriority /= spellCount;
        }

        if (controller != null)
        {
            controller.SetAggression(aggression);
            controller.SetPreferredDistance(preferredRange);
        }

        Debug.Log(
            $"{name}\n" +
            $"Aggression: {aggression:F1}\n" +
            $"Preferred Range: {preferredRange:F1}\n" +
            $"Average Priority: {averagePriority:F1}");

        void AddSpell(Spell spell)
        {
            if (spell == null)
                return;

            aggression += spell.Aggression;
            preferredRange += spell.PreferredRange;
            averagePriority += spell.BaseCastPriority;

            spellCount++;
        }
    }

    private void Update()
    {
        if (!MatchManager.RoundActive)
            return;

        if (target == null)
        {
            FindTarget();
            return;
        }

        if (Time.time < nextCastTime)
            return;

        nextCastTime =
            Time.time + castInterval;

        CastBestSpell();
    }

    private void CastBestSpell()
    {
        float bestScore = float.MinValue;
        System.Action bestCast = null;

        EvaluateSpell(
            spellCaster.PrimarySpell,
            spellCaster.CastPrimary,
            ref bestScore,
            ref bestCast);

        EvaluateSpell(
            spellCaster.SecondarySpell,
            spellCaster.CastSecondary,
            ref bestScore,
            ref bestCast);

        EvaluateSpell(
            spellCaster.ThirdSpell,
            spellCaster.CastThird,
            ref bestScore,
            ref bestCast);

        EvaluateUltimate(
            spellCaster.UltimateSlot,
            spellCaster.CastUltimate,
            ref bestScore,
            ref bestCast);

        bestCast?.Invoke();
    }

    private void EvaluateSpell(
    SpellSlot slot,
    System.Action cast,
    ref float bestScore,
    ref System.Action bestCast)
    {
        if (slot.Spell == null)
            return;

        if (slot.cooldownRemaining > 0f)
            return;

        if (spellCaster.mana.CurrentMana < slot.Spell.manaCost)
            return;

        float distance = Vector2.Distance(
            transform.position,
            target.position);

        if (distance < slot.Spell.MinimumCastRange)
            return;

        if (distance > slot.Spell.MaximumCastRange)
            return;

        float score = slot.Spell.BaseCastPriority;

        // Prefer defensive spells when low HP.
        if (health.HealthPercent <=
            slot.Spell.IdealCastHealth)
        {
            score += 25f;
        }

        if (health.HealthPercent >
            slot.Spell.MaximumCastHealth)
        {
            return;
        }

        if (score > bestScore)
        {
            bestScore = score;
            bestCast = cast;
        }
    }

    private void EvaluateUltimate(
    UltimateSlot slot,
    System.Action cast,
    ref float bestScore,
    ref System.Action bestCast)
    {
        if (slot.Spell == null)
            return;

        if (!spellCaster.ultimateCharge.IsReady)
            return;

        float score =
            slot.Spell.BaseCastPriority + 20f;

        if (score > bestScore)
        {
            bestScore = score;
            bestCast = cast;
        }
    }
}