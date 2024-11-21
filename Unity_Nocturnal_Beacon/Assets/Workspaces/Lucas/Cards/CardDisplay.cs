using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CardAttribute;
using UnityEngine.EventSystems;
using UniRx;
using System;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] bool enablePointToZoom = false;
    [SerializeField] float zoomRatio = 0.6f;
    [SerializeField] Texture2D onZoomCursor;

    [SerializeField] protected Card card;

    [SerializeField] TMP_Text manaText;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text typeText;
    [SerializeField] TMP_Text descText;

    [SerializeField] Image element;
    [SerializeField] Image background;
    [SerializeField] Image title;

    [SerializeField] Image mainImg;

    float oriZoomRatio = 1f;

    Action onClick;
    protected virtual void Start()
    {
        oriZoomRatio = transform.localScale.x;


        if (card != null)
        {
            Setup(card);
        }
    }

    public virtual void Setup(Card card)
    {
        this.card = card;

        manaText.text = card.manaCost >= 0 ? card.manaCost.ToString() : "X";
        titleText.text = card.name.ToString();
        typeText.text = card.cardType.ToString();
        descText.text = card.GetEffectDescStr();

        mainImg.sprite = card.sprite;
        SetColors(card); 
    }

    public void SetScale(float scale)
    {
        transform.localScale = Vector3.one * scale;
        oriZoomRatio = transform.localScale.x;
    }

    public virtual void SetupForDeck(Card card, Action onClick)
    {
        enablePointToZoom = true;
        this.card = card;
        this.onClick = onClick;

        manaText.text = card.manaCost >= 0 ? card.manaCost.ToString() : "X";
        titleText.text = card.name.ToString();
        typeText.text = card.cardType.ToString();
        descText.text = card.GetEffectDescStr();

        mainImg.sprite = card.sprite;
        SetColors(card);
    }

    public virtual void SetColors(Card card)
    {
        Color goodColor = new(1, 0.7f, 0.3f);

        title.color = goodColor;
        background.color = GetRarityColor(card.rarity);
        element.color = GetElementColor(card.element);
    }

    public Color GetElementColor(Element element)
    {
        switch (element)
        {
            case Element.NONE:
                return new Color(0.9f, 0.6f, 0.7f);
            case Element.EARTH:
                return new Color(0.8f, 0.4f, 0f);
            case Element.WIND:
                return new Color(0.5f, 1f,0f);
            case Element.WATER:
                return new Color(0.2f, 0.7f, 1f);
            case Element.FIRE:
                return new Color(1f, 0.2f, 0f);
            case Element.DARK:
                return new Color(0.5f, 0f, 0.5f);
            case Element.LIGHT:
                return new Color(1f, 1f, 0f);
            default:
                return new Color(1f, 0, 0);
        }
    }

    public Color GetRarityColor(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Normal:
                return new Color(0.6f,1f,0.7f);
            case Rarity.Rare:
                return new Color(0.8f, 0.5f, 1f);
            case Rarity.Legendary:
                return new Color(1f, 0.6f, 0f);
            default:
                return new Color(1f, 0, 0);
            case Rarity.Enemy:
                return new Color(0f, 1f, 0f);
        }
    }

    public Card GetCard() => card;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!enablePointToZoom) return;

        transform.localScale = Vector3.one * zoomRatio;
        Cursor.SetCursor(onZoomCursor, Vector3.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!enablePointToZoom) return;

        transform.localScale = Vector3.one * oriZoomRatio;
        Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!enablePointToZoom) return;
        
        var cdp = UIManager.Instance.ShowPage(GamePage.CardDetailPage).GetComponent<CardDetailPage>();
        cdp.Setup(card);
        cdp.OnClose.Subscribe(x =>
        {
            onClick?.Invoke();
        }).AddTo(this);
    }


}
