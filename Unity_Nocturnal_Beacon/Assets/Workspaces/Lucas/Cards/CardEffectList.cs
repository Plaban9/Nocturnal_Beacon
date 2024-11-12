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

    ReactiveProperty<int> effectValue = new ReactiveProperty<int>(0);
    
    protected override void Start()
    {
        base.Start();

        effectValue.Subscribe(x =>
        {
            valueText.text = x.ToString();

        foreach (var c in selectables)
            {
                c.data.SetMainValue(x);
                c.SetCost(c.data.GetEffectCost());
                c.UpdateInfo();
            }
        }).AddTo(this);
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

    public override void SetSelecting(CardEffect data)
    {
        if (data == null)
        {
            selecting.Value = null;
            return;
        }

        if (selecting.Value != null && selecting.Value.data == data)
        {
            // Unselect
            selecting.Value = null;
            return;
        }

        selecting.Value = selectables.First(x => x.data.Id == data.Id);


    }

    public void AddEffectValue(int val)
    {
        effectValue.Value += val;
    }

    public void MinusEffectValue(int val)
    {
        effectValue.Value -= val;
    }

    public void SetEffectValue(int val)
    {
        effectValue.Value = val;
    }
}
