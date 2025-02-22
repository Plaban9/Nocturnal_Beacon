using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipCard : MonoBehaviour
{

    [Header("Assets")]
    [SerializeField] CardDisplay _cardDisplay;

    public void SetCard(Card card)
    {
        _cardDisplay.Setup(card); 
    }
}
