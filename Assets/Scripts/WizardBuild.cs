using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WizardBuild
{
    public Spell primarySpell;
    public Spell secondarySpell;
    public Spell thirdSpell;
    public Spell ultimateSpell;

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

    public void SetSpell(BuildSlot slot, Spell spell)
    {
        switch (slot)
        {
            case BuildSlot.Primary:
                primarySpell = spell;
                break;

            case BuildSlot.Secondary:
                secondarySpell = spell;
                break;

            case BuildSlot.Third:
                thirdSpell = spell;
                break;

            case BuildSlot.Ultimate:
                ultimateSpell = spell;
                break;
        }
    }

    public bool HasUltimateSpell()
    {
        return ultimateSpell != null;
    }

    public bool SetUltimateSpell(Spell spell)
    {
        if (spell == null)
            return false;

        ultimateSpell = spell;
        return true;
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

    public bool HasSpell(Spell spell)
    {
        return primarySpell == spell ||
            secondarySpell == spell ||
            thirdSpell == spell;
    }

    public bool HasAccessory(Accessory accessory)
    {
        return accessory1 == accessory ||
            accessory2 == accessory ||
            accessory3 == accessory ||
            accessory4 == accessory ||
            accessory5 == accessory ||
            accessory6 == accessory;
    }

    public bool HasSpecialEffect(AccessoryEffect effect)
    {
        Accessory[] accessories =
        {
            accessory1,
            accessory2,
            accessory3,
            accessory4,
            accessory5,
            accessory6
        };

        foreach (Accessory accessory in accessories)
        {
            if (accessory == null)
                continue;

            foreach (AccessoryEffect specialEffect in accessory.SpecialEffects)
            {
                if (specialEffect == effect)
                    return true;
            }
        }

        return false;
    }

    public List<OnHitModifier> GetOnHitModifiersForSpell(
    Spell spell)
    {
        List<OnHitModifier> modifiers = new();

        Accessory[] accessories =
        {
            accessory1,
            accessory2,
            accessory3,
            accessory4,
            accessory5,
            accessory6
        };

        foreach (Accessory accessory in accessories)
        {
            if (accessory == null)
                continue;

            foreach (OnHitModifier modifier in accessory.OnHitModifiers)
            {
                if (modifier.Element == SpellElement.Any ||
                    modifier.Element == spell.Element)
                {
                    modifiers.Add(modifier);
                }
            }
        }

        return modifiers;
    }

    public List<PassiveModifier> GetPassiveModifiers()
    {
        List<PassiveModifier> modifiers = new();

        Accessory[] accessories =
        {
            accessory1,
            accessory2,
            accessory3,
            accessory4,
            accessory5,
            accessory6
        };

        foreach (Accessory accessory in accessories)
        {
            if (accessory == null)
                continue;

            modifiers.AddRange(
                accessory.PassiveModifiers);
        }

        return modifiers;
    }

    public WizardBuild Clone()
    {
        return new WizardBuild
        {
            primarySpell = primarySpell,
            secondarySpell = secondarySpell,
            thirdSpell = thirdSpell,
            ultimateSpell = ultimateSpell,

            accessory1 = accessory1,
            accessory2 = accessory2,
            accessory3 = accessory3,
            accessory4 = accessory4,
            accessory5 = accessory5,
            accessory6 = accessory6
        };
    }
    public void RemoveSpell(Spell spell)
    {
        if (primarySpell == spell)
            primarySpell = null;

        if (secondarySpell == spell)
            secondarySpell = null;

        if (thirdSpell == spell)
            thirdSpell = null;

        if (ultimateSpell == spell)
            ultimateSpell = null;
    }
}