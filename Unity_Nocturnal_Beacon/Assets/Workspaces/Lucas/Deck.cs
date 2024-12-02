using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable, CreateAssetMenu(fileName = "New Deck", menuName = "Deck")]
public class Deck : ScriptableObject
{
    [SerializeField] List<CardJson> cardJsons = new();

    [SerializeField, SerializeReference] List<Card> cards = new();

    public Dictionary<int, Card> CardByUID { get; private set; }

    static readonly int InitCounter = 1000000;
    public int Counter { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Deck ToJson()
    {
        this.cardJsons = cards.Select(x => new CardJson(x.id)).ToList();
        return this;
    }

    public void InitDeck()
    {
        Counter = InitCounter;
        CardByUID = new Dictionary<int, Card>();

        for (int i=0; i<cards.Count; i++)
        {
            var uId = Counter++;
            cards[i].uId = uId;

            CardByUID[uId] = cards[i];
        }

    }

    public Deck()
    {

    }

    public Deck(bool playerDeck = false)
    {
        if(playerDeck)
            LoadPlayerDeck();
    }

    public Deck(Deck deck)
    {
        Counter = deck.Counter;
        cards = deck.Export();
    }

    public int Size() => cards.Count;

    public void AddCard(Card card)
    {
        card.uId = Counter++;
        cards.AddCard(card);
    }

    public bool AddCard(int cardId)
    {
        if (CardLibrary.Instance.ValidateCardById(cardId))
        {
            AddCard(CardLibrary.Instance.GetCardById(cardId));
            return true;
        }

        Debug.LogError($"Adding invalid card [{cardId}]");
        return false;
    }

    void ResetDeck()
    {
        cards.Clear();
        Counter = InitCounter;
    }

    public void CloneFromDeck(Deck deck)
    {
        ResetDeck();

        cards = new List<Card>();

        foreach (var card in deck.Export())
        {
            AddCard(card);
        }
    }

    public List<Card> Export()
    {
        return cards.ToList();
    }

    void LoadInitialDeck()
    {
        var initialDeck = Resources.Load("Deck/InitialDeck") as Deck;

        if (initialDeck != null)
        {
            CloneFromDeck(initialDeck);
        }
        else
        {
            //for (int i = 0; i < 5; i++)
            //{
            //    var atkCard = CardLibrary.Instance.GetCardById(10001);
            //    AddCard(atkCard);
            //}

            //for (int i = 0; i < 5; i++)
            //{
            //    var defCard = CardLibrary.Instance.GetCardById(10002);
            //    AddCard(defCard);
            //}
        }
    }

    public void LoadPlayerDeck(Deck deck = null)
    {
        if (!deck)
        {
            var playerDeck = Resources.Load("Deck/PlayerDeck") as Deck;

            if (playerDeck != null)
            {
                CloneFromDeck(playerDeck);
            }
            else
            {
                LoadInitialDeck();
            }
        }
        else
        {
            CloneFromDeck(deck);
        }

    }

    public void ReplaceCard(int uId, Card card)
    {
        if(CardByUID.ContainsKey(uId))
        {
            CardByUID[uId] = card;
            cards = CardByUID.Values.ToList();
        }
    }

    public void RemoveCard(Card card)
    {
        if(cards.Contains(card))
        {
            cards.Remove(card);
        }
    }

}

