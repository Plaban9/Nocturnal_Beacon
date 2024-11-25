using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public class FilterEffect : MapEventCardFilter
{
    [SerializeReference, SubclassSelector] CardEffect filteredEffect;
    [SerializeField] Containment comparison;

    public override int GetOutcomeSuccess(Card card)
    {
        var type = filteredEffect.GetType();
        foreach(CardEffect ce in card.effects)
        {
            Debug.Log($"{type} == {ce.GetType()} => {type == ce.GetType()}");
        }
        switch (comparison)
        {
            case Containment.Have:
                return card.effects.Any(it => it.GetType() == type ) ? 1 : 0;
            case Containment.DontHave:
                return card.effects.Any(it => it.GetType() == type )? 0 : 1;
        }
        return 0;
    }

}

public enum Containment
{
    Have,
    DontHave
}