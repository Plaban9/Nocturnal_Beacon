using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class MapQstNodeScreen : MapNonBattleNodeScreen
{
    [Header("DEBUG")]
    [SerializeField] MapQuest _mapQuest;

    [Header("Assets")]
    [SerializeField] TextMeshProUGUI _title;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _description;
    [SerializeField] TextMeshProUGUI _questOutcome;
    [SerializeField] GameObject _cardHolder;
    [SerializeField] GameObject _cardPrefab;
    [SerializeField] CardDisplay _resultCard;
    [SerializeField] GameObject _outcomeHolder;
    [SerializeField] GameObject _chooserHolder;

    Animator _animator;
    List<CardPickerHoverables> _cardHoverables = new List<CardPickerHoverables>();

    public void Start()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
    }

    public void SetQuest(MapQuest quest)
    {
        if (quest != null)
        {
            _mapQuest = quest;
            _title.text = _mapQuest.title;
            string finalQuestString = "";
            foreach (MapQuestConditional questConditional in _mapQuest.conditions)
            {
                finalQuestString += string.Format("{0}\n", questConditional.GetReqString());
            }

            _description.text = string.Format("{0}\n\n{1}",
                _mapQuest.eventDescription,
                finalQuestString);
            _icon.material.mainTexture = _mapQuest.image;
            SetCards(quest.randomCardsAvailable);
        }
        else
        {
            foreach(Transform t in _chooserHolder.transform)
            {
                Destroy(t.gameObject);
            }
        }
    }

    public void SetCards(int numberOfCards)
    {
        Deck userDeck = NoctBeaconRunData.Instance.GetPlayerInformation().GetCurrentDeck();
        List<Card> deckCards = userDeck.Export();
        List<Card> cards = new List<Card>();
        _chooserHolder.GetComponent<CanvasGroup>().interactable = true;
        _chooserHolder.GetComponent<CanvasGroup>().blocksRaycasts = true;

        int cardsOnHolder = _cardHolder.transform.childCount;
        if (cardsOnHolder < numberOfCards)
        {
            for (int i = 0; i < numberOfCards - cardsOnHolder; i++)
            {
                GameObject c = Instantiate(_cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, _cardHolder.transform);
                _cardHoverables.Add(c.GetComponent<CardPickerHoverables>());
                c.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            }
        }

        foreach (CardPickerHoverables card in _cardHoverables)
        {
            card.gameObject.SetActive(false);
        }

        int index = 0;
        foreach (CardPickerHoverables card in _cardHoverables)
        {
            index++;
            if (index < numberOfCards)
            {
                card.gameObject.SetActive(true);
            }
        }

        if (cardsOnHolder > numberOfCards)
        {
            for (int i = cardsOnHolder - 1; i > numberOfCards; i--)
            {
                _cardHoverables[i].gameObject.SetActive(false);
            }
        }


        if (deckCards.Count <= numberOfCards)
        {
            cards.AddRange(deckCards);
        }
        else
        {
            cards = deckCards.OrderBy(x => Random.Range(0f, 1f)).Take(numberOfCards).ToList();
        }

        var qstNodeScreen = this;
        for (int i = 0; i <= cards.Count-1; i++)
        {
            _cardHoverables[i].GetComponent<CardDisplay>().Setup(cards[i]);
            _cardHoverables[i].SetData(i, cards[i], _mapQuest);
            _cardHoverables[i].SetOnClick((i, card) =>
            {
                SelectionStart(card, i);
            });
        }
        _chooserHolder.GetComponent<CanvasGroup>().interactable = true;
        _chooserHolder.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    private void SelectionStart(Card card, int i)
    {
        SelectCard(card);
        for (int o = 0; o < _cardHoverables.Count-1; o++)
        {
            if (o != i)
            {
                _cardHoverables[o].Close();
            }
        }
    }
    private void SelectCard(Card card)
    {
        _chooserHolder.GetComponent<CanvasGroup>().interactable = false;
        _chooserHolder.GetComponent<CanvasGroup>().blocksRaycasts = false;
        StartCoroutine(PerformSelectCard(card));
    }

    private IEnumerator PerformSelectCard(Card card)
    {
        yield return new WaitForSeconds(1f);
        _resultCard.GetComponent<CardDisplay>().Setup(card);
        _animator.SetBool("outcome", true);
        string finalSelectedString = "Result\n\n";
        int finalResult = 0;
        foreach(MapQuestConditional questConditional in _mapQuest.conditions)
        {
            finalResult += questConditional.GetResult(card);
            finalSelectedString += string.Format("{0}\n", questConditional.GetResString(card));
        }

        finalSelectedString += string.Format("\n{0}\nGained {1} cost to customize Card {2} !",
            _mapQuest.questEnd,
            _mapQuest.rewards.GetReward(finalResult),
            card.name);

        _questOutcome.text = finalSelectedString;
        yield return new WaitForSeconds(1.5f);
        _manager.SetProgressContinue();
        _manager.ShowContinue();
        _manager.SetOnContinueCallback(skipped =>
        {
            int amountToCustomize = _mapQuest.rewards.GetReward(finalResult);
            Card cardToCustomize = card;
            var ccp = UIManager.Instance.ShowPage(GamePage.CustomizeCardPage).GetComponent<CustomizeCardPage>();
            ccp.Setup(cardToCustomize /*The card*/, amountToCustomize /*The value you want*/).Subscribe(x =>
            {
                DeactivateNonBattleNodeScreen();
            }).AddTo(ccp);
        });
    }



    public override void ActivateNonBattleNodeScreen()
    {
        GetComponent<CanvasGroup>().alpha = 1f;
        _animator.SetBool("open", true) ;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }


    public override void DeactivateNonBattleNodeScreen()
    {
        _animator.SetBool("open", false);
        _animator.SetBool("outcome", false);
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<CanvasGroup>().DOFade(0f, 2f);
        SetQuest(null);
        _manager.HideContinue();
    }
}
