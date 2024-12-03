using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CardLibrary : MonoBehaviour
{
    public static CardLibrary Instance { get; private set; }

    [SerializeField] Dictionary<int, Card> cardsDict = new Dictionary<int, Card>();
    
    [SerializeField] List<Card> cards = new List<Card>();

    [SerializeField] List<Card> shopCards = new List<Card>();

    static readonly int InitCounter = 1000000;

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
        var cardObjects = Resources.LoadAll<Card>("CardObject/PlayerCards");

        foreach (var cardObject in cardObjects)
        {
            if(!cardsDict.TryAdd(cardObject.id, cardObject))
            {
                Debug.LogError($"Card [{cardObject.name}] has duplicated Id: {cardObject.id} with {cardsDict[cardObject.id].name}.");
            }
            cards.Add(cardObject);
        }

        #if !UNITY_EDITOR
        var customizedCards = ScriptableObjectSaver.LoadScriptableObject<Card>("PlayerCards");

        foreach(var cc in customizedCards)
        {
            if(!cardsDict.TryAdd(cc.id, cc))
            {
                Debug.LogError($"Customized Card [{cc.name}] has duplicated Id: {cc.id}");
            }

            cards.Add(cc);
        }
        #endif

        shopCards = Resources.LoadAll<Card>("CardObject/ShopCards").ToList();
    }

    public List<Card> GetNonCustomizedCards()
    {
        return cards.Where(x => !x.isCustomized).ToList();
    }

    public List<Card> GetShopCards() => shopCards;
    
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

    public int GetNextCardId() => cardsDict.Keys.Max() + 1;
    #endregion


    public bool AddNewCard(Card card)
    {
        card.id = GetNextCardId();
        return cardsDict.TryAdd(card.id, card);
    }

    [ContextMenu("Resort Cards ID (USE CAREFULLY)")]
    public void ResortCardsId()
    {
        var cardObjects = Resources.LoadAll<Card>("CardObject/PlayerCards");
        var idCounter = InitCounter;

       foreach(var cardObject in cardObjects)
        {
            cardObject.id = ++idCounter;
            EditorUtility.SetDirty(cardObject);            
        }

    }
}
