using CardAttribute;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifyShield : CardEffect
{

    public override string LocalizationKey => "CE_DESC_ModifyShield";
    public override string EffectDescription
    {
        get
        {
            string result = String.Format("{0}{1} Block{2}{3}",
                target == EffectTarget.Self ? (val1 > 0 ? "Gain " : "Lose ") :
                    (val1 > 0 ? "Bestow " : "Corrode "),
                val1,
                target == EffectTarget.Self ? "" :
                val1 > 0 ? " to" : " of"
                ,
                target == EffectTarget.Self ? "" :
                target switch
                {
                    EffectTarget.Self => ".",
                    EffectTarget.OpponentSingle => " an enemy.",
                    EffectTarget.OpponentRandom => " a random enemy.",
                    EffectTarget.OpponentAll => " all enemies.",
                    EffectTarget.Global => " all units.",
                    EffectTarget.Both => " caster and target",
                    _ => " NO ONE. LOL"
                }
                );
            return result;
        }
    } 

    public ModifyShield(int amount)
    {
        val1 = amount;
        effectType = EffectType.GainShield;
    }

    public override int GetEffectCost()
    {
        return Mathf.Abs(val1*2);
    }

    public ModifyShield() {
        effectType = EffectType.GainShield;
    }

    override public void OnUse(Card card, BattleUnit owner, List<BattleUnit> targets)
    {
        foreach (BattleUnit target in targets)
        {
            float eleAffinity = ElementalTable.GetEffectivityMultiplier(target.GetElementalAffinity(card.element));
            int shield = val1;

            if (val1 > 0)
            {
                int finalDamage = owner.GetUnitStatusData().OnGainBlock(shield);
                int afterElemental = (int)Mathf.Floor(finalDamage * eleAffinity);
                target.GetHPData().AddShield(afterElemental);
            }
            else if (val1 < 0)
            {
                int finalDamage = owner.GetUnitStatusData().OnLoseBlock(shield);
                int afterElemental = (int)Mathf.Floor(finalDamage * eleAffinity);
                target.GetHPData().RemoveShield(afterElemental);
            }
        }
    }

    public override bool Compare(CardEffect e)
    {
        if (e is not ModifyShield) return false;
        ModifyShield other = (ModifyShield)e;
        return true;
    }
}