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

    public override int GetEffectCost()
    {
        return 3 * Mathf.Abs(val1);
    }

    public ModifyShield() {
        effectType = EffectType.GainShield;
    }

    override public void OnUse(Card card, BattleUnit owner, List<BattleUnit> targets)
    {
        foreach (BattleUnit target in targets)
        {
            float eleAffinity = ElementalTable.GetElementalAffinity(card.element, target.GetUnitData().unitElement);
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
}