using CardAttribute;
using System;
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

    public void OnUse(EffectTarget target)
    {
        /*
         * target.AddShield(amount)
         */
    }
}