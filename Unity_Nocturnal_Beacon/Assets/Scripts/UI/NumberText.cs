using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberText : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI numberText;
    [SerializeField] float duration = 0.5f;
    [SerializeField] bool enableUpdateNumber = true;
    [SerializeField] bool enableChangeColor = true;

    [SerializeField] Color positiveColor;
    [SerializeField] Color negativeColor;

    int curValue;
    int targetValue;
    float timer = 0f;

    private void Awake()
    {
        numberText = GetComponent<TMPro.TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!enableUpdateNumber) return;

        if(curValue != targetValue)
        {
            timer += Time.deltaTime;

            curValue = (int)Mathf.Lerp(curValue, targetValue, timer / duration);

            if (curValue == targetValue || Mathf.Abs(curValue - targetValue) < 1)
            {
                curValue = targetValue;
                timer = 0; 
            }

            SetText();
        }
    }

    public void SetInitValue(int val)
    {
        curValue = val;
        targetValue = val;

        SetText();
    }

    public void SetTargetValue(int val)
    {
        targetValue = val;
        timer = 0;

        if (!enableUpdateNumber)
        {
            curValue = val;
            SetText();
        }
    }

    public void SetTargetValueWithDiff(int val)
    {
        targetValue = val;
        curValue = val;

        if(val >= 0)
        {
            numberText.text = $"(+{val})";
        }
        else
        {
            numberText.text = $"({val})";
        }


        if (!enableChangeColor) return;

        if (curValue >= 0)
            numberText.color = positiveColor;
        else
            numberText.color = negativeColor;
    }

    void SetText()
    {
        numberText.text = curValue.ToString();

        if (!enableChangeColor) return;

        if (curValue >= 0)
            numberText.color = positiveColor;
        else
            numberText.color = negativeColor;
    }
}
