using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class FilterCardType : CardFilter
{
    [SerializeField] CardAttribute.CardType type;
    [SerializeField] CardTypeComparison comparison;
    public override int Filter(Card card)
    {
        switch (comparison)
        {
            case CardTypeComparison.IS:
                return type == card.cardType ? 1 : 0;
            case CardTypeComparison.ISNOT:
                return type == card.cardType ? 0 : 1;
        }
        return 0;
    }

}

public enum CardTypeComparison
{
    IS,
    ISNOT
}