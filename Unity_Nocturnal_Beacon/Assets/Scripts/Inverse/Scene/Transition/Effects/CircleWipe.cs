using DG.Tweening;

using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace Minimalist.Scene.Transition.Effect
{
    public class CircleWipe : SceneTransition
    {
        [SerializeField] private Image _circle;

        public override IEnumerator AnimateTransitionIn()
        {
            _circle.rectTransform.anchoredPosition = new Vector2(-_circle.rectTransform.sizeDelta.x, 0f);
            var tweener = _circle.rectTransform.DOAnchorPosX(0f, 1f);

            yield return tweener.WaitForCompletion();
        }

        public override IEnumerator AnimateTransitionOut()
        {
            var tweener = _circle.rectTransform.DOAnchorPosX(_circle.rectTransform.sizeDelta.x, 1f);

            yield return tweener.WaitForCompletion();
        }

    }
}
