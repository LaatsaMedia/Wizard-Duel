using UnityEngine;

[RequireComponent(typeof(Mana))]
public class BuildApplier : MonoBehaviour
{
    private Health health;
    private Mana mana;
    private PlayerController playerController;
    private EnemyController enemyController;

    private void Awake()
    {
        health = GetComponent<Health>();
        mana = GetComponent<Mana>();
        playerController = GetComponent<PlayerController>();
        enemyController = GetComponent<EnemyController>();
    }

    public void ApplyBuild(WizardBuild build)
    {
        ApplyPassiveModifiers(build);
        ApplySpecialEffects(build);
    }

    private void ApplyPassiveModifiers(WizardBuild build)
    {
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
                    // TODO
                    break;

                case PassiveModifierType.WizardSize:
                    transform.localScale *= modifier.Value;
                    break;

                case PassiveModifierType.JumpHeight:
                    if (playerController != null)
                        playerController.jumpForce += modifier.Value;

                    if (enemyController != null)
                        enemyController.jumpForce += modifier.Value;

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
                }
            }
        }
    }
}