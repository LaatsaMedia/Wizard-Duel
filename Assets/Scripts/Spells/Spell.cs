using UnityEngine;

public enum SpellCategory
{
    Offensive,
    Defensive,
    Utility,
    Disable,
    Ultimate
}

public enum SpellMastery
{
    Apprentice,
    Adept,
    Master,
    Archmage
}

public enum SpellElement
{
    Neutral,

    Fire,
    Water,
    Earth,
    Air,

    Arcane
}

[CreateAssetMenu(menuName = "Spells/Spell")]
public class Spell : ScriptableObject
{
    [SerializeField] private SpellCategory category;
    public SpellCategory Category => category;

    [SerializeField] private SpellMastery mastery;
    public SpellMastery Mastery => mastery;

    [SerializeField]
    private SpellElement element = SpellElement.Neutral;
    public SpellElement Element => element;

    public string spellName;
    public Sprite icon;
    public string description = "This is placeholder for description";

    [Header("Stats")]
    public float cooldown;
    public float manaCost;

    [Header("Audio")]
    public AudioClip castSFX;

    [Header("Prefab")]
    public GameObject spellPrefab;

    [Header("Enemy AI - Personality")]

    [Tooltip(
        "Controls how aggressively this spell influences the enemy's playstyle.\n\n" +
        "Negative = defensive.\n" +
        "Positive = aggressive.\n\n" +
        "The AI averages this value across all equipped spells.")]
    [SerializeField]
    [Range(-5, 5)]
    private int aggression = 0;
    public int Aggression => aggression;

    [Tooltip(
        "Preferred distance from the target while this spell is equipped.\n\n" +
        "The AI averages this value across all equipped spells to determine its overall fighting distance.")]
    [SerializeField]
    [Min(0f)]
    private float preferredRange = 8f;
    public float PreferredRange => preferredRange;

    [Header("Enemy AI - Casting")]

    [Tooltip(
        "Minimum distance from the target where the AI will consider casting this spell.\n\n" +
        "If the enemy is closer than this distance, the spell will not be considered.")]
    [SerializeField]
    [Min(0f)]
    private float minimumCastRange = 0f;
    public float MinimumCastRange => minimumCastRange;

    [Tooltip(
        "Maximum distance from the target where the AI will consider casting this spell.\n\n" +
        "If the enemy is farther away than this distance, the spell will not be considered.")]
    [SerializeField]
    [Min(0f)]
    private float maximumCastRange = 10f;
    public float MaximumCastRange => maximumCastRange;

    [Tooltip(
        "Base desirability of casting this spell.\n\n" +
        "Higher values make the AI naturally prefer this spell over others.\n\n" +
        "This is only the starting score. The AI modifies it depending on the situation.")]
    [SerializeField]
    [Range(0, 100)]
    private int baseCastPriority = 50;
    public int BaseCastPriority => baseCastPriority;

    [Tooltip(
        "The AI will never consider casting this spell while its health is ABOVE this value.\n\n" +
        "Example:\n" +
        "1.0 = Always allowed\n" +
        "0.6 = Only below 60% HP\n" +
        "0.3 = Only below 30% HP")]
    [SerializeField]
    [Range(0f, 1f)]
    private float maximumCastHealth = 1f;
    public float MaximumCastHealth => maximumCastHealth;

    [Tooltip(
        "When the AI's health falls to or below this value, this spell receives a priority bonus.\n\n" +
        "Useful for defensive, healing or emergency spells.")]
    [SerializeField]
    [Range(0f, 1f)]
    private float idealCastHealth = 1f;
    public float IdealCastHealth => idealCastHealth;
}