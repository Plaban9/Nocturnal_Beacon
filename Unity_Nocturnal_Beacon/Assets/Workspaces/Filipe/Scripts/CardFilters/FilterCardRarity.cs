using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class FilterCardRarity : CardFilter
{
    [SerializeField] CardAttribute.Rarity rarity;
    [SerializeField] ValueComparator comparator;
    public override int Filter(Card card)
    {
        switch (comparator)
        {
            case ValueComparator.GreaterThan:
                return rarity > card.rarity ? 1 : 0;
            case ValueComparator.LessThan:
                return rarity < card.rarity ? 1 : 0;
            case ValueComparator.EqualTo:
                return rarity == card.rarity ? 1 : 0;
            case ValueComparator.NotEqualTo:
                return rarity != card.rarity ? 1 : 0;
            case ValueComparator.GreaterThanOrEqualTo:
                return rarity >= card.rarity ? 1 : 0;
            case ValueComparator.LessThanOrEqualTo:
                return rarity <= card.rarity ? 1 : 0;
        }
        return 0;
    }

}
