using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class FilterCardRarity : CardFilter
{
    [SerializeField] CardAttribute.Rarity rarity;
    [SerializeField] CardValueComparator comparator;
    public override int Filter(Card card)
    {
        switch (comparator)
        {
            case CardValueComparator.GreaterThan:
                return rarity > card.rarity ? 1 : 0;
            case CardValueComparator.LessThan:
                return rarity < card.rarity ? 1 : 0;
            case CardValueComparator.EqualTo:
                return rarity == card.rarity ? 1 : 0;
            case CardValueComparator.NotEqualTo:
                return rarity != card.rarity ? 1 : 0;
            case CardValueComparator.GreaterThanOrEqualTo:
                return rarity >= card.rarity ? 1 : 0;
            case CardValueComparator.LessThanOrEqualTo:
                return rarity <= card.rarity ? 1 : 0;
        }
        return 0;
    }

}
