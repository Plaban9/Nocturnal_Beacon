using CardAttribute;
using System;
using UnityEngine;

[Serializable]
public class ModifyHealth : CardEffect
{
    
    public override string LocalizationKey => "CE_DESC_ModifyHealth";
    public override string EffectDescription
    {
        get
        {
            string result = string.Empty;

            if(target == EffectTarget.Self)
            {
                result = (val1 >= 0 ? "Heal " : "Lose ") + $"{val1} HP";
            }
            else
            {
                result = $"Deal {val1} damage";

                switch (target)
                {
                    case EffectTarget.OpponentSingle:
                        break;
                    case EffectTarget.OpponentAll:
                        result += " to ALL enemies";
                        break;
                    case EffectTarget.OpponentRandom:
                        result += " to a random enemy";
                        break;
                }

                if (val2 > 0)
                {
                    result += $" {val2} times";
                }
                else if (val2 == -1)
                {
                    result += "X times";
                }
            }
            return result + ".";
        }
    }
    public ModifyHealth() { }


    public ModifyHealth(EffectTarget target, int val1, int val2)
    {
        this.val1 = val1;
        this.val2 = val2;
    }



    public override int GetEffectCost()
    {
        int result;
        float multiplier = 1;

        switch (target)
        {
            case EffectTarget.OpponentSingle:
                multiplier = 2;
                break;
            case EffectTarget.OpponentAll:
                multiplier = 4;
                break;
            case EffectTarget.OpponentRandom:
                multiplier = 1;
                break;
        }

        if (val2 > 0)
        {
            multiplier += val2;
        }
        else if (val2 == -1)
        {
            multiplier += 2;
        }

        result = (int)multiplier * val1;

        return result;
    }


    public void OnUse(EffectTarget target)
    {
        /*
         * DealDamage/HealTarget({target}, {amount}, {user})
         * TODO: Implement
         * Deal {amount} damage/ Heal {amount} damage to the {target}
         */
    }

}
