using DG.Tweening;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Minimalist.Scene.Transition.Effect
{
    public class CrossFade : SceneTransition
    {
        [SerializeField] private CanvasGroup _crossFade;

        public override IEnumerator AnimateTransitionIn()
        {
            _crossFade.blocksRaycasts = true;
            var tweener = _crossFade.DOFade(1f, 1f);
            yield return tweener.WaitForCompletion();
        }

        public override IEnumerator AnimateTransitionOut()
        {
            _crossFade.blocksRaycasts = false;
            var tweener = _crossFade.DOFade(0f, 1f);
            yield return tweener.WaitForCompletion();
        }
    }
}
