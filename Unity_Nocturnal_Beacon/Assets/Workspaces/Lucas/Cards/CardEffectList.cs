using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using DG.Tweening;

public class CardEffectList : SelectionList<CardEffectSelectable, CardEffect>
{
    [Header("Effect value")]
    [SerializeField] TMPro.TextMeshProUGUI valueText;

    ReactiveProperty<int> effectValue = new ReactiveProperty<int>(1);

    public ReactiveProperty<int> EffectValue() => effectValue;

    bool isLocked = false;
    bool isClosed = false;

    public bool IsLocked() => isLocked;
    public bool IsClosed() => isClosed;

    protected override void Start()
    {
        base.Start();

        effectValue.Subscribe(x =>
        {
            valueText.text = x.ToString();

            foreach (var c in selectables)
            {
                c.data.SetMainValue(x);
                c.SetCost(CardEffectCostManager.Instance.GetEffectCost(c.data));
                c.UpdateInfo();
            }
        }).AddTo(this);
    }

    public void Show()
    {
        var rt = GetComponent<RectTransform>();

        transform.DOLocalMoveX(Screen.width * 0.5f, 0.3f).SetEase(Ease.InOutBack);

        isClosed = false;
    }

    public void Hide()
    {
        if (transform.localPosition.x > Screen.width * 0.5f) return;

        var rt = GetComponent<RectTransform>();

        transform.DOLocalMoveX(Screen.width * 0.5f + rt.sizeDelta.x, 0.3f).SetEase(Ease.InOutQuint);

        isClosed = true;
    }

    public void SetSelectingWithLock(CardEffect data)
    {
        isLocked = true;

        if (selecting.Value != null && data.Compare(selecting.Value.data))
            ShowSelecting(selecting.Value);
        else
            selecting.Value = selectables.First(x => x.data.Compare(data));
    }

    public void SetDefaultSelecting(CardEffect data)
    {
        isLocked = false;

        if (data == null)
        {
            selecting.Value = null;
            effectValue.Value = 1;
            return;
        }

        if (selecting.Value != null && selecting.Value.data.Compare(data))
        {
            ShowSelecting(selecting.Value);
        }

        selecting.Value = selectables.First(x => x.data.Compare(data));
    }

    public override void SetSelecting(CardEffect data)
    {
        if (isLocked) return;

        if (data == null)
        {
            selecting.Value = null;
            effectValue.Value = 1;
            return;
        }

        if (selecting.Value != null && selecting.Value.data == data)
        {
            // Unselect
            selecting.Value = null;
            return;
        }

        selecting.Value = selectables.First(x => x.data.Compare(data));
    }

    public override void ShowSelecting(CardEffectSelectable selecting)
    {
        if(!isLocked)
            base.ShowSelecting(selecting);
        else
        {
            foreach (var c in selectables)
            {
                if (c == selecting)
                    c.SetSelecting(true);
                else
                {
                    c.SetLocked(true);
                }
            }
        }
    }

    public void SetLockedEffect(List<CardEffect> cardEffects)
    {
        foreach(var e in selectables)
        {
            e.SetLocked(cardEffects.Exists(x => x.Compare(e.data)));
        }
    }
    public override void Reset()
    {
        effectValue.Value = 1;
        isLocked = false;

        base.Reset();
    }

    public void AddEffectValue(int val)
    {
        effectValue.Value += val;
    }

    public void MinusEffectValue(int val)
    {
        if (effectValue.Value - val <= 0) return;

        effectValue.Value -= val;
    }

    public void SetEffectValue(int val)
    {
        effectValue.Value = Mathf.Abs(val);
    }
}
