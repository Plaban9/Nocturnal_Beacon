using CardAttribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ElementalTable
{
    public static float GetElementalAffinity(Element origin, Element target)
    {
        switch (target)
        {
            case Element.NONE:
                return 1f;
                
            case Element.EARTH:
                if(origin == Element.EARTH)
                {
                    return 0.75f; 
                }else if(origin == Element.FIRE)
                {
                    return 1.5f;
                }else if(origin == Element.WIND)
                {
                    return 0.5f;
                }
                break;
            case Element.WIND:
                if (origin == Element.WIND)
                {
                    return 0.75f;
                }
                else if (origin == Element.EARTH)
                {
                    return 1.5f;
                }
                else if (origin == Element.WATER)
                {
                    return 0.5f;
                }
                break;
            case Element.WATER:
                if (origin == Element.WATER)
                {
                    return 0.75f;
                }
                else if (origin == Element.WIND)
                {
                    return 1.5f;
                }
                else if (origin == Element.FIRE)
                {
                    return 0.5f;
                }
                break;
            case Element.FIRE:
                if (origin == Element.FIRE)
                {
                    return 0.75f;
                }
                else if (origin == Element.WATER)
                {
                    return 1.5f;
                }
                else if (origin == Element.EARTH)
                {
                    return 0.5f;
                }
                break;
            case Element.DARK:
                if (origin == Element.NONE)
                {
                    return 1f;
                }
                else if (origin == Element.LIGHT)
                {
                    return 2f;
                }
                else if (origin == Element.DARK)
                {
                    return 0.5f;
                }
                else
                {
                    return 0.75f;
                }
            case Element.LIGHT:
                if (origin == Element.NONE)
                {
                    return 1f;
                }
                else if (origin == Element.DARK)
                {
                    return 2f;
                }
                else if (origin == Element.LIGHT)
                {
                    return 0.5f;
                }
                else
                {
                    return 0.75f;
                }
        }
        return 1f;
    }

}
