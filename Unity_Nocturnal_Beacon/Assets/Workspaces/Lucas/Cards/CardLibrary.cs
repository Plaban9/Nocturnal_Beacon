using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLibrary : MonoBehaviour
{
    public static CardLibrary Instance { get; private set; }

    [SerializeField] Dictionary<int, Card> cardsDict = new Dictionary<int, Card>();
    
    [SerializeField] List<Card> cards = new List<Card>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Init();
    }

    void Init()
    {
        Reset();
        LoadAllCards();
    }

    void LoadAllCards()
    {
        var cardObjects = Resources.LoadAll<Card>("CardObject");

        foreach (var cardObject in cardObjects)
        {
            if(!cardsDict.TryAdd(cardObject.id, cardObject))
            {
                Debug.LogError($"Card [{cardObject.name}] has duplicated Id: {cardObject.id}");
            }
            cards.Add(cardObject);
        }
    }

    private void Reset()
    {
        cardsDict.Clear();
    }

    #region Get
    public Card GetCardById(int id)
    {
        if (cardsDict.TryGetValue(id, out Card result))
        {
            return result;
        }
        else
            throw new System.ArgumentException($"[ERROR] Card not found. Id: ${id}");
    }

    public bool ValidateCardById(int id) => cardsDict.ContainsKey(id);
    #endregion

    
}
