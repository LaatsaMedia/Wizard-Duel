using UnityEngine;

public class InspectionManager : MonoBehaviour
{
    [SerializeField] private PassiveModifierLibrary passiveModifierLibrary;

    public PassiveModifierLibrary PassiveModifierLibrary => passiveModifierLibrary;

    [SerializeField] private OnHitModifierLibrary onHitModifierLibrary;

    public OnHitModifierLibrary OnHitModifierLibrary => onHitModifierLibrary;
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