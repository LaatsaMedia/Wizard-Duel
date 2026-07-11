using UnityEngine;

public class Barrier : MonoBehaviour
{
    private float currentBarrierHealth;

    public float CurrentBarrierHealth => currentBarrierHealth;
    public float MaxBarrierHealth { get; private set; }

    public bool IsBroken => currentBarrierHealth <= 0f;

    public void Initialize(float health)
    {
        MaxBarrierHealth = health;
        currentBarrierHealth = health;
    }

    public float AbsorbDamage(float damage)
    {
        if (currentBarrierHealth <= 0f)
            return damage;

        float absorbed = Mathf.Min(currentBarrierHealth, damage);

        currentBarrierHealth -= absorbed;

        return damage - absorbed;
    }
}