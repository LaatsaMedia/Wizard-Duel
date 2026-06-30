using UnityEngine;

[System.Serializable]
public class SpellSlot
{
    public Spell spell;

    [HideInInspector]
    public float cooldownRemaining;

    [HideInInspector]
    public SpellBehaviour activeRecastSpell;
}