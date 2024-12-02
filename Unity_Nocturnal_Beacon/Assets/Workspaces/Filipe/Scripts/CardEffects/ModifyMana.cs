using CardAttribute;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifyMana : CardEffect
{
    
    public override string LocalizationKey => "CE_DESC_ModifyMana";
    public override string EffectDescription
    {
        get
        {

            return string.Format(
                val1 > 0 ?
                    "Recover {0} mana." :
                    "Lose {0} mana."
                ,
                Math.Abs(val1));
        }

    }
    public ModifyMana()
    {
    }
    public ModifyMana(int i) {
        this.target = EffectTarget.Self;
        val1 = i;
    }


    public ModifyMana(EffectTarget target, int val1, int val2)
    {
        this.target = target;
        this.val1 = val1;
        this.val2 = val2;
    }


    public override int GetEffectCost()
    {
        int result;
        float multiplier = 1;

        if (val1 > 0)
        {
            int baseVal = 20;
            int scaling = (int) Mathf.Pow(4, 1 + val1);
            result = val1 > 0 ? (int) baseVal + scaling : 0;
            return result;
        }
        else {
            int baseVal = 5;
            int scaling = 3 * val1;
            result = val1 != 0 ? (int) baseVal + scaling : 0;
            return result;
        }
    }


    override public void OnUse(Card card, BattleUnit owner, List<BattleUnit> targets)
    {
        BattleManager.Instance.ModifyMana(val1);
    }


}
