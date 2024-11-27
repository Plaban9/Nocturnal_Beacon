using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class RandomDialog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] List<string> lines = new List<string>();
    [SerializeField] float fadeTime = 0f;

    CanvasGroup canvasGroup;
    bool isShowing = false;

    Sequence tween;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(float duration = -1)
    {
        var t = "";

        do
        {
            t = lines[Random.Range(0, lines.Count)];
        }
        while (dialogText.text.Equals(t));

        dialogText.text = t;

        if (isShowing) return;

        isShowing = true;

        if (duration != -1)
        {
            tween = DOTween.Sequence();
            tween.Append(canvasGroup.DOFade(1, fadeTime)).AppendInterval(duration).AppendCallback(Hide);
        }
        else
        {
            canvasGroup.DOFade(1, fadeTime);
        }
    }

    public void Hide()
    {
        tween?.Kill();
        isShowing = false;
        canvasGroup.DOFade(0, fadeTime);
    }
}
