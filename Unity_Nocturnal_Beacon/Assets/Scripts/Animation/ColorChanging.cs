using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DoTween.Animation
{
    public class ColorChanging : MonoBehaviour
    {
        [SerializeField] bool playAtStart = true;
        [SerializeField, ColorUsage(true)] Color initColor;
        [SerializeField, ColorUsage(true)] Color finalColor;
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
            main.DOColor(initColor, 0);

            seq = DOTween.Sequence();
            seq.Append(main.DOColor(finalColor, interval));
            //seq.Append(main.DOFade(minAlpha, interval));
            seq.SetLoops(count, LoopType.Yoyo);
        }
    }
}

