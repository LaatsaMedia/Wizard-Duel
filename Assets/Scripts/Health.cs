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

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        //Debug.Log($"{name} took {damage} damage.");

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