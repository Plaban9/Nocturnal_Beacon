using CardAttribute;
using System;
using System.Collections.Generic;
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
                result = $"Deal {Mathf.Abs(val1)} damage";

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


    override public void OnUse(BattleUnit owner, List<BattleUnit> targets)
    {
        foreach(BattleUnit target in targets)
        {
            if (val1 < 0)
            {
                int damage = val1;
                damage = owner.GetUnitStatusData().OnDealDamage(damage);
                target.GetHPData().DealDamage(damage);
            }
            else if (val1 > 0)
                target.GetHPData().RecoverHealth(val1);
        }
    }


}
