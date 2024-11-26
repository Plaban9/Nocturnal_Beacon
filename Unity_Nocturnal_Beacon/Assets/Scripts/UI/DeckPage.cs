using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using TMPro;

public class DeckPage : CommonPage
{
    [SerializeField] GameObject cardDisplayPrefab;
    [SerializeField] Transform content;
    [SerializeField] Texture2D onPointCursor;
    [SerializeField] TextMeshProUGUI promptText;
    [SerializeField] GameObject closeBtn;

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

    public void Setup(Deck deck = null, Action<CardDisplay> onDeckCardClick = null)
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
        if (transform.localPosition.y > -Screen.height) return;

        transform.DOLocalMoveY(-50, 0.5f);
        canvasGroup.DOFade(1, 0.5f);
    }

    public override void Close()
    {
        if (isClosing) return;

        isClosing = true;

        transform.DOLocalMoveY(-Screen.height-100, 0.5f);
        canvasGroup.DOFade(0, 0.5f).onComplete += () =>
        {
            gameObject.SetActive(false);
            isClosing = false;
        };
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

    public void SetCloseBtn(bool set) => closeBtn.SetActive(set);
}
