using UnityEngine;

public abstract class SpellBehaviour : MonoBehaviour
{
    protected GameObject caster;
    
    public virtual bool SupportsRecast => false;
    public virtual bool IsGroundSpell => false;
    protected float castDirection;

    public virtual void Initialize(GameObject caster, float castDirection)
    {
        this.caster = caster;
        this.castDirection = castDirection;
    }

    public virtual bool Recast()
    {
        return false;
    }
}