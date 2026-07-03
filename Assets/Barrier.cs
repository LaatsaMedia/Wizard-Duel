using UnityEngine;

public class Barrier : MonoBehaviour
{
    private float currentBarrierHealth;

    public bool IsBroken => currentBarrierHealth <= 0f;

    public void Initialize(float health)
    {
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