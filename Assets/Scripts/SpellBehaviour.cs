using UnityEngine;

public abstract class SpellBehaviour : MonoBehaviour
{
    protected GameObject caster;
    protected Spell spell;
    protected float castDirection;

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

    public virtual bool Recast()
    {
        return false;
    }
}