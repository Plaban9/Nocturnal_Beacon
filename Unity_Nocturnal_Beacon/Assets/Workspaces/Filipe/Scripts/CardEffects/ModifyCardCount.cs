using CardAttribute;
using UnityEngine;
using System;

[Serializable]
public class ModifyCardCount : CardEffect
{
    [SerializeField] CardType? specificType;

    public override string LocalizationKey => "CE_DESC_ModifyCardCount";
    public override string EffectDescription
    {
        get
        {
            string result = string.Empty;

            result += val1 >= 0 ? "Draw " : "Discard ";
            result += Mathf.Abs(val1);
            result += specificType == null ? "" : specificType.ToString();
            result += " card" + (Mathf.Abs(val1) > 1 ? "s" : "");
            result += targetAmount == EffectTargetAmount.Random ? "at random." : ".";
            return result;
        }
    }

    public override int GetEffectCost()
    {
        return (6 + val1) * Mathf.Abs(val1);
    }

    public ModifyCardCount(int amount)
    {
        effectType = amount > 0 ? EffectType.DrawCard : EffectType.DiscardCard;
        targetAmount = EffectTargetAmount.Designated; 
        val1 = amount;
    }

    public void OnUse(EffectTarget target)
    {
        /* owner.Draw/Discard(amount);1
         * {owner} draws/discards {amount} cards
         */
    }
}
