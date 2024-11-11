using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using DG.Tweening;

public class CardEffectList : MonoBehaviour
{
    [Header("Effect value")]
    [SerializeField] TMPro.TextMeshProUGUI valueText;

    [Header("Scrollview")]
    [SerializeField] GameObject prefab;
    [SerializeField] Transform content;

    ReactiveProperty<CardEffectSelectable> selectingCardEffect = new ReactiveProperty<CardEffectSelectable>();
    List<CardEffectSelectable> cardEffectSelectables = new List<CardEffectSelectable>();

    ReactiveProperty<int> effectValue = new ReactiveProperty<int>(0);

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

        effectValue.Subscribe(x =>
        {
            valueText.text = x.ToString();
            foreach(var c in cardEffectSelectables)
            {
                c.cardEffect.SetMainValue(x);
                c.UpdateInfo();
            }
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
        //foreach(Transform c in content)
        //{
        //    Destroy(c.gameObject);
        //}

        selectingCardEffect.Value = null;

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

        Reset();
    }

    public void AddEffectValue(int val)
    {
        effectValue.Value += val;
    }

    public void MinusEffectValue(int val)
    {
        effectValue.Value -= val;
    }
}
