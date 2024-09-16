using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Deck", menuName = "Deck")]
public class Deck : ScriptableObject
{
    [SerializeField, SerializeReference] List<Card> cards = new List<Card>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Deck(bool playerDeck = false)
    {
        if(playerDeck)
            LoadPlayerDeck();
    }

    public Deck(Deck deck)
    {
        cards = deck.Export();
    }

    public int Size() => cards.Count;

    public void AddCard(Card card)
    {
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
    }

    void CloneFromDeck(Deck deck)
    {
        ResetDeck();
        cards = new List<Card>();

        foreach (var card in deck.Export())
        {
            cards.AddCard(card);
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
            for (int i = 0; i < 5; i++)
            {
                var atkCard = CardLibrary.Instance.GetCardById(10001);
                cards.AddCard(atkCard);
            }

            for (int i = 0; i < 5; i++)
            {
                var defCard = CardLibrary.Instance.GetCardById(10002);
                cards.AddCard(defCard);
            }
        }
    }

    void LoadPlayerDeck()
    {
        var playerDeck = Resources.Load("Deck/PlayerDeck") as Deck;

        if(playerDeck != null)
        {
            CloneFromDeck(playerDeck);
        }
        else
        {
            LoadInitialDeck();
        }
    }
}

