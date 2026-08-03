using UnityEngine;

public class InspectionManager : MonoBehaviour
{
    [SerializeField] private PassiveModifierLibrary passiveModifierLibrary;
    public PassiveModifierLibrary PassiveModifierLibrary => passiveModifierLibrary;

    [SerializeField] private OnHitModifierLibrary onHitModifierLibrary;
    public OnHitModifierLibrary OnHitModifierLibrary => onHitModifierLibrary;

    [SerializeField] private StatusEffectLibrary statusEffectLibrary;
    public StatusEffectLibrary StatusEffectLibrary => statusEffectLibrary;

    [SerializeField] private PassiveStatLibrary passiveStatLibrary;
    public PassiveStatLibrary PassiveStatLibrary => passiveStatLibrary;

    [Space]

    [SerializeField] private Sprite damageIcon;
    [SerializeField] private Sprite healIcon;
    [SerializeField] private Sprite shieldIcon;
    [SerializeField] private Sprite manaIcon;
    [SerializeField] private Sprite cooldownIcon;

    public Sprite DamageIcon => damageIcon;
    public Sprite HealIcon => healIcon;
    public Sprite ShieldIcon => shieldIcon;
    public Sprite ManaIcon => manaIcon;
    public Sprite CooldownIcon => cooldownIcon;

    public static InspectionManager Instance { get; private set; }

    [SerializeField] private InformationPanel panel;
    public bool IsInspecting { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Show(IInspectionProvider provider)
    {
        if (!provider.CanInspect)
            return;

        IsInspecting = true;

        panel.Display(provider.GetInspectionData());
    }

    public void Hide()
    {
        IsInspecting = false;

        panel.Clear();
    }
}