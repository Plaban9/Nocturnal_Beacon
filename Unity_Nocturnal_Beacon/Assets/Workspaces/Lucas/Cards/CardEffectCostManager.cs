using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CardEffectCostManager : MonoBehaviour
{
    private static CardEffectCostManager instance;

    public static CardEffectCostManager Instance => instance;

    public Card cardSO { get; private set; }
    public CardManaSetting cardManaSetting { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCard(Card c)
    {
        cardSO = c;
    }

    public void SetCardManaSetting(CardManaSetting cms)
    {
        cardManaSetting = cms;
    }

    public int GetManaCost(int mana)
    {
        if (cardManaSetting == null) return 0;

        if (cardSO == null) return cardManaSetting.GetCardManaCost(mana);

        var cost = 0;
        var curCost = cardManaSetting.GetCardManaCost(cardSO.manaCost);

        if(mana == cardSO.manaCost)
        {
            cost = 0;
        }
        else
        {
            cost = cardManaSetting.GetCardManaCost(mana) - curCost;
        }

        return cost;
    }

    public int GetEffectCost(CardEffect ce)
    {
        var cost = 0;

        switch (ce.GetEffectType())
        {
            case CardAttribute.EffectType.DrawCard:
                cost = GetEffectCostDrawCard(ce);
                break;
            case CardAttribute.EffectType.DiscardCard:
                cost = GetEffectCostDiscardCard(ce);
                break;
            case CardAttribute.EffectType.DealDamage:
                cost = GetEffectCostDealDamage(ce);
                break;
            case CardAttribute.EffectType.GainHealth:
                cost = GetEffectCostGainHealth(ce);
                break;
            case CardAttribute.EffectType.GainShield:
                cost = GetEffectCostGainShield(ce);
                break;
            case CardAttribute.EffectType.GainStatusStrength:
            case CardAttribute.EffectType.GainStatusDexterity:
            case CardAttribute.EffectType.GainStatusRegenerate:
                cost = GetEffectCostGainStatus(ce);
                break;
                
        }

        return cost;
    }

    int GetEffectCostDrawCard(CardEffect ce)
    {
        int cost;
        var oriEffect = GetCardEffectInCard(ce);
        var curVal = Mathf.Abs(ce.GetMainValue());
        var oriVal = oriEffect == null ? 0 : Mathf.Abs(oriEffect.GetMainValue());
        var diff = curVal - oriVal;
        float multiplier = 3;

        cost = (int)(multiplier + curVal) * Mathf.Abs(diff);

        return cost;
    }

    int GetEffectCostDiscardCard(CardEffect ce)
    {
        int cost;
        var oriEffect = GetCardEffectInCard(ce);
        var curVal = Mathf.Abs(ce.GetMainValue());
        var oriVal = oriEffect == null ? 0 : Mathf.Abs(oriEffect.GetMainValue());
        var diff = curVal - oriVal;
        float multiplier = 1;

        cost = (int)-(multiplier + curVal) * Mathf.Abs(diff);

        return cost;
    }

    int GetEffectCostDealDamage(CardEffect ce)
    {
        int cost;
        var oriEffect = GetCardEffectInCard(ce);
        var curVal = Mathf.Abs(ce.GetMainValue());
        var oriVal = oriEffect == null ? 0 : Mathf.Abs(oriEffect.GetMainValue());
        var diff = curVal - oriVal;
        float multiplier = 1;

        switch(ce.GetTarget())
        {
            case CardAttribute.EffectTarget.Self:
                multiplier = Mathf.Min(-1, -(curVal / 10));
                break;
            case CardAttribute.EffectTarget.OpponentSingle:
                multiplier = Mathf.Max(1, curVal / 20);
                break;
            case CardAttribute.EffectTarget.OpponentRandom:
                multiplier = Mathf.Max(1, curVal / 10);
                multiplier *= 0.5f;
                break;
            case CardAttribute.EffectTarget.OpponentAll:
                multiplier = 1.5f;
                break;
        }

        cost = (int)(multiplier * diff);
        
        return cost;
    }

    int GetEffectCostGainHealth(CardEffect ce)
    {
        int cost;
        var oriEffect = GetCardEffectInCard(ce);
        var curVal = Mathf.Abs(ce.GetMainValue());
        var oriVal = oriEffect == null ? 0 : Mathf.Abs(oriEffect.GetMainValue());
        var diff = curVal - oriVal;
        float multiplier = 1;

        cost = (int)(multiplier * Mathf.Abs(diff));

        return cost;
    }

    int GetEffectCostGainShield(CardEffect ce)
    {
        int cost;
        var oriEffect = GetCardEffectInCard(ce);
        var curVal = Mathf.Abs(ce.GetMainValue());
        var oriVal = oriEffect == null ? 0 : Mathf.Abs(oriEffect.GetMainValue());
        var diff = curVal - oriVal;
        float multiplier = 1;

        cost = (int)(multiplier * Mathf.Abs(diff));

        return cost;
    }

    int GetEffectCostGainStatus(CardEffect ce)
    {
        int cost;
        var oriEffect = GetCardEffectInCard(ce);
        var curVal = Mathf.Abs(ce.GetMainValue());
        var oriVal = oriEffect == null ? 0 : Mathf.Abs(oriEffect.GetMainValue());
        var diff = curVal - oriVal;
        float multiplier = 5;

        cost = (int)(multiplier + curVal) * Mathf.Abs(diff);

        return cost;
    }

    CardEffect GetCardEffectInCard(CardEffect ce)
    {
        if(cardSO != null)
        {
            return cardSO.effects.FirstOrDefault(x => x.Compare(ce));
        }

        return null;
    }
}
