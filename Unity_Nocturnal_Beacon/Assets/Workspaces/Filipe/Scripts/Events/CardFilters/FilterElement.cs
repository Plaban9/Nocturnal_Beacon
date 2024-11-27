using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class FilterElement : CardFilter
{
    [SerializeField] Element element = Element.NONE;
    public override int Filter(Card card)
    {
        if (element == card.element) return 1;
        return 0;
    }

}
