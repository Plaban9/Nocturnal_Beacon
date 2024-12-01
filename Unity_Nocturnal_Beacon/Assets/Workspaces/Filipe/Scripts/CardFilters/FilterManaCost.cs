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
    [SerializeField] ValueComparator comparison = ValueComparator.EqualTo;

    public override int Filter(Card card)
    {
        switch (comparison)
        {
            case ValueComparator.GreaterThan:
                return manaCost > card.GetManaCost()? 1 : 0;
            case ValueComparator.LessThan:
                return manaCost < card.GetManaCost() ? 1 : 0;
            case ValueComparator.EqualTo:
                return manaCost == card.GetManaCost() ? 1 : 0;
            case ValueComparator.NotEqualTo:
                return manaCost != card.GetManaCost() ? 1 : 0;
            case ValueComparator.GreaterThanOrEqualTo:
                return manaCost >= card.GetManaCost() ? 1 : 0;
            case ValueComparator.LessThanOrEqualTo:
                return manaCost <= card.GetManaCost() ? 1 : 0;
        }
        return 0;
    }

}

public enum ValueComparator
{
    GreaterThan,
    LessThan,
    EqualTo,
    NotEqualTo,
    GreaterThanOrEqualTo,
    LessThanOrEqualTo
}