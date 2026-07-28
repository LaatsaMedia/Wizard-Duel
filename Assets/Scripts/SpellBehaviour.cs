using System.Collections.Generic;
using UnityEngine;

public abstract class SpellBehaviour : MonoBehaviour
{
    protected GameObject caster;
    protected Spell spell;
    protected float castDirection;

    private readonly HashSet<Health> hitTargets = new();

    public GameObject Caster => caster;
    public Spell Spell => spell;

    public virtual bool SupportsRecast => false;
    public virtual bool IsGroundSpell => false;

    public virtual void Initialize(
        GameObject caster,
        Spell spell,
        float castDirection)
    {
        this.caster = caster;
        this.spell = spell;
        this.castDirection = castDirection;
    }

    protected bool RegisterHit(Health target)
    {
        return hitTargets.Add(target);
    }

    protected void ClearHitTargets()
    {
        hitTargets.Clear();
    }

    public virtual bool Recast()
    {
        return false;
    }
}