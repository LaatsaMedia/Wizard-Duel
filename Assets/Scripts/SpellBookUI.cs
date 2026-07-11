using UnityEngine;

public class SpellbookUI : MonoBehaviour
{
    public static SpellbookUI Instance { get; private set; }
    [SerializeField] private SpellBookEntry primary;
    [SerializeField] private SpellBookEntry secondary;
    [SerializeField] private SpellBookEntry third;
    [SerializeField] private SpellBookEntry ultimate;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Refresh(RunManager.Instance.PlayerBuild);
    }

    public void Refresh(WizardBuild build)
    {
        primary.Setup(build.primarySpell);
        secondary.Setup(build.secondarySpell);
        third.Setup(build.thirdSpell);
        ultimate.Setup(build.ultimateSpell);
    }

    public void SetSelectable(bool selectable)
    {
        primary.SetSelectable(selectable);
        secondary.SetSelectable(selectable);
        third.SetSelectable(selectable);
        ultimate.SetSelectable(selectable);
    }
}