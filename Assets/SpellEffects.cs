using System.Collections.Generic;
using UnityEngine;

public static class SpellEffects
{
    public static void DealDamage(
        GameObject caster,
        Spell spell,
        Health target,
        float damage)
    {
        if (caster == null ||
            spell == null ||
            target == null)
            return;

        SpellCaster spellCaster =
            caster.GetComponent<SpellCaster>();

        if (spellCaster == null)
        {
            target.TakeDamage(damage);
            return;
        }

        WizardBuild build =
            spellCaster.Build;

        List<OnHitModifier> modifiers =
            build.GetOnHitModifiersForSpell(spell);

        float finalDamage = damage;

        // Apply damage modifiers.
        foreach (OnHitModifier modifier in modifiers)
        {
            switch (modifier.Modifier)
            {
                case OnHitModifierType.DamageMultiplier:
                    finalDamage *=
                        1f + modifier.Value / 100f;
                    break;
            }
        }

        target.TakeDamage(finalDamage);

        // Apply secondary effects.
        foreach (OnHitModifier modifier in modifiers)
        {
            switch (modifier.Modifier)
            {
                case OnHitModifierType.Burn:

                if (target.TryGetComponent(out StatusEffectController statusEffects))
                {
                    statusEffects.ApplyBurn(
                        modifier.Value,
                        modifier.Duration);
                }

                break;

                case OnHitModifierType.Slow:

                    if (target.TryGetComponent(
                        out StatusEffectController slowStatus))
                    {
                        // TODO:
                        // slowStatus.ApplySlow(
                        //     modifier.Value,
                        //     modifier.Duration);

                        Debug.Log(
                            $"Slow {modifier.Value}% for {modifier.Duration} sec.");
                    }

                    break;
            }
        }
    }
}