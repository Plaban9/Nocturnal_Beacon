using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

public class CardEffectSelectable : SelectionItem<CardEffect>
{
    [SerializeField] TextMeshProUGUI effectText;
    [SerializeField] CardEffectCost cardEffectCost;

    int cost = 0;

    public int GetCost() => cost;
    
    public override void Setup(CardEffect cardEffect)
    {
        base.Setup(cardEffect);

        cost = CardEffectCostManager.Instance.GetEffectCost(cardEffect);

        effectText.text = cardEffect.EffectDescription;
        cardEffectCost.SetCost(cost);
    }

    public void SetCost(int cost)
    {
        this.cost = cost;
    }

    public override void SetSelecting(bool set)
    {
        base.SetSelecting(set);

        if (set)
        {
            cardEffectCost.SetHideCost(false);
            UpdateInfo();
        }
    }
    public override void SetLocked(bool set)
    {
        base.SetLocked(set);
        cardEffectCost.SetHideCost(set);
    }

    public void UpdateInfo()
    {
        effectText.text = data.EffectDescription;
        cardEffectCost.SetCost(cost);
    }

}
