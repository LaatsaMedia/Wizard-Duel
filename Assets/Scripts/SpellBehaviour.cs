using UnityEngine;

public abstract class SpellBehaviour : MonoBehaviour
{
    protected GameObject caster;
    public virtual bool SupportsRecast => false;

    public virtual void Initialize(GameObject caster)
    {
        this.caster = caster;
    }

    public virtual bool Recast()
    {
        return false;
    }
}