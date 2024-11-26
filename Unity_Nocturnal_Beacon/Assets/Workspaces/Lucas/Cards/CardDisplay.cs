using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CardAttribute;
using UnityEngine.EventSystems;
using UniRx;
using System;
using DG.Tweening;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] bool enablePointToZoom = false;
    [SerializeField] float zoomRatio = 0.6f;
    [SerializeField] Texture2D onPointCursor;

    [SerializeField] protected Card card;

    [SerializeField] TMP_Text manaText;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text typeText;
    [SerializeField] TMP_Text descText;

    [SerializeField] Image element;
    [SerializeField] Image background;
    [SerializeField] Image title;
    [SerializeField] Image mainImg;

    [SerializeField] PriceTag priceTag;

    float oriZoomRatio = 1f;

    Action<CardDisplay> onClick;
    ReactiveProperty<CardDisplay> onPoint = new();

    public ReactiveProperty<CardDisplay> OnPoint() => onPoint;

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
        if (card == null) return; 
        manaText.text = card.GetManaCost() >= 0 ? card.GetManaCost().ToString() : "X";
        if (card.GetManaCost() > card.GetBaseManaCost())
        {
            manaText.color = new Color(1f, 0.2f, 0.2f);
        }
        else if (card.GetManaCost() < card.GetBaseManaCost())
        {
            manaText.color = new Color(0.2f, 1f, 0.2f);
        }
        else
        {
            manaText.color = new Color(0.2f, 0.2f, 1f);
        }
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

    public void SetZoomRatio(float ratio) => zoomRatio = ratio;

    public virtual void SetupForClickable(Card card, Action<CardDisplay> onClick, Texture2D onPointCursor = null)
    {
        enablePointToZoom = true;
        this.card = card;
        this.onClick = onClick;
        this.onPointCursor = onPointCursor;

        manaText.text = card.GetManaCost() >= 0 ? card.GetManaCost().ToString() : "X";
        titleText.text = card.name.ToString();
        typeText.text = card.cardType.ToString();
        descText.text = card.GetEffectDescStr();

        mainImg.sprite = card.sprite;
        SetColors(card);
        SetPrice(card.price);
    }

    public void EnablePriceTag(bool set) => priceTag.gameObject.SetActive(set);
    public void SetPrice(int price) => priceTag.SetPrice(price);
    public void RefreshPrice() => priceTag.Refresh();

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
            case Element.GHOST:
                return new Color(0.8f, 0.2f, 0.8f);
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
        if (!enablePointToZoom || IsHided()) return;

        onPoint.Value = this;
        transform.localScale = Vector3.one * zoomRatio;
        Cursor.SetCursor(onPointCursor, Vector3.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!enablePointToZoom || IsHided()) return;

        onPoint.Value = null;
        transform.localScale = Vector3.one * oriZoomRatio;
        Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsHided()) return;

        onClick?.Invoke(this);
    }

    public Subject<bool> FadeOut(float duration = 0.25f)
    {
        var finish = new Subject<bool>();
        var cg = GetComponent<CanvasGroup>();
        var sq = DOTween.Sequence();

        sq.Append(cg.DOFade(1, 0f))
            .Append(cg.DOFade(0, duration))
            .AppendCallback(() => { finish.OnNext(true); });

        return finish;
    }

    public Subject<bool> FadeIn(float duration = 0.25f)
    {
        var finish = new Subject<bool>();
        var cg = GetComponent<CanvasGroup>();
        var sq = DOTween.Sequence();

        sq.Append(cg.DOFade(0, 0f))
            .Append(cg.DOFade(1, duration))
            .AppendCallback(() => { finish.OnNext(true); });

        return finish;
    }

    public bool IsHided() => GetComponent<CanvasGroup>().alpha == 0 || !gameObject.activeSelf;
}
