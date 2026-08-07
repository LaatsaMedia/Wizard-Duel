using UnityEngine;

[RequireComponent(typeof(Mana))]
public class BuildApplier : MonoBehaviour
{
    private Health health;
    private Mana mana;
    private PlayerController playerController;
    private EnemyController enemyController;
    private UltimateCharge ultimateCharge;
    private SpellCaster spellCaster;

    private void Awake()
    {
        health = GetComponent<Health>();
        mana = GetComponent<Mana>();
        playerController = GetComponent<PlayerController>();
        enemyController = GetComponent<EnemyController>();
        ultimateCharge = GetComponent<UltimateCharge>();
        spellCaster = GetComponent<SpellCaster>();
    }

    public void ApplyBuild(WizardBuild build)
    {
        ApplyPassiveModifiers(build);
        ApplySpecialEffects(build);
    }

    private void ApplyPassiveModifiers(WizardBuild build)
    {
        // Modifier Convention:
        //
        // Flat modifiers:
        //  Value is applied directly.
        //  Example: 20 = +20 Max Health.
        //
        // Percentage modifiers:
        //  Value is stored as a percentage.
        //  Example: 15 = +15%.
        //  Never store 1.15.
        //
        // Current percentage modifiers:
        // - SpellDamage
        // - CooldownRecovery
        // - WizardSize
        // - UltimateCharge

        foreach (PassiveModifier modifier in build.GetPassiveModifiers())
        {
            switch (modifier.Modifier)
            {
                case PassiveModifierType.ManaRegen:
                    mana.manaRegeneration += modifier.Value;
                    break;

                case PassiveModifierType.MaxMana:
                    mana.maxMana += modifier.Value;
                    mana.UpdateMana();
                    break;

                case PassiveModifierType.MaxHealth:
                    health.maxHealth += modifier.Value;
                    health.currentHealth = health.maxHealth;
                    break;

                case PassiveModifierType.MoveSpeed:

                    if (playerController != null)
                        playerController.moveSpeed += modifier.Value;

                    if (enemyController != null)
                        enemyController.moveSpeed += modifier.Value;

                    break;

                case PassiveModifierType.SpellDamage:
                    // TODO
                    break;

                case PassiveModifierType.CooldownRecovery:
                    spellCaster.CooldownRecovery += modifier.Value;
                    break;

                case PassiveModifierType.WizardSize:
                    transform.localScale *=
                        1f + modifier.Value / 100f;
                    break;

                case PassiveModifierType.JumpHeight:
                    if (playerController != null)
                        playerController.jumpForce += modifier.Value;

                    if (enemyController != null)
                        enemyController.jumpForce += modifier.Value;

                    break;
                
                case PassiveModifierType.UltimateCharge:
                    ultimateCharge.ChargeGainMultiplier *=
                        1f + modifier.Value / 100f;
                    break;
                
                case PassiveModifierType.HealthRegen:
                    health.healthRegeneration += modifier.Value;
                    break;

                case PassiveModifierType.DamageTakeManaRestore:
                    health.onHitManaRestore += modifier.Value;
                    break;

                case PassiveModifierType.DamageTakeManaRestorePercent:
                    health.onHitManaRestore += modifier.Value;
                    break;
            }
        }
    }

    private void ApplySpecialEffects(WizardBuild build)
    {
        Accessory[] accessories =
        {
            build.accessory1,
            build.accessory2,
            build.accessory3,
            build.accessory4,
            build.accessory5,
            build.accessory6
        };

        foreach (Accessory accessory in accessories)
        {
            if (accessory == null)
                continue;

            foreach (AccessoryEffect effect in accessory.SpecialEffects)
            {
                switch (effect)
                {
                    case AccessoryEffect.DoubleJump:

                        if (playerController != null)
                            playerController.EnableDoubleJump();

                        if (enemyController != null)
                            enemyController.EnableDoubleJump();

                    break;

                    case AccessoryEffect.FallingStars:

                        if (GetComponent<StarfallEffect>() == null)
                        {
                            StarfallEffect starfall =
                                gameObject.AddComponent<StarfallEffect>();

                            starfall.Initialize(accessory.Prefab);
                        }

                    break;

                    case AccessoryEffect.NaturesGift:

                    spellCaster.EnableNaturesGift();

                    break;
                }
            }
        }
    }
}