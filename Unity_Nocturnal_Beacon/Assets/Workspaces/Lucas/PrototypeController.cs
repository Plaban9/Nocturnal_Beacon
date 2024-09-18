using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PrototypeController : MonoBehaviour
{
    public static PrototypeController Instance { get; private set; }

    [SerializeField] HandZone handZone;
    Deck playerDeck;
    CardPileManager cardPileManager;

    [Header("UI")]
    [SerializeField] TMPro.TMP_Text drawPileCounter;
    [SerializeField] TMPro.TMP_Text discardPileCounter;
    [SerializeField] TMPro.TMP_Text manaCounter;

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
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerDeck = new Deck(true);
        cardPileManager = new CardPileManager(playerDeck);
    }

    // Update is called once per frame
    void Update()
    {
        drawPileCounter.text = cardPileManager.GetPileSize(CardPileManager.PileType.Draw).ToString();
        discardPileCounter.text = cardPileManager.GetPileSize(CardPileManager.PileType.Discard).ToString();
    }

    [ContextMenu("Draw Card")]
    public void DrawCard()
    {
        StartCoroutine(PerformDrawCard(5));
    }

    IEnumerator PerformDrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var card = cardPileManager.DrawCard();
            handZone.AddCard(card);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void DeployCard(Card card)
    {
        /*
         * Do deploy logic
         */

        cardPileManager.AddCardToBottom(CardPileManager.PileType.Discard, card);
    }
}
