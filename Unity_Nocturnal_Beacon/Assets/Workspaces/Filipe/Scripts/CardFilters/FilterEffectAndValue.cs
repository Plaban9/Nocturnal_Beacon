using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static WithManaDoEffect;
[Serializable]
public class FilterEffectAndValue : CardFilter
{
    [SerializeReference, SubclassSelector] CardEffect filteredEffect;
    [SerializeField] Containment comparison;
    [SerializeField] int value;
    [SerializeField] ValueComparator valueComparator;
    [SerializeField] CardVariable variable;
    public override int Filter(Card card)
    {
        var type = filteredEffect.GetType();
        foreach (CardEffect ce in card.effects)
        {
            Debug.Log($"{type} == {ce.GetType()} => {type == ce.GetType()}");
        }
        switch (comparison)
        {
            case Containment.Have:
                return card.effects.Any(it => it.GetType() == type) ? 1 : 0;
            case Containment.DontHave:
                return card.effects.Any(it => it.GetType() == type) ? 0 : 1;
        }
        return 0;
    }

    private int ValueCompare(CardEffect ce)
    {
        switch (variable)
        {
            case CardVariable.VAL1:
                return Compare(ce.GetValue(CardVariable.VAL1));
            case CardVariable.VAL2:
                return Compare(ce.GetValue(CardVariable.VAL2));
            case CardVariable.VAL3:
                return Compare(ce.GetValue(CardVariable.VAL3));
            case CardVariable.VAL4:
                return Compare(ce.GetValue(CardVariable.VAL4));
            default:
                return Compare(ce.GetValue(CardVariable.VAL1)); 
        }
    }

    private int Compare(int a)
    {
        switch (valueComparator)
        {
            case ValueComparator.GreaterThan:
                return value > a ? 1 : 0;
            case ValueComparator.LessThan:
                return value < a ? 1 : 0;
            case ValueComparator.EqualTo:
                return value == a ? 1 : 0;
            case ValueComparator.NotEqualTo:
                return value != a ? 1 : 0;
            case ValueComparator.GreaterThanOrEqualTo:
                return value >= a ? 1 : 0;
            case ValueComparator.LessThanOrEqualTo:
                return value <= a ? 1 : 0;
        }
        return 0;
    }
}



