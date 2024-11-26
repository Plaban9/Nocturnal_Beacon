using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public class FilterManaCost : CardFilter
{
    [SerializeField] int manaCost = 0;
    [SerializeField] CardValueComparator comparison = CardValueComparator.EqualTo;

    public override int Filter(Card card)
    {
        switch (comparison)
        {
            case CardValueComparator.GreaterThan:
                return manaCost > card.GetManaCost()? 1 : 0;
            case CardValueComparator.LessThan:
                return manaCost < card.GetManaCost() ? 1 : 0;
            case CardValueComparator.EqualTo:
                return manaCost == card.GetManaCost() ? 1 : 0;
            case CardValueComparator.NotEqualTo:
                return manaCost != card.GetManaCost() ? 1 : 0;
            case CardValueComparator.GreaterThanOrEqualTo:
                return manaCost >= card.GetManaCost() ? 1 : 0;
            case CardValueComparator.LessThanOrEqualTo:
                return manaCost <= card.GetManaCost() ? 1 : 0;
        }
        return 0;
    }

}

public enum CardValueComparator
{
    GreaterThan,
    LessThan,
    EqualTo,
    NotEqualTo,
    GreaterThanOrEqualTo,
    LessThanOrEqualTo
}