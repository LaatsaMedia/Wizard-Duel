using System;
using UnityEngine;

[Serializable]
public class UltimateSlot
{
    public Spell spell;
    public Spell Spell => spell;

    [HideInInspector]
    public SpellBehaviour activeRecastSpell;
}