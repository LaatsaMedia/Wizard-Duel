using UnityEngine;

[System.Serializable]
public class WizardBuild
{
    public Spell primarySpell;
    public Spell secondarySpell;
    public Spell thirdSpell;

    public bool AddSpell(Spell spell)
    {
        if (primarySpell == null)
        {
            primarySpell = spell;
            return true;
        }

        if (secondarySpell == null)
        {
            secondarySpell = spell;
            return true;
        }

        if (thirdSpell == null)
        {
            thirdSpell = spell;
            return true;
        }

        return false;
    }

    public int SpellCount
    {
        get
        {
            int count = 0;

            if (primarySpell != null) count++;
            if (secondarySpell != null) count++;
            if (thirdSpell != null) count++;

            return count;
        }
    }
}