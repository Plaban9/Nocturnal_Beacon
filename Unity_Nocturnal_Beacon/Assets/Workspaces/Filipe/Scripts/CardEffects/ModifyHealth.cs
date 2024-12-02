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
                result = (val1 >= 0 ? "Heal " : "Lose ") + $"{Mathf.Abs(val1)} HP";
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
        this.target = target;
        this.val1 = val1;
        this.val2 = val2;

        if (val1 > 0)
            effectType = EffectType.GainHealth;
        else
            effectType = EffectType.DealDamage;
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

        result = (int)multiplier * Mathf.Abs(val1);

        return result;
    }


    override public void OnUse(Card card, BattleUnit owner, List<BattleUnit> targets)
    {
        Debug.Log($"Attacking {val2} times");

        for (int i = 0; i < val2 + 1; i++)
        {
            if (this.target == EffectTarget.OpponentRandom)
            {
                BattleUnit target = targets.GetRandom();
                ElementalEffectivity eleEffectivity = target.GetElementalAffinity(card.element);
                float multiplier = ElementalTable.GetEffectivityMultiplier(eleEffectivity);
                if (val1 < 0)
                {
                    int damage = val1;
                    int finalDamage = owner.GetUnitStatusData().OnDealDamage(damage);
                    int afterElemental = (int)Mathf.Floor(finalDamage * multiplier);
                    target.GetHPData().DealDamage(owner, afterElemental, false, i * 0.2f);
                }
                else if (val1 > 0)
                {
                    int damage = val1;
                    int finalDamage = owner.GetUnitStatusData().OnDealDamage(damage);
                    int afterElemental = (int)Mathf.Floor(finalDamage * multiplier);
                    target.GetHPData().RecoverHealth(afterElemental);
                }
            }
            else {
                foreach (BattleUnit target in targets)
                {
                    ElementalEffectivity eleEffectivity = target.GetElementalAffinity(card.element);
                    float multiplier = ElementalTable.GetEffectivityMultiplier(eleEffectivity);
                    if (val1 < 0)
                    {
                        int damage = val1;
                        int finalDamage = owner.GetUnitStatusData().OnDealDamage(damage);
                        int afterElemental = (int)Mathf.Floor(finalDamage * multiplier);
                        target.GetHPData().DealDamage(owner, afterElemental, false, i * 0.2f);
                    }
                    else if (val1 > 0)
                    {
                        int damage = val1;
                        int finalDamage = owner.GetUnitStatusData().OnDealDamage(damage);
                        int afterElemental = (int)Mathf.Floor(finalDamage * multiplier);
                        target.GetHPData().RecoverHealth(afterElemental);
                    }
                }
            }
        }
        
    }


}
