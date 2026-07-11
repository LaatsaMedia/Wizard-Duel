using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private Team team;

    private Health health;
    private Mana mana;
    private UltimateCharge ultimateCharge;

    [SerializeField] private StatusBarUI healthBar;
    [SerializeField] private StatusBarUI manaBar;
    [SerializeField] private StatusBarUI shieldBar;
    [SerializeField] private TMP_Text shieldText;
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
        shieldBar.gameObject.SetActive(false);
        shieldText.gameObject.SetActive(false);
        
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

        Barrier barrier = health.GetComponent<Barrier>();

        float shield = 0f;

        if (barrier != null)
        {
            shieldBar.gameObject.SetActive(true);

            shieldBar.UpdateBar(
                barrier.CurrentBarrierHealth,
                barrier.MaxBarrierHealth);

            shieldText.gameObject.SetActive(true);
            shieldText.text = Mathf.CeilToInt(
                barrier.CurrentBarrierHealth).ToString();
        }
        else
        {
            shieldBar.gameObject.SetActive(false);
            shieldText.gameObject.SetActive(false);
        }

        healthBar.UpdateBar(
            health.CurrentHealth,
            health.MaxHealth,
            shield);

        manaBar.UpdateBar(
            mana.CurrentMana,
            mana.MaxMana);

        ultimateBar.UpdateBar(
            ultimateCharge.CurrentCharge,
            100f);

        /*
        if (ultimateBar.gameObject.activeSelf)
        {
            ultimateReady.SetActive(
                ultimateCharge.IsReady);
        }
        */
    }
}