using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }

    [SerializeField] private RewardProgression rewardProgression;
    private int currentRewardIndex;

    public RewardCategory CurrentRewardCategory => rewardProgression.rewardOrder[currentRewardIndex];

    private const string SetupScene = "Setup";
    private const string CombatScene = "Combat";

    [Header("Databases")]
    [SerializeField] private SpellDatabase spellDatabase;
    [SerializeField] private AccessoryDatabase accessoryDatabase;
    public List<Accessory> GetAccessories()
    {
        return new List<Accessory>(accessoryDatabase.accessories);
    }

    public WizardBuild PlayerBuild { get; private set; } = new();
    public WizardBuild EnemyBuild { get; private set; } = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        LoadPreparation();
    }

    public void LoadPreparation()
    {
        SceneManager.LoadScene(SetupScene);
    }

    public void LoadCombat()
    {
        SceneManager.LoadScene(CombatScene);
    }

    public void AdvanceSetupPhase()
    {
        currentRewardIndex++;
    }

    public SpellCategory? CurrentSpellCategory
    {
        get
        {
            return CurrentRewardCategory switch
            {
                RewardCategory.Offensive => SpellCategory.Offensive,
                RewardCategory.Utility => SpellCategory.Utility,
                RewardCategory.Disable => SpellCategory.Disable,
                RewardCategory.Defensive => SpellCategory.Defensive,
                _ => null
            };
        }
    }

    public void GenerateEnemyReward()
    {
        switch (CurrentRewardCategory)
        {
            case RewardCategory.Offensive:
            case RewardCategory.Utility:
            case RewardCategory.Disable:
            case RewardCategory.Defensive:
            case RewardCategory.Any:
                GenerateEnemySpellReward();
                break;

            case RewardCategory.Accessory:
                GenerateEnemyAccessoryReward();
                break;
        }
    }

    private void GenerateEnemySpellReward()
    {
        SpellCategory? spellCategory = CurrentSpellCategory;

        if (spellCategory == null)
            return;

        List<Spell> rewards =
            GetRandomSpells(spellCategory.Value, 1);

        if (rewards.Count > 0)
        {
            EnemyBuild.AddSpell(rewards[0]);
        }
    }

    private void GenerateEnemyAccessoryReward()
    {
        List<Accessory> rewards =
            GetRandomAccessories(1);

        if (rewards.Count > 0)
        {
            EnemyBuild.AddAccessory(rewards[0]);
        }
    }

    public List<Spell> GetSpells(SpellCategory category)
    {
        List<Spell> spells = new();

        foreach (Spell spell in spellDatabase.spells)
        {
            if (spell.Category == category)
            {
                spells.Add(spell);
            }
        }

        return spells;
    }

    public List<Spell> GetRandomSpells(SpellCategory category, int amount)
    {
        List<Spell> available = GetSpells(category);
        List<Spell> selected = new();

        while (selected.Count < amount && available.Count > 0)
        {
            int index = Random.Range(0, available.Count);

            selected.Add(available[index]);
            available.RemoveAt(index);
        }

        return selected;
    }

    public List<Accessory> GetRandomAccessories(int amount)
    {
        List<Accessory> available = GetAccessories();
        List<Accessory> selected = new();

        while (selected.Count < amount && available.Count > 0)
        {
            int index = Random.Range(0, available.Count);

            selected.Add(available[index]);
            available.RemoveAt(index);
        }

        return selected;
    }

    public void MatchFinished(bool playerWon)
    {
        if (playerWon)
        {
            //Extra Reroll
        }
        else
        {
            //Lose Life
        }
        // Later:
        // - Award reroll if playerWon
        // - Remove tournament life if playerLost
        // - Check elimination
        // - Save statistics

        AdvanceSetupPhase();

        LoadPreparation();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == CombatScene)
        {
            MatchManager.Instance.StartMatch();
        }
    }
}