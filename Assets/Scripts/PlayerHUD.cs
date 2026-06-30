using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private Team team;
    private Health health;
    private Mana mana;

    [SerializeField] private StatusBarUI healthBar;
    [SerializeField] private StatusBarUI manaBar;

    private void OnEnable()
    {
        MatchManager.OnCharactersSpawned += AssignTarget;
    }

    private void OnDisable()
    {
        MatchManager.OnCharactersSpawned -= AssignTarget;
    }

    private void Start()
    {
        AssignTarget();
    }

    private void AssignTarget()
    {
        GameObject target =
            team == Team.Player
            ? MatchManager.Instance.CurrentPlayer
            : MatchManager.Instance.CurrentEnemy;

        if (target == null)
            return;

        health = target.GetComponent<Health>();
        mana = target.GetComponent<Mana>();

        if (target.TryGetComponent(out HitController hitController))
        {
            hitController.SetHealthBarShake(
                healthBar.GetComponent<UIShakeEffect>());
        }
    }

    private void Update()
    {
        if (health == null)
        {
            AssignTarget();
            return;
        }

        healthBar.UpdateBar(health.CurrentHealth, health.MaxHealth);
        manaBar.UpdateBar(mana.CurrentMana, mana.MaxMana);
    }
}