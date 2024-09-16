using CardAttribute;
using System;
using UnityEngine;

[Serializable]
public class ModifyHealth : CardEffect, ICardEffect
{
    

    public string LocalizationKey => "CE_DESC_ModifyHealth";
    public string EffectDescription
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




    public void OnUse(EffectTarget target)
    {
        /*
         * DealDamage/HealTarget({target}, {amount}, {user})
         * TODO: Implement
         * Deal {amount} damage/ Heal {amount} damage to the {target}
         */
    }

}
