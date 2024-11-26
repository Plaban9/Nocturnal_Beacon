using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopAnimator : MonoBehaviour
{
    [SerializeField] RectTransform background;
    [SerializeField] RectTransform player;
    [SerializeField] RectTransform merchant;

    [SerializeField] float playerWalkSpeed = 300f;
    [SerializeField] float playerInitPosX = -1400f;
    [SerializeField] float backgroundInitPosX = -400f;
    Sequence playerWalk;

    [ContextMenu("Start Animate")]
    public void StartAnimate()
    {
        var curPos = player.localPosition;

        playerWalk = DOTween.Sequence();
        playerWalk.Append(player.DOLocalMoveY(curPos.y + 10, 0.2f)).Append(player.DOLocalMoveY(curPos.y, 0.2f)).SetLoops(-1);

        player.DOLocalMoveX(playerInitPosX, Mathf.Abs(playerInitPosX - curPos.x) / playerWalkSpeed).From().SetEase(Ease.Linear).onComplete += AnimateBackground;
    }

    void AnimateBackground()
    {
        var curPos = background.localPosition;

        background.DOLocalMoveX(backgroundInitPosX, Mathf.Abs(backgroundInitPosX - curPos.x) / playerWalkSpeed).SetEase(Ease.Linear).onComplete += () =>
        {
            playerWalk.Kill();
        };
    }

    public void SkipAnimate()
    {
        player.DOLocalMoveX(playerInitPosX, 0).From().SetEase(Ease.Linear);
        background.DOLocalMoveX(backgroundInitPosX, 0).SetEase(Ease.Linear);
    }
}
