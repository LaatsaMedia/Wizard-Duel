using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }

    [SerializeField] private RewardProgression rewardProgression;
    private int currentRewardIndex;

    public int CurrentStage => currentRewardIndex + 1;

    public RewardCategory CurrentRewardCategory => rewardProgression.rewardOrder[currentRewardIndex];

    private const string SetupScene = "Setup";
    private const string CombatScene = "Combat";

    [Header("Databases")]
    [SerializeField] private SpellDatabase spellDatabase;
    [SerializeField] private AccessoryDatabase accessoryDatabase;
    
    public bool ShouldGenerateUltimateReward()
    {
        int chance = CurrentStage switch
        {
            1 => 20,
            2 => 0,
            3 => 5,
            4 => 5,
            5 => 10,
            6 => 10,
            _ => 20
        };

        return Random.Range(0, 100) < chance;
    }

    private static readonly SpellCategory[] AnySpellCategories =
    {
        SpellCategory.Offensive,
        SpellCategory.Defensive,
        SpellCategory.Utility,
        SpellCategory.Disable
    };

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

    public SpellCategory GetRewardSpellCategory()
    {
        return CurrentRewardCategory switch
        {
            RewardCategory.Offensive => SpellCategory.Offensive,
            RewardCategory.Utility => SpellCategory.Utility,
            RewardCategory.Disable => SpellCategory.Disable,
            RewardCategory.Defensive => SpellCategory.Defensive,
            _ => SpellCategory.Offensive
        };
    }

    public SpellCategory GetRandomSpellCategory()
    {
        return AnySpellCategories[
            Random.Range(0, AnySpellCategories.Length)];
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
        SpellCategory category =
            CurrentRewardCategory == RewardCategory.Any
            ? GetRandomSpellCategory()
            : GetRewardSpellCategory();

        List<Spell> rewards =
            GetRandomSpells(
                category,
                EnemyBuild,
                1);

        if (rewards.Count > 0)
        {
            EnemyBuild.AddSpell(rewards[0]);
        }
    }

    private void GenerateEnemyAccessoryReward()
    {
        List<Accessory> rewards =
            GetRandomAccessories(
                EnemyBuild,
                1);

        if (rewards.Count > 0)
        {
            EnemyBuild.AddAccessory(rewards[0]);
        }
    }

    private SpellMastery GetRewardMastery()
    {
        int stage = CurrentStage;

        int roll = Random.Range(0, 100);

        switch (stage)
        {
            case 1:
                return SpellMastery.Apprentice;

            case 2:
                return roll < 90
                    ? SpellMastery.Apprentice
                    : SpellMastery.Adept;

            case 3:
                return roll < 70
                    ? SpellMastery.Apprentice
                    : SpellMastery.Adept;

            case 4:
                return roll < 40
                    ? SpellMastery.Apprentice
                    : SpellMastery.Adept;

            case 5:
                if (roll < 20)
                    return SpellMastery.Apprentice;

                if (roll < 80)
                    return SpellMastery.Adept;

                return SpellMastery.Master;

            case 6:
                if (roll < 10)
                    return SpellMastery.Apprentice;

                if (roll < 70)
                    return SpellMastery.Adept;

                return SpellMastery.Master;

            default:
                if (roll < 10)
                    return SpellMastery.Apprentice;

                if (roll < 40)
                    return SpellMastery.Adept;

                if (roll < 80)
                    return SpellMastery.Master;

                return SpellMastery.Archmage;
        }
    }

    public List<Spell> GetUltimateSpells(
    WizardBuild build)
    {
        List<Spell> spells = new();

        foreach (Spell spell in spellDatabase.spells)
        {
            if (spell.Mastery == SpellMastery.Archmage &&
                spell != build.ultimateSpell)
            {
                spells.Add(spell);
            }
        }

        return spells;
    }

    public Spell GetRandomUltimateSpell(
    WizardBuild build)
    {
        List<Spell> spells =
            GetUltimateSpells(build);

        if (spells.Count == 0)
            return null;

        return spells[
            Random.Range(0, spells.Count)];
    }

    public List<Spell> GetSpells(
        SpellCategory category,
        SpellMastery mastery)
    {
        List<Spell> spells = new();

        foreach (Spell spell in spellDatabase.spells)
        {
            if (spell.Category == category &&
                spell.Mastery == mastery)
            {
                spells.Add(spell);
            }
        }

        return spells;
    }

    public List<Spell> GetRandomSpells(
        SpellCategory category,
        WizardBuild build,
        int amount,
        List<Spell> excludedSpells = null)
    {
        SpellMastery mastery = GetRewardMastery();

        List<Spell> available;

        do
        {
            available = GetSpells(
                category,
                mastery);

            available.RemoveAll(build.HasSpell);

            if (excludedSpells != null)
            {
                available.RemoveAll(excludedSpells.Contains);
            }

            if (available.Count > 0)
                break;

            mastery--;
        }
        while (mastery >= SpellMastery.Apprentice);

        List<Spell> selected = new();

        while (selected.Count < amount &&
            available.Count > 0)
        {
            int index = Random.Range(0, available.Count);

            selected.Add(available[index]);
            available.RemoveAt(index);
        }

        return selected;
    }

    public List<Accessory> GetRandomAccessories(
        WizardBuild build,
        int amount)
    {
        List<Accessory> available = GetAccessories();

        available.RemoveAll(build.HasAccessory);

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