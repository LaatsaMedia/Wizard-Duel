using UnityEngine;

public class SpellBarUI : MonoBehaviour
{
    [SerializeField] private Team team;

    private SpellCaster spellCaster;

    [SerializeField] private Transform content;
    [SerializeField] private SpellSlotUI spellSlotPrefab;

    private void OnEnable()
    {
        MatchManager.OnCharactersSpawned += AssignSpellCaster;
    }

    private void OnDisable()
    {
        MatchManager.OnCharactersSpawned -= AssignSpellCaster;
    }

    private void Start()
    {
        AssignSpellCaster();
    }

    private void AssignSpellCaster()
    {
        ClearSlots();

        GameObject target =
            team == Team.Player
            ? MatchManager.Instance.CurrentPlayer
            : MatchManager.Instance.CurrentEnemy;

        if (target == null)
            return;

        spellCaster = target.GetComponent<SpellCaster>();

        CreateSlot(spellCaster.PrimarySpell);
        CreateSlot(spellCaster.SecondarySpell);
        CreateSlot(spellCaster.ThirdSpell);
    }

    private void ClearSlots()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateSlot(SpellSlot slot)
    {
        if (slot.spell == null)
            return;

        SpellSlotUI ui = Instantiate(spellSlotPrefab, content);
        ui.Initialize(slot);
    }
}