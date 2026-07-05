using UnityEngine;

[CreateAssetMenu(menuName = "Game/Spell Database")]
public class SpellDatabase : ScriptableObject
{
    public Spell[] spells;
}