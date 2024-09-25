using CardAttribute;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifyShield : CardEffect
{

    public override string LocalizationKey => "CE_DESC_ModifyShield";
    public override string EffectDescription => (val1 >= 0 ? "Gain " : "Lose ") + val1 + " Block.";

    public ModifyShield(int amount)
    {
        val1 = amount;
    }

    public ModifyShield() { }

    override public void OnUse(EffectTarget targetting, List<BattleUnit> targets)
    {
        foreach (BattleUnit target in targets)
        {
            if (val1 > 0)
                target.GetHPData().AddShield(val1);
            else if (val1 < 0)
                target.GetHPData().RemoveShield(val1);
        }
    }
}