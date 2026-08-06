using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Team")]
    [SerializeField] private Team team;
    public Team Team => team;

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float HealthPercent => currentHealth / maxHealth;

    private StatusEffectController statusEffects;

    private void Awake()
    {
        currentHealth = maxHealth;
        statusEffects = GetComponent<StatusEffectController>();
    }

    public void TakeDamage(
        float damage,
        bool consumeFrozen = true)
    {
        if(!MatchManager.RoundActive)
            return;

        Barrier barrier = GetComponent<Barrier>();

        if (barrier != null)
        {
            damage = barrier.AbsorbDamage(damage);

            if (barrier.IsBroken)
            {
                Destroy(barrier);
            }

            if (damage <= 0f)
                return;
        }

        if (consumeFrozen &&
            statusEffects != null &&
            statusEffects.ConsumeFrozen())
        {
            damage *= 1.5f;
        }

        currentHealth -= damage;

        if (TryGetComponent(out UltimateCharge ultimate))
        {
            ultimate.AddChargeFromDamageTaken(damage);
        }

        if (TryGetComponent(out HitController hitController))
        {
            hitController.PlayHitFeedback();
        }

        if (currentHealth <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        //Debug.Log($"{name} healed {amount}. Current HP: {currentHealth}");
    }

    private void Die()
    {
        MatchManager.Instance.EndRound(team, transform.position);

        Destroy(gameObject);
    }
}