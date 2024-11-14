using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectCost : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI costText;

    public bool IsHideCost { get; private set; }
    public int Cost { get; private set; }

    public void SetCost(int cost)
    {
        Cost = cost;

        if(IsHideCost)
        {
            costText.text = $"<color=#FFFFFF>--</color>";
        }
        else
        {
            if (cost > 0)
            {
                costText.text = $"<color=#FF5F00>-{cost}</color>";
            }
            else if (cost < 0)
            {
                costText.text = $"<color=#A9FF00>+{Mathf.Abs(cost)}</color>";
            }
            else
            {
                costText.text = $"<color=#FFFFFF>{cost}</color>";
            }
        }
    }

    public void SetHideCost(bool set)
    {
        IsHideCost = set;
        SetCost(Cost);
    }
}
