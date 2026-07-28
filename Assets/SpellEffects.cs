using System.Collections.Generic;
using UnityEngine;

public static class SpellEffects
{
    public static void DealDamage(
        GameObject caster,
        Spell spell,
        Health target,
        float damage,
        float onHitMultiplier = 1f)
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

        float finalDamage =
            ApplyDamageModifiers(
                target,
                damage,
                modifiers,
                onHitMultiplier);

        target.TakeDamage(finalDamage);

        ApplyResourceModifiers(
            caster,
            target,
            finalDamage,
            modifiers,
            onHitMultiplier);

        ApplyStatusModifiers(
            target,
            modifiers,
            onHitMultiplier);

        ApplyUtilityModifiers(
            caster,
            target,
            modifiers,
            onHitMultiplier);
    }

    private static float ApplyDamageModifiers(
        Health target,
        float damage,
        List<OnHitModifier> modifiers,
        float onHitMultiplier)
    {
        float finalDamage = damage;

        foreach (OnHitModifier modifier in modifiers)
        {
            float value = modifier.Value * onHitMultiplier;

            switch (modifier.Modifier)
            {
                case OnHitModifierType.DamageMultiplier:
                    finalDamage *=
                        1f + value / 100f;
                    break;

                case OnHitModifierType.FlatDamage:
                    finalDamage += value;
                    break;

                case OnHitModifierType.CurrentHealthDamage:
                    finalDamage +=
                        target.CurrentHealth *
                        value / 100f;
                    break;

                case OnHitModifierType.MissingHealthDamage:
                    finalDamage +=
                        (target.MaxHealth - target.CurrentHealth) *
                        value / 100f;
                    break;

                case OnHitModifierType.MaxHealthDamage:
                    finalDamage +=
                        target.MaxHealth *
                        value / 100f;
                    break;
            }
        }

        return finalDamage;
    }

    private static void ApplyStatusModifiers(
        Health target,
        List<OnHitModifier> modifiers,
        float onHitMultiplier)
    {
    foreach (OnHitModifier modifier in modifiers)
    {
        float value = modifier.Value * onHitMultiplier;

        switch (modifier.Modifier)
        {
            case OnHitModifierType.Burn:

                if (target.TryGetComponent(
                    out StatusEffectController statusEffects))
                {
                    statusEffects.ApplyBurn(
                        value,
                        modifier.Duration);
                }

                break;

            case OnHitModifierType.Slow:

                if (target.TryGetComponent(
                    out StatusEffectController slowStatus))
                {
                    // TODO:
                    // slowStatus.ApplySlow(
                    //     value,
                    //     modifier.Duration);

                    Debug.Log(
                        $"Slow {value}% for {modifier.Duration} sec.");
                }

                break;
        }
    }
}

    private static void ApplyResourceModifiers(
        GameObject caster,
        Health target,
        float damageDealt,
        List<OnHitModifier> modifiers,
        float onHitMultiplier)
    {
        if (caster == target.gameObject)
            return;

        Health casterHealth = caster.GetComponent<Health>();
        Mana casterMana = caster.GetComponent<Mana>();
        Mana targetMana = target.GetComponent<Mana>();

        foreach (OnHitModifier modifier in modifiers)
        {
            float value = modifier.Value * onHitMultiplier;

            switch (modifier.Modifier)
            {
                case OnHitModifierType.LifeSteal:

                    if (casterHealth != null)
                    {
                        float healAmount =
                            damageDealt * value / 100f;

                        casterHealth.Heal(healAmount);
                    }

                    break;

                case OnHitModifierType.HealOnHit:

                    if (casterHealth != null)
                    {
                        casterHealth.Heal(value);
                    }

                    break;

                case OnHitModifierType.ManaRestoreOnHit:

                    if (casterMana != null)
                    {
                        casterMana.RestoreMana(value);
                    }

                    break;

                case OnHitModifierType.ManaBurn:

                    if (targetMana != null)
                    {
                        targetMana.BurnMana(value);
                    }

                    break;

                case OnHitModifierType.UltimateCharge:

                    // TODO:
                    // Ultimate system

                    break;
            }
        }
    }

    private static void ApplyUtilityModifiers(
        GameObject caster,
        Health target,
        List<OnHitModifier> modifiers,
        float onHitMultiplier)
    {
        foreach (OnHitModifier modifier in modifiers)
        {
            float value = modifier.Value * onHitMultiplier;

            switch (modifier.Modifier)
            {
                case OnHitModifierType.Knockback:

                    // TODO:
                    // Apply knockback using value.

                    break;

                case OnHitModifierType.Pull:

                    // TODO:
                    // Apply pull using value.

                    break;

                case OnHitModifierType.CooldownReductionOnHit:

                    // TODO:
                    // Reduce cooldowns using value.

                    break;
            }
        }
    }
}