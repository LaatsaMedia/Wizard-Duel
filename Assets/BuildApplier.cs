using System.Runtime.CompilerServices;
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
        ApplyAccessory(build.accessory1);
        ApplyAccessory(build.accessory2);
        ApplyAccessory(build.accessory3);
        ApplyAccessory(build.accessory4);
        ApplyAccessory(build.accessory5);
        ApplyAccessory(build.accessory6);
    }

    private void ApplyAccessory(Accessory accessory)
    {
        if (accessory == null)
            return;

        foreach (AccessoryModifier modifier in accessory.Modifiers)
        {
            switch (modifier.Effect)
            {
                case AccessoryEffect.ManaRegen:
                    mana.manaRegeneration += modifier.Value;
                    break;

                case AccessoryEffect.MaxMana:
                    // Later
                    break;

                case AccessoryEffect.MaxHealth:
                    health.maxHealth += modifier.Value;
                    health.currentHealth = health.maxHealth;
                    break;

                case AccessoryEffect.MoveSpeed:
                    // Later
                    break;

                case AccessoryEffect.SpellDamage:
                    // Later
                    break;

                case AccessoryEffect.CooldownRecovery:
                    // Later
                    break;
                case AccessoryEffect.DoubleJump:
                    if(playerController != null)
                        playerController.EnableDoubleJump();
                    if(enemyController != null)
                        enemyController.EnableDoubleJump();
                    break;
            }
        }
    }
}