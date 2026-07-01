using UnityEngine;

[System.Serializable]
public class WizardBuild
{
    public Spell primarySpell;
    public Spell secondarySpell;
    public Spell thirdSpell;

    public Accessory accessory1;
    public Accessory accessory2;
    public Accessory accessory3;
    public Accessory accessory4;
    public Accessory accessory5;
    public Accessory accessory6;

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

    public bool AddAccessory(Accessory accessory)
    {
        if (accessory1 == null)
        {
            accessory1 = accessory;
            return true;
        }

        if (accessory2 == null)
        {
            accessory2 = accessory;
            return true;
        }

        if (accessory3 == null)
        {
            accessory3 = accessory;
            return true;
        }

        if (accessory4 == null)
        {
            accessory4 = accessory;
            return true;
        }

        if (accessory5 == null)
        {
            accessory5 = accessory;
            return true;
        }

        if (accessory6 == null)
        {
            accessory6 = accessory;
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