using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (drawPileCounter)
        {
            drawPileCounter.text = _cardPileManager?.GetPileSize(CardPileManager.PileType.Draw).ToString();
            discardPileCounter.text = _cardPileManager?.GetPileSize(CardPileManager.PileType.Discard).ToString();
        }
    }

    public void SetDeck(Deck deck)
    {
        _playerDeck.LoadPlayerDeck(deck);
        _cardPileManager.Init(_playerDeck);

    }

    [ContextMenu("Draw Card")]
    public void DrawCard(int i, UnitStatusData ubsf)
    {
        StartCoroutine(PerformDrawCard(i, ubsf)); 
    }

    IEnumerator PerformDrawCard(int amount, UnitStatusData ubsf)
    {
        for (int i = 0; i < amount; i++)
        {
            var card = _cardPileManager.DrawCard();

            /*
             * If player has confusion, add tag to randomize.
             * Flushes previous effects on cards to avoid being affected on turns where you are not confused
             */
            card.FlushStatuses();
            if(ubsf.GetStatusEffects().Any(it => it._status.statusEffect == CardAttribute.StatusEffect.Confused))
            {
                card.AddStatus(new CardStatus_RandomCost());
            }

            handZone.AddCard(card);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public bool DeployCard(Card card, BattleUnit target)
    {
        if (_bm.PlayerTryToUseCard(card, target))
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

    public IEnumerator DiscardHandZoneCard()
    {
        var cards = handZone.GetCardsInHand();
        cards.Reverse();

        foreach(var c in cards)
        {
            yield return StartCoroutine(c.PerformDiscard());
            _cardPileManager.DiscardCard(c.GetCard());
            handZone.RemoveCard(c);
        }

        yield return new WaitForSeconds(1f);

        /* TODO:
         * Animation of discarding cards?
         */
    }
}
