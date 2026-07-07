using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private Team team;
    private Health health;
    private Mana mana;
    private UltimateCharge ultimateCharge;

    [SerializeField] private StatusBarUI healthBar;
    [SerializeField] private StatusBarUI manaBar;
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

        healthBar.UpdateBar(health.CurrentHealth, health.MaxHealth);
        manaBar.UpdateBar(mana.CurrentMana, mana.MaxMana);

        ultimateBar.UpdateBar(ultimateCharge.CurrentCharge, 100f);
    
        /*if (ultimateBar.gameObject.activeSelf)
        {
            ultimateBar.UpdateBar(
                ultimateCharge.CurrentCharge,
                100f);

            ultimateReady.SetActive(
                ultimateCharge.IsReady);
        }*/
    }
}