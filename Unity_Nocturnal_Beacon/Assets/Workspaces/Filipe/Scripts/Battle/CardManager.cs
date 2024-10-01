using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    /*
     * Originally made by Lucas! Modified by Filipe.
     */

    [SerializeField] HandZone handZone;
    Deck _playerDeck;
    CardPileManager _cardPileManager;

    [Header("UI")]
    [SerializeField] TMPro.TMP_Text drawPileCounter;
    [SerializeField] TMPro.TMP_Text discardPileCounter;
    [SerializeField] TMPro.TMP_Text manaCounter;

    private BattleManager _bm;

    private void Awake()
    {
        _playerDeck = new Deck(true);
        _cardPileManager = new CardPileManager(_playerDeck);
        _bm = GetComponent<BattleManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        drawPileCounter.text = _cardPileManager.GetPileSize(CardPileManager.PileType.Draw).ToString();
        discardPileCounter.text = _cardPileManager.GetPileSize(CardPileManager.PileType.Discard).ToString();
    }

    [ContextMenu("Draw Card")]
    public void DrawCard(int i)
    {
        StartCoroutine(PerformDrawCard(i));
    }

    IEnumerator PerformDrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var card = _cardPileManager.DrawCard();
            handZone.AddCard(card);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public bool DeployCard(Card card)
    {
        if (_bm.PlayerTryToUseCard(card))
        {
            _cardPileManager.AddCardToBottom(CardPileManager.PileType.Discard, card);
            return true;
        }
        else
        {
            // TODO: INFORM THE PLAYER THEY CANT USE CARD
            return false;
        }

    }
}
