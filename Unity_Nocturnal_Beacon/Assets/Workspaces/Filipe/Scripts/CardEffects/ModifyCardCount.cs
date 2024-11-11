using CardAttribute;
using UnityEngine;
using System;

[Serializable]
public class ModifyCardCount : CardEffect
{
    [SerializeField] EffectTargetAmount targetAmount = EffectTargetAmount.Designated;
    [SerializeField] CardType? specificType;

    public override string LocalizationKey => "CE_DESC_ModifyCardCount";
    public override string EffectDescription
    {
        get
        {
            string result = string.Empty;

            result += val1 >= 0 ? "Draw " : "Discard ";
            result += val1;
            result += specificType == null ? "" : specificType.ToString();
            result += " card" + (Mathf.Abs(val1) > 1 ? "s " : " ");
            result += targetAmount == EffectTargetAmount.Random ? "at random." : ".";
            return result;
        }
    }

    public override int GetEffectCost()
    {
        return (6 + val1) * val1;
    }

    public ModifyCardCount(int amount)
    {
        val1 = amount;
    }

    public ModifyCardCount() { }

    public void OnUse(EffectTarget target)
    {
        /* owner.Draw/Discard(amount);1
         * {owner} draws/discards {amount} cards
         */
    }
}
