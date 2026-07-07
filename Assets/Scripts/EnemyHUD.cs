using UnityEngine;

public class EnemyHUD : MonoBehaviour
{
    [SerializeField] private Team team;
    private Mana mana;
    private Health health;

    [SerializeField] private StatusBarUI healthBar;
    [SerializeField] private StatusBarUI manaBar;
    private UltimateCharge ultimateCharge;

    [SerializeField] private StatusBarUI ultimateBar;
    [SerializeField] private GameObject ultimateReady;
    
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
        ultimateCharge = target.GetComponent<UltimateCharge>();

        SpellCaster spellCaster = target.GetComponent<SpellCaster>();

        ultimateBar.gameObject.SetActive(
            spellCaster.Build.HasUltimateSpell());

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

        healthBar.UpdateBar(
            health.CurrentHealth,
            health.MaxHealth);

        manaBar.UpdateBar(
            mana.CurrentMana,
            mana.MaxMana);

        if (ultimateBar.gameObject.activeSelf)
        {
            ultimateBar.UpdateBar(
                ultimateCharge.CurrentCharge,
                100f);

            ultimateReady.SetActive(
                ultimateCharge.IsReady);
        }
        /*
        if (ultimateBar.gameObject.activeSelf)
        {
            ultimateBar.UpdateBar(
                ultimateCharge.CurrentCharge,
                100f);

            ultimateReady.SetActive(
                ultimateCharge.IsReady);
        }*/
    }
}