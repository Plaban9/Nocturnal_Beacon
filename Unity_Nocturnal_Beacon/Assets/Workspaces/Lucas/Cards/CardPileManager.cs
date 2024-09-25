using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPileManager
{
    public enum PileType
    {
        Draw,
        Discard,
        Exhaust
    }

    Deck combatDeck = new Deck();

    Dictionary<PileType, CardPile> piles = new Dictionary<PileType, CardPile>();

    public CardPileManager()
    {
        
    }

    public CardPileManager(Deck deck)
    {
        Init(deck);
    }

    public void Init(Deck deck)
    {
        combatDeck = new Deck(deck);

        piles = new Dictionary<PileType, CardPile>();

        piles.Add(PileType.Draw, new CardPile());
        piles.Add(PileType.Discard, new CardPile());
        piles.Add(PileType.Exhaust, new CardPile());

        piles[PileType.Draw].Refill(deck);
    }

    #region GET
    public List<Card> PreviewDrawPile()
    {
        return piles[PileType.Draw].Preview();
    }

    public List<Card> PreviewDrawPileWithDrawSequence()
    {
        return piles[PileType.Draw].PreviewWithDrawSequence();
    }

    public List<Card> PreviewDiscardPile()
    {
        return piles[PileType.Discard].Preview();
    }

    public List<Card> PreviewExhaustPile()
    {
        return piles[PileType.Exhaust].Preview();
    }

    public int GetPileSize(PileType type) => piles[type].Size();
    #endregion

    public void AddCard(PileType type, Card card, int index = -1)
    {
        combatDeck.AddCard(card);
        piles[type].Insert(card, index);
    }

    public void AddCardToTop(PileType type, Card card)
    {
        AddCard(type, card, piles[type].Size() - 1);
    }

    public void AddCardToBottom(PileType type, Card card)
    {
        AddCard(type, card, 0);
    }

    public IEnumerator DrawCard(int amount)
    {
        for(int i=0; i<amount; i++)
        {
            if(piles[PileType.Draw].IsEmpty())
            {
                if(!piles[PileType.Discard].IsEmpty())
                {
                    RefillDrawPileByDiscardPile();
                }
                else
                {
                    Debug.LogError($"No more cards to draw!!");
                }
            }

            piles[PileType.Draw].Draw();

            yield return new WaitForSeconds(0.1f);
        }
    }

    public Card DrawCard()
    {
        if (piles[PileType.Draw].IsEmpty())
        {
            if (!piles[PileType.Discard].IsEmpty())
            {
                RefillDrawPileByDiscardPile();
            }
            else
            {
                Debug.LogError($"No more cards to draw!!");
            }
        }

        return piles[PileType.Draw].Draw();
    }

    public void DiscardCard(Card card)
    {
        piles[PileType.Discard].Push(card);
    }

    //public IEnumerator RefillDrawPileByDiscardPile()
    //{
    //    while(!piles[PileType.Discard].IsEmpty())
    //    {
    //        piles[PileType.Draw].Push(piles[PileType.Discard].Draw());

    //        /*
    //         * Some Animation?
    //         */

    //        yield return new WaitForEndOfFrame();
    //    }

    //    piles[PileType.Draw].Shuffle();

    //    yield return null;
    //}

    public void RefillDrawPileByDiscardPile()
    {
        while (!piles[PileType.Discard].IsEmpty())
        {
            piles[PileType.Draw].Push(piles[PileType.Discard].Draw());
        }

        piles[PileType.Draw].Shuffle();
    }
}
