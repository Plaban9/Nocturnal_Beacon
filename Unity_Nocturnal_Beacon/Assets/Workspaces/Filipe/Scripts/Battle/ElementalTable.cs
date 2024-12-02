using CardAttribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class ElementalTable
{
    public static ElementalEffectivity GetElementalAffinity(Element origin, Element target)
    {
        switch (target)
        {
            case Element.NONE:
                if (origin == Element.GHOST)
                {
                    return ElementalEffectivity.UNAFFECTED;
                }
                return ElementalEffectivity.NEUTRAL;

            case Element.EARTH:
                if (origin == Element.EARTH)
                {
                    return ElementalEffectivity.INEFFECTIVE;
                }
                else if (origin == Element.FIRE)
                {
                    return ElementalEffectivity.VERY_EFFECTIVE;
                }
                else if (origin == Element.WIND)
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
                if (origin == Element.NONE)
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

    public static Sprite GetElementalIcon(Element element)
    {
        Sprite[] elementSpritesheet = Resources.LoadAll<Sprite>("Sprites/Icons/elements");
        switch (element)
        {
            case Element.NONE:
                return elementSpritesheet[6];
            case Element.FIRE:
                return elementSpritesheet[2];
            case Element.WIND:
                return elementSpritesheet[8];
            case Element.WATER:
                return elementSpritesheet[7];
            case Element.EARTH:
                return elementSpritesheet[1];
            case Element.DARK:
                return elementSpritesheet[0];
            case Element.LIGHT:
                return elementSpritesheet[4];
            case Element.GHOST:
                return elementSpritesheet[3];
            default:
                return Resources.Load<Sprite>("Sprites/Icons/ELENEUTRAL");
        }
    }

    public static string GetAffinityText(ElementalEffectivity effectivity)
    {
        switch (effectivity)
        {
            case ElementalEffectivity.UNAFFECTED:
                return "IMPERVIOUS";
                break;
            case ElementalEffectivity.RESIST:
                return  "RESISTS";
                break;
            case ElementalEffectivity.INEFFECTIVE:

                return "INEFFECTIVE";
                break;
            case ElementalEffectivity.NEUTRAL:

                return "NEUTRAL";
                break;
            case ElementalEffectivity.EFFECTIVE:
                return "EFFECTIVE";

                break;
            case ElementalEffectivity.VERY_EFFECTIVE:
                return "VRY EFFECTIVE";
                break;
            case ElementalEffectivity.MAX_EFFECTIVE:
                return "MAX EFFECTIVE";
                break;
        }
        return "unavailable";
    }

    public static Color GetAffinityColor(ElementalEffectivity effectivity)
    {
        switch (effectivity)
        {
            case ElementalEffectivity.UNAFFECTED:
                return new Color(0.8f, 0.2f, 0.8f);
                break;
            case ElementalEffectivity.RESIST:
                return new Color(0.5f, 0.5f, 0.9f);
                break;
            case ElementalEffectivity.INEFFECTIVE:
                return new Color(0.5f, 0.3f, 0.7f);
                break;
            case ElementalEffectivity.NEUTRAL:
                return new Color(0.5f, 0.5f, 0.5f);
                break;
            case ElementalEffectivity.EFFECTIVE:
                return new Color(0.7f, 0.65f, 0.64f);
                break;
            case ElementalEffectivity.VERY_EFFECTIVE:
                return new Color(0.8f, 0.35f, 0.38f);
                break;
            case ElementalEffectivity.MAX_EFFECTIVE:
                return new Color(0.86f, 0.3f, 0.3f);
                break;
        }
        return Color.white;
    }

}
