using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.ComponentModel;

///// <summary>
///// A space for the cards to be drawn/discard during combat.
///// </summary>
public class CardPile : MonoBehaviour
{
    [SerializeField] List<Card> cards = new List<Card>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int Size() => cards.Count;

    public bool IsEmpty() => Size() <= 0;

    public void Shuffle()
    {
        cards.Shuffle();
    }

    public void Refill(Deck deck, bool needShuffle = true)
    {
        Refill(deck.Export(), needShuffle);
    }

    public void Refill(List<Card> cardList, bool needShuffle = true)
    {
        cards.Clear();

        foreach(var card in cardList)
        {
            cards.AddCard(card);
        }

        if(needShuffle)
        {
            cards.Shuffle();
        }
    }

    public void Insert(Card card, int index = -1)
    {
        if (index == -1)
        {
            index = Random.Range(0, cards.Count);
        }

        if (index >= 0 && index < cards.Count)
        {
            cards.InsertCard(card, index);
        }
        else
        {
            Push(card);
        }
    }

    public void Insert(List<Card> cardList)
    {
        foreach(var card in cardList)
        {
            int index = Random.Range(0, cards.Count);
            cards.InsertCard(card, index);
        }

    }

    public void Push(Card card)
    {
        cards.AddCard(card);
    }

    public Card Draw()
    {
        if (!IsEmpty())
            return cards.Pop();
        else
            throw new System.Exception("Draw from empty pile.");
    }

    public void Remove(Card card)
    {
        cards.Remove(card);
    }

    public List<Card> Extract()
    {
        var clone = cards.Clone();

        cards.Clear();

        return clone;
    }

    public List<Card> Preview()
    {
        return cards.OrderByDescending(x => x.rarity).ToList();
    }

    public List<Card> PreviewWithDrawSequence()
    {
        var clone = cards.Clone();
        clone.Reverse();

        return clone;
    }


    /*
     * For CardEffects! - Filipe
     */
    public List<Card> GetCardsInPile(List<CardFilter> filters = null)
    {
       if(filters == null)
        {
            return cards;
        }

       List<Card> eligibleCards = new List<Card>();
        foreach (Card card in cards)
        {
            if(filters.All(it => it.Filter(card) == 1))
            {
                eligibleCards.Add(card);
            }
        }
        return eligibleCards;
    }

    public void DrawCardFromPile(Card card)
    {
        cards.Remove(card);
    }
    /*
     * 
     */
}

//public class CardPile : MonoBehaviour
//{
//    [SerializeField] List<Card> cards = new List<Card>();
//    [SerializeField] Stack<int> drawSequences = new Stack<int>();

//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    public int Size() => cards.Count;

//    public void InitWithDeck(Deck deck)
//    {
//        var deckCards = deck.Export();
//        drawSequences = deckCards.ToShuffleIndex();

//        cards.Clear();

//        foreach(var card in deckCards)
//        {
//            cards.Add(card);
//        }
//    }

//    public void PushWithShuffle(List<Card> cardList)
//    {
//        cardList.Shuffle();

//        foreach (var card in cardList)
//        {
//            cards.Add(card);
//        }
//    }

//    public void Push(List<Card> cardList)
//    {
//        foreach (var card in cardList)
//        {
//            cards.Add(card);
//        }

//        Shuffle();
//    }

//    public void Push(Card card)
//    {
//        cards.Add(card);

//        Shuffle();
//    }

//    public Card Draw()
//    {
//        return cards.Pop();
//    }

//    public List<Card> Export()
//    {
//        return cards.ToList();
//    }

//    public List<Card> ExportWithDrawSequence()
//    {
//        var r = new List<Card>();

//        while(drawSequences.Count > 0)
//        {
//            var s = drawSequences.Pop();

//            if (s >= cards.Count) continue;

//            r.Add(cards[s]);
//        }

//        return r;
//    }

//    void Shuffle()
//    {
//        drawSequences = cards.ToShuffleIndex();
//    }
//}