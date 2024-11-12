using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UniRx;

namespace DoTween.Animation
{
    public enum Direction
    {
        Vertical,
        Horizontal
    }
    public class ScrollingNumber : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] TMPro.TextMeshProUGUI currentNumberText;
        [SerializeField] TMPro.TextMeshProUGUI nextNumberText;
        [SerializeField] GameObject addButton;
        [SerializeField] GameObject minusButton;

        [Header("Parameters")]
        [SerializeField] int startNum = 1;
        [SerializeField] int minNum = 0;
        [SerializeField] int maxNum = 99;
        [SerializeField] Direction direction = Direction.Vertical;
        [SerializeField] float interval = 0.5f;
        [SerializeField] float distance = 100f;

        ReactiveProperty<int> curNum = new ReactiveProperty<int>();
        int targetNum;

        Vector2 oriPos;
        RectTransform curNumRect;
        RectTransform nextNumRect;
        Tween curNumTween;
        Tween nextNumTween;
        // Start is called before the first frame update
        void Start()
        {
            Setup(startNum, minNum, maxNum);
        }

        public void Setup(int startNum, int minNum, int maxNum)
        {
            this.startNum = startNum;
            this.minNum = minNum;
            this.maxNum = maxNum;
            targetNum = startNum;

            currentNumberText.text = startNum.ToString();

            curNumRect = currentNumberText.gameObject.GetComponent<RectTransform>();
            nextNumRect = nextNumberText.gameObject.GetComponent<RectTransform>();
            oriPos = curNumRect.localPosition;
        }

        public void Add(int v)
        {
            curNum.Value = targetNum;
            targetNum = curNum.Value + v;
            currentNumberText.text = curNum.Value.ToString();
            nextNumberText.text = targetNum.ToString();

            curNumTween.Kill();
            nextNumTween.Kill();

            if (direction == Direction.Horizontal)
            {
                
            }
            else
            {
                DoUpScroll();
            }
        }

        public void Minus(int v)
        {
            curNum.Value = targetNum;
            targetNum = curNum.Value - v;
            currentNumberText.text = curNum.Value.ToString();
            nextNumberText.text = targetNum.ToString();

            curNumTween.Kill();
            nextNumTween.Kill();

            if (direction == Direction.Horizontal)
            {

            }
            else
            {
                DoDownScroll();
            }
        }

        public void SetVal(int v)
        {
            if(v > targetNum)
            {
                Add(targetNum - v);
            }
            else
            {
                Minus(targetNum - v);
            }
        }

        public void SetVal(string s)
        {
            curNum.Value = targetNum;
            currentNumberText.text = curNum.Value.ToString();
            nextNumberText.text = s.ToString();

            curNumTween.Kill();
            nextNumTween.Kill();

            if (direction == Direction.Horizontal)
            {

            }
            else
            {
                DoUpScroll();
            }
        }

        void DoUpScroll()
        {
            curNumRect.localPosition = oriPos;
            nextNumRect.localPosition = oriPos - new Vector2(0, distance);

            curNumTween = curNumRect.DOLocalMoveY(distance, interval);
            nextNumTween = nextNumRect.DOLocalMoveY(0, interval);

            nextNumTween.onComplete += () => { curNum.Value = targetNum; };
        }

        void DoDownScroll()
        {
            curNumRect.localPosition = oriPos;
            nextNumRect.localPosition = oriPos + new Vector2(0, distance);

            curNumTween = curNumRect.DOLocalMoveY(-distance, interval);
            nextNumTween = nextNumRect.DOLocalMoveY(0, interval);

            nextNumTween.onComplete += () => { curNum.Value = targetNum; };
        }
    }

}
