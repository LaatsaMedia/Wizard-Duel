using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Spell")]
public class Spell : ScriptableObject
{
    public string spellName;
    public Sprite icon;

    [Header("Stats")]
    public float cooldown;
    public float manaCost;

    [Header("Audio")]
    public AudioClip castSFX;

    [Header("Prefab")]
    public GameObject spellPrefab;
}