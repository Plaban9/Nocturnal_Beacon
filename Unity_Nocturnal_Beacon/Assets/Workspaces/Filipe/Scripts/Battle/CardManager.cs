using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
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
    [SerializeField] GameObject drawPileTitle;
    [SerializeField] GameObject discardPileTitle;

    [SerializeField] TMPro.TMP_Text drawPileCounter;
    [SerializeField] TMPro.TMP_Text discardPileCounter;
    [SerializeField] TMPro.TMP_Text manaCounter;

    [SerializeField] DeckPage pilePage;

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
        if (ubsf.GetStatusEffects().Any(it => it._status.statusEffect == CardAttribute.StatusEffect.NoDraw)) return;
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

    [ContextMenu("Draw Card (Effect)")]
    public void DrawRandomFromPile(CardPileManager.PileType cardpile, int amount, List<CardFilter> cardFilters, UnitStatusData ubsf)
    {
        if (ubsf.GetStatusEffects().Any(it => it._status.statusEffect == CardAttribute.StatusEffect.NoDraw)) return;
        StartCoroutine(PerformDrawRandomFromPile(cardpile, amount, cardFilters, ubsf));
    }

    IEnumerator PerformDrawRandomFromPile(CardPileManager.PileType cardpile, int amount, List<CardFilter> cardFilters, UnitStatusData ubsf)
    {
        List<Card> cards = _cardPileManager.GetCardsInPile(cardpile, cardFilters);
        List<int> cardIndex = new List<int>();
        /*
         * All this to make sure that the drawing does not draw the same card twice or affect the list during drawing.
         */
        if (amount < cards.Count)
        {
            for (int i = 0; i < amount; i++)
            {
                int value = Random.Range(0, cards.Count - 1);
                while (cardIndex.Contains(value))
                {
                    value = Random.Range(0, cards.Count - 1);
                }
                cardIndex.Add(value);
            }
        }
        if (cardIndex.Count > 0 && amount > 0)
        {
            foreach (int value in cardIndex)
            {
                Card card = cards[value];
                
                /*
                * If player has confusion, add tag to randomize.
                * Flushes previous effects on cards to avoid being affected on turns where you are not confused
                */
                card.FlushStatuses();
                if (ubsf.GetStatusEffects().Any(it => it._status.statusEffect == CardAttribute.StatusEffect.Confused))
                {
                    card.AddStatus(new CardStatus_RandomCost());
                }

                Debug.Log($"Drawing {card.name} from {cardpile.ToString()}...");

                _cardPileManager.RemoveCard(cardpile,card);
                handZone.AddCard(card);
                yield return new WaitForSeconds(0.2f);
            }
        }
        else
        {
            /*
             * If the number of drawn cards is bigger or equal to amount of cards available, no point in setting which ones to draw.
             */
            foreach (Card card in cards)
            {

                /*
                * If player has confusion, add tag to randomize.
                * Flushes previous effects on cards to avoid being affected on turns where you are not confused
                */
                card.FlushStatuses();
                if (ubsf.GetStatusEffects().Any(it => it._status.statusEffect == CardAttribute.StatusEffect.Confused))
                {
                    card.AddStatus(new CardStatus_RandomCost());
                }

                _cardPileManager.RemoveCard(cardpile,card);
                handZone.AddCard(card);

                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    [ContextMenu("Draw Specific Cards (Effect)")]
    /*
     * This ought to be used only after checking which cards are on the pile, please.
     */
    public void DrawSpecificCardsFromPile(CardPileManager.PileType cardpile, List<Card> cards, UnitStatusData ubsf)
    {
        if (ubsf.GetStatusEffects().Any(it => it._status.statusEffect == CardAttribute.StatusEffect.NoDraw)) return;
        StartCoroutine(PerformDrawSpecificCardsFromPile(cardpile, cards, ubsf));
    }

    IEnumerator PerformDrawSpecificCardsFromPile(CardPileManager.PileType cardpile, List<Card> cards, UnitStatusData ubsf)
    {
        foreach (Card card in cards)
        {

            /*
            * If player has confusion, add tag to randomize.
            * Flushes previous effects on cards to avoid being affected on turns where you are not confused
            */
            card.FlushStatuses();
            if (ubsf.GetStatusEffects().Any(it => it._status.statusEffect == CardAttribute.StatusEffect.Confused))
            {
                card.AddStatus(new CardStatus_RandomCost());
            }

            _cardPileManager.RemoveCard(cardpile, card);
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

    public void DiscardRandomCard(int amount)
    {
        StartCoroutine(PerformDiscardRandomCard(amount));
    }

    public IEnumerator PerformDiscardRandomCard(int amount)
    {
        var cards = handZone.GetCardsInHand();
        if (cards.Count <= amount)
        {
            StartCoroutine(DiscardHandZoneCard());
            yield break;
        } 

        for (int i = 0; i < amount; i++)
        {
            var cardsInHand = handZone.GetCardsInHand().FindAll(it=> it.IsAvailableToDiscard());
            var card = cardsInHand[Random.Range(0, cardsInHand.Count - 1)];
            yield return StartCoroutine(card.PerformDiscard());
            _cardPileManager.DiscardCard(card.GetCard());
            handZone.RemoveCard(card);
        }

        yield return new WaitForSeconds(1f);

        /* TODO:
         * Animation of discarding cards?
         */
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

    public List<Card> CardsInHand()
    {
        return handZone.GetCards();
    }

    #region UI
    public void OnClickDrawPile()
    {
        if (pilePage.IsShowing()) return;

        var deck = _cardPileManager.PreviewDrawPile();
        //pilePage = UIManager.Instance.ShowPage(GamePage.DeckPage).GetComponent<DeckPage>();
        pilePage.Setup(deck);
        pilePage.SetPromptText("Cards are drawn from here at the start of each turn.\n" +
            "<size=28><color=#FFA342>(Cards shown are sorted by rarity)</color></size>");
        pilePage.Show();
        var closeBtn = pilePage.GetCloseButton();
        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(() =>
        {
            pilePage.Close();
            drawPileTitle.SetActive(false);
        });

        pilePage.transform.SetAsLastSibling();
        drawPileTitle.transform.parent.SetAsLastSibling();
        drawPileTitle.SetActive(true);
    }

    public void OnClickDiscardPile()
    {
        if (pilePage.IsShowing()) return;

        var deck = _cardPileManager.PreviewDiscardPile();
        //pilePage = UIManager.Instance.ShowPage(GamePage.DeckPage).GetComponent<DeckPage>();
        pilePage.Setup(deck);
        pilePage.SetPromptText("Cards here are shuffled into your draw pile when it runs out of cards.");
        pilePage.Show();
        var closeBtn = pilePage.GetCloseButton();
        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(() =>
        {
            pilePage.Close();
            discardPileTitle.SetActive(false);
        });

        pilePage.transform.SetAsLastSibling();
        discardPileTitle.transform.parent.SetAsLastSibling();
        discardPileTitle.SetActive(true);
    }
    #endregion
}
