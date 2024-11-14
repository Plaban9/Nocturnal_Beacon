using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class NoticeBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI noticeText;
    [SerializeField] bool isSelfDestroy = false;
    [SerializeField] float duration = 2f;

    bool isShowing = false;

    public bool IsShowing() => isShowing;

    public void Show(string text, float showTime = 2f)
    {
        noticeText.text = text;
        isShowing = true;

        var sq = DOTween.Sequence();
        sq.Append(transform.DOMoveY(Screen.height + 100, 0f))
            .Append(transform.DOMoveY(Screen.height, 0.5f))
            .AppendInterval(showTime)
            .onComplete += Hide;
    }

    public void ShowOnce(string text, float showTime = 2f)
    {
        isSelfDestroy = true;

        Show(text, showTime);
    }

    void Hide()
    {
        Debug.Log("HIDING");
        transform.DOMoveY(Screen.height + 100f, 0.5f).onComplete += () =>
          {
              isShowing = false;

              if (isSelfDestroy)
                  Destroy(gameObject);
          };
    }
}
