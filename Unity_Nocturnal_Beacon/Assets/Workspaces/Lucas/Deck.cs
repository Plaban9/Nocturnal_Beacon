using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

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

    void ResetDeck()
    {
        cards.Clear();
    }

    public List<Card> Export()
    {
        return cards;
    }

    void LoadInitialDeck()
    {
        var initialDeck = Resources.Load("Deck/InitialDeck") as Deck;

        if (initialDeck != null)
        {
            cards = new List<Card>();

            foreach (var card in initialDeck.Export())
            {
                cards.Add(card);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                var atkCard = CardLibrary.Instance.GetCardById(10001);
                cards.Add(atkCard);
            }

            for (int i = 0; i < 5; i++)
            {
                var defCard = CardLibrary.Instance.GetCardById(10002);
                cards.Add(defCard);
            }
        }
    }
}

