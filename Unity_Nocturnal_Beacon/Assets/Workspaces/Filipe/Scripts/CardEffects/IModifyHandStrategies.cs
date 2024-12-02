
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UniRx.Triggers;
using UnityEngine;
using static CardPileManager;

interface IModifyHandStrategies
{
    public string GetString();

    public int GetCost();
    public Action<List<Card>> GetCardList(BattleManager bm);
}

[Serializable]
public class GetNextFromDeck : IModifyHandStrategies
{
    [SerializeField] public int amount = 1;
    public Action<List<Card>> GetCardList(BattleManager bm)
    {
        bm._cardManager.DrawCard(amount, bm.GetPlayerbattleUnit().GetUnitStatusData());
        return null;
    }

    public string GetString()
    {
        return String.Format("Draw {0} card{1} at the deck top.", amount, amount>1?"s":"");
    }

    public int GetCost()
    {
        if(amount > 0 )
        {
            return (int)Mathf.Pow(7, amount);
        }
        else
        {
            return Mathf.Min(30,(int)Mathf.Pow(4, amount));
        }
    }
}

[Serializable]
public class GetFromPile : IModifyHandStrategies
{
    [SerializeField] public int amount = 1;
    [SerializeField]
    [Description("Use -1 if there is no limit")] int availableToDraw = -1;
    [SerializeField]
    List<CardFilter> filters = new List<CardFilter>();
    [SerializeField]
    CardPileManager.PileType pile = CardPileManager.PileType.Draw;

    public Action<List<Card>> GetCardList(BattleManager bm)
    {
        bm._cardManager.DrawRandomFromPile(pile, amount, new List<CardFilter>(), bm.GetPlayerbattleUnit().GetUnitStatusData());
        return null;
    }

    public string GetString()
    {
        string piletype = "darw";
        switch (pile)
        {
            case CardPileManager.PileType.Draw:
                piletype = "draw pile";
                break;
            case CardPileManager.PileType.Discard:
                piletype = "discarded pile";
                break;
            case CardPileManager.PileType.Exhaust:
                piletype = "exhausted pile";
                break;
        }
        return String.Format("Draw {0} card{1} from the {2}.", amount, amount > 1 ? "s" : "", piletype);
    }

    public int GetCost()
    {
        if (amount > 0)
        {
            switch (pile)
            {
                case CardPileManager.PileType.Draw:
                    return (int)Mathf.Pow(5, amount);
                case CardPileManager.PileType.Discard:
                    return (int)Mathf.Pow(6, amount);
                case CardPileManager.PileType.Exhaust:
                    return (int)Mathf.Pow(10, amount);
            }
        }
        else
        {
            switch (pile)
            {
                case CardPileManager.PileType.Draw:
                    return Mathf.Min(30, (int)Mathf.Pow(3, amount));
                case CardPileManager.PileType.Discard:
                    return Mathf.Min(30, (int)Mathf.Pow(3, amount));
                case CardPileManager.PileType.Exhaust:
                    return Mathf.Min(30, (int)Mathf.Pow(3, amount));
            }
        }
        return 0;
    }
}

[Serializable]
public class DiscardFromHand : IModifyHandStrategies
{
    [SerializeField][Tooltip("Use -1 if you want to discard the whole hand")] public int amount = 1;
    [SerializeField]
    List<CardFilter> filters = new List<CardFilter>();

    public Action<List<Card>> GetCardList(BattleManager bm)
    {
        bm._cardManager.DiscardRandomCard(amount);
        return null;
    }

    public string GetString()
    {
        return String.Format("Discard {0} random card{1} from your hand.", amount, amount > 1 ? "s" : "");
    }

    public int GetCost()
    {
        if (amount > 0)
        {
            return Mathf.Min(30, (int)Mathf.Pow(7, amount));
        }
        else
        {
            return (int)Mathf.Pow(8, amount);
        }
    }
}

[Serializable]
public class DiscardSelectedFromHand : IModifyHandStrategies
{
    [SerializeField] public int amount = 1;
    [SerializeField]
    List<CardFilter> filters = new List<CardFilter>();

    public Action<List<Card>> GetCardList(BattleManager bm)
    {
        bm._cardManager.DrawCard(amount, bm.GetPlayerbattleUnit().GetUnitStatusData());
        return null;
    }

    public string GetString()
    {
        return String.Format("Discard {0} card{1} from your hand.", amount, amount > 1 ? "s" : "");
    }

    public int GetCost()
    {
        return -1;
    }
}


