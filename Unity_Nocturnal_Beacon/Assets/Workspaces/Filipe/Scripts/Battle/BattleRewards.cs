using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class BattleRewards : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button _addCardButton;
    [SerializeField] Button _modifyCardButton;
    [SerializeField] Button _healButton;
    [SerializeField] Button _skipButton;



    [Header("Assets")]
    [SerializeField] GameObject _rewardsCanvas;
    [SerializeField] GameObject _cardRewardHolder;
    [SerializeField] GameObject _cardRewardCanvas;
    [SerializeField] GameObject _cardHoverable;
    List<CardPickerHoverables> _cardHoverables = new List<CardPickerHoverables>();

    private void Start()
    {
        _addCardButton.onClick.AddListener(OpenNewCardReward);
        _modifyCardButton.onClick.AddListener(OpenModifyCardReward);
        _healButton.onClick.AddListener(OpenRegenRewards);
        _skipButton.onClick.AddListener(Skipping);
    }

    public void DisableOptions()
    {
        _rewardsCanvas.GetComponent<CanvasGroup>().interactable = false;
        _rewardsCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
        _rewardsCanvas.GetComponent<CanvasGroup>().DOFade(0f, 1f);
    }

    public void Skipping()
    {
        DisableOptions();
        HideSelf();
        StartCoroutine(ToMainMenuAfter(1f));
    }

    public void OpenNewCardReward()
    {
        DisableOptions();
        _cardRewardHolder.SetActive(true);
        _cardRewardCanvas.GetComponent<CanvasGroup>().DOFade(1f, 1f);
        _cardRewardCanvas.GetComponent<CanvasGroup>().interactable = true;
        _cardRewardCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
        _cardRewardHolder.GetComponent<CanvasGroup>().DOFade(1f, 1f);
        SetRewardCards(3);
    }

    public void OpenModifyCardReward()
    {
        DisableOptions();

        HideSelf();
        List<Card> allAvailableCards = NoctBeaconRunData.Instance.GetPlayerInformation().GetCurrentDeck().Export();
        allAvailableCards.Shuffle();
        Card card = allAvailableCards.Take(1).First();

        int amountToCustomize = 5;
        Card cardToCustomize = card;
        var ccp = UIManager.Instance.ShowPage(GamePage.CustomizeCardPage).GetComponent<CustomizeCardPage>();
        ccp.Setup(cardToCustomize /*The card*/, amountToCustomize /*The value you want*/).Subscribe(x =>
        {
            StartCoroutine(ToMainMenuAfter(2f));
        }).AddTo(ccp);
    }

    public void OpenRegenRewards()
    {
        DisableOptions();
        HideSelf();
        BattleUnit userHPData = BattleManager.Instance.GetPlayerbattleUnit();
        BattleManager.Instance.GetPlayerbattleUnit().GetHPData().RecoverHealth(
            (int)Mathf.Floor((float)userHPData.GetUnitData().maxHp * 0.20f)
            );
        StartCoroutine(ToMainMenuAfter(2f)); 
    }

    private IEnumerator ToMainMenuAfter(float amount = 1f)
    {
        yield return new WaitForSeconds(amount);
        HideSelf();
        BattleManager.Instance.ToMap();
    }


    public void SetRewardCards(int numberOfCards)
    {
        //Load all cards
        Object[] cardsOnReward;
        cardsOnReward = Resources.LoadAll("CardObject/PlayerCards/Obtainable",typeof(Card));
        List<Card> availableCards = new List<Card>();

        foreach(Card card in cardsOnReward)
        {
            availableCards.Add(card);
        }


        List<Card> cards = new List<Card>();
        _cardRewardHolder.GetComponent<CanvasGroup>().interactable = true;
        _cardRewardHolder.GetComponent<CanvasGroup>().blocksRaycasts = true;

        int cardsOnHolder = _cardRewardHolder.transform.childCount;

        for (int i = 0; i < numberOfCards - cardsOnHolder; i++)
        {
            GameObject c = Instantiate(_cardHoverable, new Vector3(0, 0, 0), Quaternion.identity, _cardRewardHolder.transform);
            _cardHoverables.Add(c.GetComponent<CardPickerHoverables>());
            c.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }

        if (availableCards.Count <= numberOfCards)
        {
            cards.AddRange(availableCards);
        }
        else
        {
            cards = availableCards.OrderBy(x => Random.Range(0f, 1f)).Take(numberOfCards).ToList();
        }

        for (int i = 0; i <= cards.Count - 1; i++)
        {
            _cardHoverables[i].GetComponent<CardDisplay>().Setup(cards[i]);
            _cardHoverables[i].SetData(i, cards[i]);
            _cardHoverables[i].SetOnClick((i, card) =>
            {
                SelectionStart(card, i);
            });
        }
    }

    private void SelectionStart(Card card, int i)
    {
        SelectCard(card);
        for (int o = 0; o < _cardHoverables.Count; o++)
        {
            if (o != i)
            {
                _cardHoverables[o].Close();
            }
        }
    }
    private void SelectCard(Card card)
    {
        _cardRewardHolder.GetComponent<CanvasGroup>().interactable = false;
        _cardRewardHolder.GetComponent<CanvasGroup>().blocksRaycasts = false;
        PerformSelectCard(card);
    }

    private void PerformSelectCard(Card card)
    {
        NoctBeaconRunData.Instance.GetPlayerInformation().GetCurrentDeck().AddCard(card);
        StartCoroutine(ToMainMenuAfter(1f));

    }

    private void HideSelf()
    {
        GetComponent<CanvasGroup>().DOFade(0f, 1f);
    }
}
