using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

namespace DoTween.Animation
{
    public class BreathingObject : MonoBehaviour
    {
        [SerializeField] bool playAtStart = true;
        [SerializeField, Min(0)] float minSize;
        [SerializeField, Min(0)] float maxSize;
        [SerializeField] float interval;
        [SerializeField, Tooltip("-1 = Looping")] int count = -1;

        RectTransform main;
        Sequence seq;

        // Start is called before the first frame update
        void Start()
        {
            if(TryGetComponent(out main) && playAtStart)
            {
                DoBreathing();
            }
        }

        public void DoBreathing()
        {
            if (main == null) return;

            seq = DOTween.Sequence();
            seq.Append(main.DOScale(maxSize, interval)).Append(main.DOScale(minSize, interval)).SetLoops(count);
        }

        public void Reset()
        {
            seq.Pause();
            main.DOScale(1, 0);
        }
    }

}
