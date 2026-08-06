using UnityEngine;

public class ArcaneOverload : SpellBehaviour
{
    [SerializeField] private float duration = 3f;

    private void Start()
    {
        if (caster.TryGetComponent(out SpellCaster spellCaster))
        {
            spellCaster.IgnoreCooldowns(duration);
        }

        Destroy(gameObject);
    }
}