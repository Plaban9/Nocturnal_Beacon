using CardAttribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ElementalTable
{
    public static ElementalEffectivity GetElementalAffinity(Element origin, Element target)
    {
        switch (target)
        {
            case Element.NONE:
                return ElementalEffectivity.NEUTRAL;
                
            case Element.EARTH:
                if(origin == Element.EARTH)
                {
                    return ElementalEffectivity.INEFFECTIVE; 
                }else if(origin == Element.FIRE)
                {
                    return ElementalEffectivity.VERY_EFFECTIVE;
                }else if(origin == Element.WIND)
                {
                    return ElementalEffectivity.RESIST;
                }
                break;
            case Element.WIND:
                if (origin == Element.WIND)
                {
                    return ElementalEffectivity.INEFFECTIVE;
                }
                else if (origin == Element.EARTH)
                {
                    return ElementalEffectivity.VERY_EFFECTIVE;
                }
                else if (origin == Element.WATER)
                {
                    return ElementalEffectivity.RESIST;
                }
                break;
            case Element.WATER:
                if (origin == Element.WATER)
                {
                    return ElementalEffectivity.INEFFECTIVE;
                }
                else if (origin == Element.WIND)
                {
                    return ElementalEffectivity.VERY_EFFECTIVE;
                }
                else if (origin == Element.FIRE)
                {
                    return ElementalEffectivity.RESIST;
                }
                break;
            case Element.FIRE:
                if (origin == Element.FIRE)
                {
                    return ElementalEffectivity.INEFFECTIVE;
                }
                else if (origin == Element.WATER)
                {
                    return ElementalEffectivity.VERY_EFFECTIVE;
                }
                else if (origin == Element.EARTH)
                {
                    return ElementalEffectivity.RESIST;
                }
                break;
            case Element.DARK:
                if (origin == Element.NONE)
                {
                    return ElementalEffectivity.NEUTRAL;
                }
                else if (origin == Element.LIGHT)
                {
                    return ElementalEffectivity.MAX_EFFECTIVE;
                }
                else if (origin == Element.DARK)
                {
                    return ElementalEffectivity.RESIST;
                }
                else
                {
                    return ElementalEffectivity.INEFFECTIVE;
                }
            case Element.LIGHT:
                if (origin == Element.NONE)
                {
                    return ElementalEffectivity.NEUTRAL;
                }
                else if (origin == Element.DARK)
                {
                    return ElementalEffectivity.MAX_EFFECTIVE;
                }
                else if (origin == Element.LIGHT)
                {
                    return ElementalEffectivity.RESIST;
                }
                else
                {
                    return ElementalEffectivity.INEFFECTIVE;
                }
            case Element.GHOST:
                if(origin == Element.NONE)
                {
                    return ElementalEffectivity.UNAFFECTED;
                }
                else
                {
                    return ElementalEffectivity.MAX_EFFECTIVE;
                }
        }
        return ElementalEffectivity.NEUTRAL;
    }

    public static float GetEffectivityMultiplier(ElementalEffectivity effectivity)
    {
        switch (effectivity)
        {
            case ElementalEffectivity.UNAFFECTED:
                return 0.1f;
            case ElementalEffectivity.RESIST:
                return 0.5f;
            case ElementalEffectivity.INEFFECTIVE:
                return 0.75f;
            case ElementalEffectivity.NEUTRAL:
                return 1.0f;
            case ElementalEffectivity.EFFECTIVE:
                return 1.25f;
            case ElementalEffectivity.VERY_EFFECTIVE:
                return 1.5f;
            case ElementalEffectivity.MAX_EFFECTIVE:
                return 2.0f;
        }
        return -1f;
    }
}
