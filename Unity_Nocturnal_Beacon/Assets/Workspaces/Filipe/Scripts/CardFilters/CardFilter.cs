    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class CardFilter
{   
    public abstract int Filter(Card card);
}
public enum Containment
{
    Have,
    DontHave
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