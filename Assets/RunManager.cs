using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }

    private const string SetupScene = "Setup";
    private const string CombatScene = "Combat";

    [Header("Databases")]
    [SerializeField] private SpellDatabase spellDatabase;

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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == CombatScene)
        {
            MatchManager.Instance.StartMatch();
        }
    }
}