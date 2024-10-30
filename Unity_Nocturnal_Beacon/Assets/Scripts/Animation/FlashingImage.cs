using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DoTween.Animation
{
    public class FlashingImage : MonoBehaviour
    {
        [SerializeField] bool playAtStart = true;
        [SerializeField, Range(0, 1)] float minAlpha;
        [SerializeField, Range(0, 1)] float maxAlpha;
        [SerializeField] float interval;
        [SerializeField, Tooltip("-1 = Looping")] int count = -1;

        Image main;
        Sequence seq;

        private void Awake()
        {

        }
        // Start is called before the first frame update
        void Start()
        {
            if (TryGetComponent(out main) && playAtStart)
            {
                DoFlashing();
            }
        }

        public void DoFlashing()
        {
            if (main == null) return;

            seq = DOTween.Sequence();
            seq.Append(main.DOFade(maxAlpha, interval));
            seq.Append(main.DOFade(minAlpha, interval));
            seq.SetLoops(count);
        }
    }
}

