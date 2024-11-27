using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_ElementalChangeNeutral : BattleStatusEffect
{
    public override Element OnGetElement(Element incoming, bool additive)
    {
        if (additive)
        {
            return incoming + (int) Element.NONE;
        }
        else
        {
            return Element.NONE;
        }
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
[Serializable]
public class StatusEffect_ElementalChangeFire : BattleStatusEffect
{
    public override Element OnGetElement(Element incoming, bool additive)
    {
        if (additive)
        {
            return incoming + (int)Element.FIRE;
        }
        else
        {
            return Element.FIRE;
        }
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
[Serializable]
public class StatusEffect_ElementalChangeEarth : BattleStatusEffect
{
    public override Element OnGetElement(Element incoming, bool additive)
    {
        if (additive)
        {
            return incoming + (int)Element.EARTH;
        }
        else
        {
            return Element.EARTH;
        }
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
[Serializable]
public class StatusEffect_ElementalChangeWater : BattleStatusEffect
{
    public override Element OnGetElement(Element incoming, bool additive)
    {
        if (additive)
        {
            return incoming + (int)Element.WATER;
        }
        else
        {
            return Element.WATER;
        }
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
[Serializable]
public class StatusEffect_ElementalChangeWind : BattleStatusEffect
{
    public override Element OnGetElement(Element incoming, bool additive)
    {
        if (additive)
        {
            return incoming + (int)Element.WIND;
        }
        else
        {
            return Element.WIND;
        }
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
[Serializable]
public class StatusEffect_ElementalChangeLight : BattleStatusEffect
{
    public override Element OnGetElement(Element incoming, bool additive)
    {
        if (additive)
        {
            return incoming + (int)Element.LIGHT;
        }
        else
        {
            return Element.LIGHT;
        }
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
[Serializable]
public class StatusEffect_ElementalChangeDark : BattleStatusEffect
{
    public override Element OnGetElement(Element incoming, bool additive)
    {
        if (additive)
        {
            return incoming + (int)Element.DARK;
        }
        else
        {
            return Element.DARK;
        }
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
[Serializable]
public class StatusEffect_ElementalChangeGhost : BattleStatusEffect
{
    public override Element OnGetElement(Element incoming, bool additive)
    {
        if (additive)
        {
            return incoming + (int)Element.GHOST;
        }
        else
        {
            return Element.GHOST;
        }
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
