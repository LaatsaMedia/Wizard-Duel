using System.Collections;
using UnityEngine;

public class VineRootArea : SpellBehaviour
{
    [Header("Animation")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] animationFrames;
    [SerializeField] private float animationDuration = 0.5f;

    [Header("Hit")]
    [SerializeField] private BoxCollider2D hitbox;
    [SerializeField] private int hitFrame = 1;

    [Header("Effect")]
    [SerializeField] private StatusEffect rootEffect;
    [SerializeField] private float rootDuration = 2f;
    [SerializeField] private float damage = 15f;

    private void Start()
    {
        if (hitbox != null)
            hitbox.enabled = false;

        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        if (animationFrames == null || animationFrames.Length == 0)
        {
            Debug.LogWarning($"{name} has no animation frames assigned.");
            Destroy(gameObject);
            yield break;
        }

        float frameTime = animationDuration / animationFrames.Length;

        for (int i = 0; i < animationFrames.Length; i++)
        {
            spriteRenderer.sprite = animationFrames[i];

            if (i == hitFrame && hitbox != null)
            {
                hitbox.enabled = true;

                // Enable for exactly one physics step.
                yield return new WaitForFixedUpdate();

                hitbox.enabled = false;
            }

            yield return new WaitForSeconds(frameTime);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Health health))
        {
            SpellEffects.DealDamage(
            caster,
            Spell,
            health,
            damage);
        }

        if (other.TryGetComponent(out StatusEffectController statusEffects))
        {
            statusEffects.ApplyEffect(rootEffect, rootDuration);
        }
    }
}