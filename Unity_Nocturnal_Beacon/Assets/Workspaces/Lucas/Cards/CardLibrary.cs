using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        var cardObjects = Resources.LoadAll<Card>("CardObject/PlayerCards");

        foreach (var cardObject in cardObjects)
        {
            if(!cardsDict.TryAdd(cardObject.id, cardObject))
            {
                Debug.LogError($"Card [{cardObject.name}] has duplicated Id: {cardObject.id}");
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

    public int GetNextCardId() => cardsDict.Keys.Max() + 1;
    #endregion


    public bool AddNewCard(Card card)
    {
        card.id = GetNextCardId();
        return cardsDict.TryAdd(card.id, card);
    }
}
