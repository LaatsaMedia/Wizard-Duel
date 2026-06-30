using UnityEngine;

public class HealSpell : SpellBehaviour
{
    [SerializeField] private float healAmount = 30f;

    [Header("VFX")]
    [SerializeField] private GameObject healEffectPrefab;

    private void Start()
    {
        if (caster.TryGetComponent(out Health health))
        {
            health.Heal(healAmount);
        }

        if (healEffectPrefab != null)
        {
            Instantiate(
                healEffectPrefab,
                caster.transform.position,
                Quaternion.identity);
        }

        Destroy(gameObject);
    }
}