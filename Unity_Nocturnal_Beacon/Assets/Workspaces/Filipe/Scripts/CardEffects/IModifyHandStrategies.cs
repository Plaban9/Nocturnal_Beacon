
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UniRx.Triggers;
using UnityEngine;

interface IModifyHandStrategies
{
    public string GetString();
    public Action<List<Card>> GetCardList(BattleManager bm);
}

[Serializable]
public class GetNextFromDeck : IModifyHandStrategies
{
    [SerializeField] int amount = 1;
    public Action<List<Card>> GetCardList(BattleManager bm)
    {
        bm._cardManager.DrawCard(amount, bm.GetPlayerbattleUnit().GetUnitStatusData());
        return null;
    }

    public string GetString()
    {
        return String.Format("Draw {0} card{1} at the deck top.", amount, amount>1?"s":"");
    }
}

[Serializable]
public class GetFromPile : IModifyHandStrategies
{
    [SerializeField] int amount = 1;
    [SerializeField]
    [Description("Use -1 if there is no limit")] int availableToDraw = -1;
    [SerializeField]
    CardFilter? filter = null;
    [SerializeField]
    CardPileManager.PileType pile = CardPileManager.PileType.Draw;

    public Action<List<Card>> GetCardList(BattleManager bm)
    {
        bm._cardManager.DrawCard(amount, bm.GetPlayerbattleUnit().GetUnitStatusData());
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
}

[Serializable]
public class DiscardFromHand : IModifyHandStrategies
{
    [SerializeField][Description("Use -1 if you want to discard the whole hand")] int amount = 1;
    [SerializeField]
    CardFilter? filter = null;
    public Action<List<Card>> GetCardList(BattleManager bm)
    {
        bm._cardManager.DrawCard(amount, bm.GetPlayerbattleUnit().GetUnitStatusData());
        return null;
    }

    public string GetString()
    {
        return String.Format("Discard {0} random card{1} from your hand.", amount, amount > 1 ? "s" : "");
    }
}

[Serializable]
public class DiscardSelectedFromHand : IModifyHandStrategies
{
    [SerializeField] int amount = 1;
    [SerializeField]
    CardFilter? filter = null;
    public Action<List<Card>> GetCardList(BattleManager bm)
    {
        bm._cardManager.DrawCard(amount, bm.GetPlayerbattleUnit().GetUnitStatusData());
        return null;
    }

    public string GetString()
    {
        return String.Format("Discard {0} card{1} from your hand.", amount, amount > 1 ? "s" : "");
    }
}


