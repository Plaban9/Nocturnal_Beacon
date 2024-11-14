using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

public class ManaCostSelectable : SelectionItem<CardManaCost>
{
    [SerializeField] TextMeshProUGUI manaText;
    [SerializeField] CardEffectCost cardEffectCost;

    int cost = 0;
    int mana = 0;

    public int GetCost() => cost;
    public int GetMana() => mana;

    public override void Setup(CardManaCost cardManaCost)
    {
        base.Setup(cardManaCost);

        mana = cardManaCost.mana;
        cost = CardEffectCostManager.Instance.GetManaCost(mana);

        manaText.text = mana >= 0 ? mana.ToString() : "X";
        cardEffectCost.SetCost(cost);
    }

}
