using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_ResistFire : BattleStatusEffect
{
    public override ElementalEffectivity OnGetElementalAffinity(Element incoming, ElementalEffectivity previousEffectivity)
    {
        if (incoming == Element.FIRE) return previousEffectivity - _intensity;
        return base.OnGetElementalAffinity(incoming, previousEffectivity);
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
[Serializable]
public class StatusEffect_ResistWater : BattleStatusEffect
{
    public override ElementalEffectivity OnGetElementalAffinity(Element incoming, ElementalEffectivity previousEffectivity)
    {
        if (incoming == Element.WATER) return previousEffectivity - _intensity;
        return base.OnGetElementalAffinity(incoming, previousEffectivity);
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
[Serializable]
public class StatusEffect_ResistEarth : BattleStatusEffect
{
    public override ElementalEffectivity OnGetElementalAffinity(Element incoming, ElementalEffectivity previousEffectivity)
    {
        if (incoming == Element.EARTH) return previousEffectivity - _intensity;
        return base.OnGetElementalAffinity(incoming, previousEffectivity);
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
[Serializable]
public class StatusEffect_ResistWind : BattleStatusEffect
{
    public override ElementalEffectivity OnGetElementalAffinity(Element incoming, ElementalEffectivity previousEffectivity)
    {
        if (incoming == Element.WIND) return previousEffectivity - _intensity;
        return base.OnGetElementalAffinity(incoming, previousEffectivity);
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
[Serializable]
public class StatusEffect_ResistNeutral : BattleStatusEffect
{
    public override ElementalEffectivity OnGetElementalAffinity(Element incoming, ElementalEffectivity previousEffectivity)
    {
        if (incoming == Element.NONE) return previousEffectivity - _intensity;
        return base.OnGetElementalAffinity(incoming, previousEffectivity);
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}

[Serializable]
public class StatusEffect_ResistLight : BattleStatusEffect
{
    public override ElementalEffectivity OnGetElementalAffinity(Element incoming, ElementalEffectivity previousEffectivity)
    {
        if (incoming == Element.LIGHT) return previousEffectivity - _intensity;
        return base.OnGetElementalAffinity(incoming, previousEffectivity);
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
[Serializable]
public class StatusEffect_ResistDark : BattleStatusEffect
{
    public override ElementalEffectivity OnGetElementalAffinity(Element incoming, ElementalEffectivity previousEffectivity)
    {
        if (incoming == Element.DARK) return previousEffectivity - _intensity;
        return base.OnGetElementalAffinity(incoming, previousEffectivity);
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
[Serializable]
public class StatusEffect_ResistGhost : BattleStatusEffect
{
    public override ElementalEffectivity OnGetElementalAffinity(Element incoming, ElementalEffectivity previousEffectivity)
    {
        if (incoming == Element.GHOST) return previousEffectivity - _intensity;
        return base.OnGetElementalAffinity(incoming, previousEffectivity);
    }
    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}