using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }

    [SerializeField] private PackDatabase packDatabase;

    [SerializeField] private PackDefinition currentPack;
    public PackDefinition CurrentPack => currentPack;

    [SerializeField] private int maxRounds = 10;
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    public int CurrentLives => currentLives;
    public int MaxLives => maxLives;
    public int MaxRounds => maxRounds;

    [SerializeField] private bool unlockAllSpellMasteries;

    [SerializeField] private RewardProgression rewardProgression;
    public RewardProgression RewardProgression => rewardProgression;
    [SerializeField] private int currentRewardIndex;
    public int CurrentRewardIndex => currentRewardIndex;
    public int CurrentStage => currentRewardIndex + 1;

    [Header("Enemy Ultimate")]
    [SerializeField]
    private int enemyUltimateUnlockStage = 5;

    [SerializeField]
    [Range(0f, 1f)]
    private float enemyUltimateChance = 0.5f;

    public RewardCategory CurrentRewardCategory => rewardProgression.rewardOrder[currentRewardIndex];

    private const string SetupScene = "Setup";
    private const string CombatScene = "Combat";

    [Header("Databases")]
    [SerializeField] private SpellDatabase spellDatabase;
    [SerializeField] private AccessoryDatabase accessoryDatabase;
    
    public void SetCurrentPack(PackDefinition pack)
    {
        currentPack = pack;
    }

    private PackDefinition GetEnemyPack()
    {
        switch (GameSettings.OpponentPackMode)
        {
            case OpponentPackMode.SameAsPlayer:
                return CurrentPack;

            case OpponentPackMode.RandomPack:

                if (packDatabase.packs.Length == 0)
                    return CurrentPack;

                return packDatabase.packs[
                    Random.Range(0, packDatabase.packs.Length)];

            case OpponentPackMode.AllPacks:
                return null;

            default:
                return CurrentPack;
        }
    }

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

    public List<Accessory> GetAccessories(PackDefinition pack)
    {
        List<Accessory> accessories = new();

        foreach (Accessory accessory in accessoryDatabase.accessories)
        {
            if (accessory == null)
                continue;

            if (pack != null &&
                accessory.OriginPack != pack)
                continue;

            accessories.Add(accessory);
        }

        return accessories;
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

    public void Start()
    {
        if (GameSettings.SelectedPack != null)
        {
            currentPack = GameSettings.SelectedPack;
        }

        LoadPreparation();
        StartNewRun();
    }

    public void StartNewRun()
    {
        currentRewardIndex = 0;

        currentLives = maxLives;

        PlayerBuild = new WizardBuild();
        EnemyBuild = new WizardBuild();
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

    public SpellCategory GetRewardSpellCategory(
        RewardCategory rewardCategory)
    {
        return rewardCategory switch
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

    public void GenerateEnemyBuild()
    {
        EnemyBuild = new WizardBuild();
        PackDefinition enemyPack = GetEnemyPack();

        for (int i = 0; i <= CurrentRewardIndex; i++)
        {
            RewardCategory reward = rewardProgression.rewardOrder[i];

            // First spell must always be Offensive.
            if (i == 0 &&
                reward != RewardCategory.Offensive)
            {
                GenerateEnemyStarterSpell(enemyPack);
            }
            else
            {
                GenerateEnemyReward(enemyPack, reward);
            }
        }

        // Chance for the enemy to receive a random ultimate.
        if (CurrentStage >= enemyUltimateUnlockStage &&
            EnemyBuild.ultimateSpell == null &&
            Random.value < enemyUltimateChance)
        {
            GenerateEnemyUltimateReward(enemyPack);
        }
    }

    private void GenerateEnemyStarterSpell(
        PackDefinition pack)
    {
        List<Spell> spells = GetSpells(
            pack,
            SpellCategory.Offensive,
            SpellMastery.Apprentice);

        if (spells.Count == 0)
            return;

        Spell spell = spells[
            Random.Range(0, spells.Count)];

        EnemyBuild.AddSpell(spell);
    }

    private void GenerateEnemyReward(
        PackDefinition pack,
        RewardCategory reward)
    {
        switch (reward)
        {
            case RewardCategory.Offensive:
            case RewardCategory.Utility:
            case RewardCategory.Disable:
            case RewardCategory.Defensive:
            case RewardCategory.Any:
                GenerateEnemySpellReward(pack, reward);
                break;

            case RewardCategory.Accessory:
                GenerateEnemyAccessoryReward(pack);
                break;

            case RewardCategory.Ultimate:
                GenerateEnemyUltimateReward(pack);
                break;
        }
    }

    private void GenerateEnemySpellReward(
        PackDefinition pack,
        RewardCategory rewardCategory)
    {
        // First spell is always a random Apprentice Offensive spell.
        if (EnemyBuild.SpellCount == 0)
        {
        List<Spell> starters = GetSpells(
            pack,
            SpellCategory.Offensive,
            SpellMastery.Apprentice);

            if (starters.Count > 0)
            {
                EnemyBuild.AddSpell(
                    starters[Random.Range(0, starters.Count)]);

                return;
            }
        }

        SpellCategory category =
            rewardCategory == RewardCategory.Any
            ? GetRandomSpellCategory()
            : GetRewardSpellCategory(rewardCategory);

        List<Spell> rewards =
            GetRandomSpells(
                pack,
                category,
                EnemyBuild,
                1,
                GetRewardMastery());

        if (rewards.Count > 0)
            EnemyBuild.AddSpell(rewards[0]);
    }

    private void GenerateEnemyAccessoryReward(PackDefinition pack)
    {
        List<Accessory> rewards =
            GetRandomAccessories(
                pack,
                EnemyBuild,
                1);

        if (rewards.Count > 0)
        {
            EnemyBuild.AddAccessory(rewards[0]);
        }
    }

    private void GenerateEnemyUltimateReward(PackDefinition pack)
    {
        Spell spell =
            GetRandomUltimateSpell(
                pack,
                EnemyBuild);

        if (spell != null)
        {
            EnemyBuild.SetUltimateSpell(spell);
        }
    }

    public SpellMastery GetRewardMastery()
    {
        if (unlockAllSpellMasteries)
            return SpellMastery.Master;

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
        return GetUltimateSpells(
            CurrentPack,
            build);
    }

    public List<Spell> GetUltimateSpells(
        PackDefinition pack,
        WizardBuild build)
    {
        List<Spell> spells = new();

        foreach (Spell spell in spellDatabase.spells)
        {
            if (spell == null)
                continue;

            if (pack != null &&
                spell.OriginPack != pack)
                continue;

            if (spell.Mastery != SpellMastery.Archmage)
                continue;

            if (spell == build.ultimateSpell)
                continue;

            spells.Add(spell);
        }

        return spells;
    }

    public Spell GetRandomUltimateSpell(
    WizardBuild build)
    {
        return GetRandomUltimateSpell(
            CurrentPack,
            build);
    }

    public Spell GetRandomUltimateSpell(
    PackDefinition pack,
    WizardBuild build)
    {
        List<Spell> spells =
            GetUltimateSpells(
                pack,
                build);

        if (spells.Count == 0)
            return null;

        return spells[
            Random.Range(0, spells.Count)];
    }

    public List<Spell> GetSpells(
    PackDefinition pack,
    SpellCategory category,
    SpellMastery mastery)
    {
        List<Spell> spells = new();

        foreach (Spell spell in spellDatabase.spells)
        {
            if (spell == null)
                continue;

            if (pack != null &&
                spell.OriginPack != pack)
                continue;

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
        return GetRandomSpells(
            CurrentPack,
            category,
            build,
            amount,
            GetRewardMastery(),
            excludedSpells);
    }

    public List<Spell> GetRandomSpells(
    SpellCategory category,
    WizardBuild build,
    int amount,
    SpellMastery mastery,
    List<Spell> excludedSpells = null)
    {
        return GetRandomSpells(
            CurrentPack,
            category,
            build,
            amount,
            mastery,
            excludedSpells);
    }

    public List<Spell> GetRandomSpells(
    PackDefinition pack,
    SpellCategory category,
    WizardBuild build,
    int amount,
    SpellMastery mastery,
    List<Spell> excludedSpells = null)
    {
        List<Spell> available = new();

        SpellMastery currentMastery = mastery;

        while (currentMastery >= SpellMastery.Apprentice)
        {
            List<Spell> spells =
                GetSpells(pack, category, currentMastery);

            spells.RemoveAll(build.HasSpell);

            if (excludedSpells != null)
                spells.RemoveAll(excludedSpells.Contains);

            available.AddRange(spells);

            currentMastery--;
        }

        for (int i = 0; i < available.Count; i++)
        {
            int j = Random.Range(i, available.Count);
            (available[i], available[j]) =
                (available[j], available[i]);
        }

        if (available.Count > amount)
            available.RemoveRange(amount, available.Count - amount);

        return available;
    }

    public List<Accessory> GetRandomAccessories(
    WizardBuild build,
    int amount,
    List<Accessory> excludedAccessories = null)
    {
        return GetRandomAccessories(
            CurrentPack,
            build,
            amount,
            excludedAccessories);
    }

    public List<Accessory> GetRandomAccessories(
        PackDefinition pack,
        WizardBuild build,
        int amount,
        List<Accessory> excludedAccessories = null)
    {
        List<Accessory> available = GetAccessories(pack);

        available.RemoveAll(build.HasAccessory);

        if (excludedAccessories != null)
            available.RemoveAll(excludedAccessories.Contains);

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
            // extra reroll
        }
        else
        {
            LoseLife();
            
            if (CurrentLives <= 0)
            {
                // TODO:
                // Game Over
                return;
            }
        }
        // Later:
        // - Award reroll if playerWon
        // - Remove tournament life if playerLost
        // - Check elimination
        // - Save statistics

        AdvanceSetupPhase();

        LoadPreparation();
    }

    public void LoseLife()
    {
        currentLives = Mathf.Max(0, currentLives - 1);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == CombatScene)
        {
            MatchManager.Instance.StartMatch();
        }
    }
}