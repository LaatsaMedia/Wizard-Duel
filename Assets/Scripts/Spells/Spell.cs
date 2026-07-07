using UnityEngine;

public enum SpellCategory
{
    Offensive,
    Defensive,
    Utility,
    Disable,
    None
}

public enum SpellMastery
{
    Apprentice,
    Adept,
    Master,
    Archmage
}

[CreateAssetMenu(menuName = "Spells/Spell")]
public class Spell : ScriptableObject
{
    [SerializeField] private SpellCategory category;
    public SpellCategory Category => category;

    [SerializeField] private SpellMastery mastery;
    public SpellMastery Mastery => mastery;
    
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
}