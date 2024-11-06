using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using DG.Tweening;

public class CardEffectList : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform content;

    ReactiveProperty<CardEffectSelectable> selectingCardEffect = new ReactiveProperty<CardEffectSelectable>();
    List<CardEffectSelectable> cardEffectSelectables = new List<CardEffectSelectable>();

    public ReactiveProperty<CardEffectSelectable> SelectingCardEffect => selectingCardEffect;

    private void Start()
    {
        selectingCardEffect.Subscribe(x =>
        {
            if(x == null)
            {
                ShowSelectingEffect(null);
                return;
            }

            ShowSelectingEffect(x.cardEffect);

        }).AddTo(this);
    }

    public void Setup(List<CardEffect> effects)
    {
        Reset();

        foreach(var e in effects)
        {
            var ces = Instantiate(prefab, content).GetComponent<CardEffectSelectable>();
            ces.Setup(e);
            ces.onClick.Subscribe(x => {
                if (x == null) return;
                SetSelectingEffect(x);
            }).AddTo(ces);
            cardEffectSelectables.Add(ces);
        }
    }

    public void SetSelectingEffect(CardEffect cardEffect)
    {
        if (cardEffect == null)
        {
            selectingCardEffect.Value = null;
            return;
        }

        if (selectingCardEffect.Value != null && selectingCardEffect.Value.cardEffect == cardEffect)
        {
            // Unselect
            selectingCardEffect.Value = null;
            return;
        }

        selectingCardEffect.Value = cardEffectSelectables.First(x => x.cardEffect == cardEffect);
    }

    void ShowSelectingEffect(CardEffect cardEffect)
    {
        foreach(var c in cardEffectSelectables)
        {
            c.SetSelecting(c.cardEffect == cardEffect);
        }
    }

    public void Reset()
    {
        foreach(Transform c in content)
        {
            Destroy(c.gameObject);
        }
    }

    public void Show()
    {
        var rt = GetComponent<RectTransform>();

        transform.DOLocalMoveX(Screen.width * 0.5f, 0.3f).SetEase(Ease.InOutBack);
    }

    public void Hide()
    {
        var rt = GetComponent<RectTransform>();

        transform.DOLocalMoveX(Screen.width * 0.5f + rt.sizeDelta.x, 0.3f).SetEase(Ease.InOutQuint);

        selectingCardEffect.Value = null;
    }
}
