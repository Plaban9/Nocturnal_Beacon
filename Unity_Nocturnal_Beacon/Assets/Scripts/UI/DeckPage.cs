using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices.WindowsRuntime;

public class DeckPage : CommonPage
{
    [SerializeField] GameObject cardDisplayPrefab;
    [SerializeField] Transform content;
    [SerializeField] Texture2D onPointCursor;
    [SerializeField] TextMeshProUGUI promptText;
    [SerializeField] Button closeBtn;

    List<CardDisplay> cardDisplayList = new List<CardDisplay>();

    CanvasGroup canvasGroup;
    bool isClosing;

    Action<CardDisplay> onDeckCardClick;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        var pos = transform.localPosition;
        pos.y = -Screen.height - 100;
        transform.localPosition = pos;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Setup()
    {
        Setup(NoctBeaconRunData.Instance.GetPlayerInformation().GetCurrentDeck().Export());
    }

    public void Setup(List<Card> deck)
    {
        var cards = deck;

        for (int i = 0; i < cards.Count; i++)
        {
            if (i >= cardDisplayList.Count)
            {
                var cd = Instantiate(cardDisplayPrefab, content).GetComponent<CardDisplay>();
                cd.Setup(cards[i]);
                cd.SetScale(0.45f);
                cardDisplayList.Add(cd);
            }
            else
            {
                cardDisplayList[i].Setup(cards[i]);
                cardDisplayList[i].gameObject.SetActive(true);
            }
        }

        for (int i = cards.Count; i < cardDisplayList.Count; i++)
        {
            cardDisplayList[i].gameObject.SetActive(false);
        }
    }

    public void Setup(Deck deck, Action<CardDisplay> onDeckCardClick)
    {
        if(deck == null)
        {
            deck = NoctBeaconRunData.Instance.GetPlayerInformation().GetCurrentDeck();
        }

        if(onDeckCardClick != null)
            this.onDeckCardClick = onDeckCardClick;

        var cards = deck.Export();

        for(int i=0; i<cards.Count; i++)
        {
            if (i >= cardDisplayList.Count)
            {
                var cd = Instantiate(cardDisplayPrefab, content).GetComponent<CardDisplay>();
                cd.SetupForClickable(cards[i], OnDeckCardClick, onPointCursor);
                cd.SetScale(0.45f);
                cardDisplayList.Add(cd);
            }
            else
            {
                cardDisplayList[i].SetupForClickable(cards[i], OnDeckCardClick, onPointCursor);
            }
        }
    }

    public void Show()
    {
        if (IsShowing() || isClosing) return;

        gameObject.SetActive(true);

        transform.DOLocalMoveY(0, 0.5f);
        canvasGroup.DOFade(1, 0.25f);
    }

    public override void Close()
    {
        if (isClosing || transform.localPosition.y < 0) return;

        isClosing = true;

        transform.DOLocalMoveY(-Screen.height - 100, 0.5f);
        canvasGroup.DOFade(0, 0.5f).onComplete += () =>
        {
            gameObject.SetActive(false);
            isClosing = false;
        };
    }

    public void Kill()
    {
        base.Close();
    }
    
    public void Refresh()
    {
        Setup();
    }

    public void OnDeckCardClick(CardDisplay card)
    {
        onDeckCardClick?.Invoke(card);
    }

    public void SetPromptText(string t)
    {
        promptText.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(t));
        promptText.text = t;
    }

    
    public void SetPointCursor(Texture2D t) => onPointCursor = t;

    public void SetCloseBtn(bool set) => closeBtn.gameObject.SetActive(set);

    public Button GetCloseButton() => closeBtn;

    public bool IsShowing() => transform.localPosition.y == 0;
}
