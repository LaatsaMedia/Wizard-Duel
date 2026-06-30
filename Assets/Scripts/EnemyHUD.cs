using UnityEngine;

public class EnemyHUD : MonoBehaviour
{
    [SerializeField] private Team team;
    private Mana mana;
    private Health health;

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
        healthBar.UpdateBar(health.CurrentHealth, health.MaxHealth);
        manaBar.UpdateBar(mana.CurrentMana, mana.MaxMana);
    }
}