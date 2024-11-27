using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class CardSelectionPage : CommonPage
{
    [SerializeField] Deck debugDeck;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform content;
    [SerializeField] GameObject cancelBtn;

    Action<Card> onSelect;
    Subject<bool> onCancel = new();

    List<Card> cards;
    List<CardDisplay> displays;
    float fadeDuration = 0.5f;

    private void Start()
    {
        if(debugDeck != null)
        {
            Setup(debugDeck, null);
        }
    }

    public void Setup(Deck deck, Action<Card> onSelect, bool fadeInAnimation = true, float fadeDuration = 0.5f)
    {
        Setup(deck.Export(), onSelect, fadeInAnimation, fadeDuration);
    }

    public void Setup(List<Card> cards, Action<Card> onSelect, bool fadeInAnimation = true, float fadeDuration = 0.5f)
    {
        this.cards = cards;
        this.onSelect = onSelect;
        this.fadeDuration = fadeDuration;

        displays = new List<CardDisplay>();

        foreach (Card card in cards)
        {
            var cd = Instantiate(cardPrefab, content).GetComponent<CardDisplay>();
            cd.SetupForClickable(card, OnSelectCard);
            cd.SetScale(0.5f);
            displays.Add(cd);

            if (fadeInAnimation)
                cd.FadeOut(0);
        }


        if (fadeInAnimation)
        {
            FadeInCard(0);
        }
    }

    public void SetTitle(string s)
    {
        title.gameObject.SetActive(s.Length > 0);
        title.text = s;
    }

    public void EnableCancelButton(bool set) => cancelBtn.SetActive(set);

    public void OnSelectCard(CardDisplay card)
    {
        onSelect?.Invoke(card.GetCard());
    }

    void FadeInCard(int index)
    {
        var cd = displays[index];

        cd.FadeIn(fadeDuration).Subscribe(x =>
        {
            if (index < displays.Count - 1)
            {
                FadeInCard(++index);
            }
        }).AddTo(cd.gameObject);
    }

    public void OnClickCancel()
    {
        onCancel?.OnNext(true);

        Close();
    }
}
