using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using DG.Tweening;

public class CardManaList : SelectionList<ManaCostSelectable, CardManaCost>
{
    public override void SetSelecting(CardManaCost data)
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

        selecting.Value = selectables.First(x => x.data.mana == data.mana);
    }


    public void Show()
    {
        var rt = GetComponent<RectTransform>();

        transform.DOLocalMoveX(-Screen.width * 0.5f, 0.3f).SetEase(Ease.InOutBack);
    }

    public void Hide()
    {
        if (transform.localPosition.x < -Screen.width * 0.5f) return;

        var rt = GetComponent<RectTransform>();

        transform.DOLocalMoveX(-Screen.width * 0.5f - rt.sizeDelta.x, 0.3f).SetEase(Ease.InOutQuint);
    }

}
