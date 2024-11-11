using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardAttribute
{
    [Serializable]
    public enum CardType
    {
        Attack,
        Skill,
        Curse,
        Status
    }

    [Serializable]
    public enum Rarity
    {
        Normal = 0,
        Rare, 
        Legendary
    };

    [Serializable]
    public enum Effect
    {
        DealDamage = 0,
        GainShield,
        GainHealth,
        GainStrength,
        DrawCard,
        E

    };

    [Serializable]
    public enum EffectTarget
    {
        Self = 0,
        OpponentSingle,
        OpponentRandom,
        OpponentAll,
        Both
    }

    [Serializable]
    public enum EffectTargetAmount
    {
        Designated = 0,
        Random,
        All
    };

    [Serializable]
    public enum AppMechanic
    {
        OnUse = 0,
        BeforeCast,
        OnCast,
        AfterCast
    }

    [Serializable]
    public enum StatusEffect
    {
        /* =============== Buffs =============== */
        Strength = 0,   // Increases/Decreases attack damage by X.
        Dexterity,      // Increases/Decreases Block gained from cards by X.
        Throns,         // When attacked, deals X damage back.
        Regenerate,     // At the end of its turn, heals X HP.
        Buffer,         // Prevent the next X times you would lose HP.
        Artifact,       // Negates X debuffs.
        DrawCard,       // 	Draw X additional cards next turn.

        /* ============== DeBuffs ============== */
        Poision,        // At the beginning of its turn, the target loses X HP and 1 stack of Poison.
        Vulerable,      // Target takes 50% more damage from attacks.
        Weak,           // Target deals 25% less attack damage.
        NoDraw,         // You may not draw any more cards this turn.
        Frail,          // Block gained from cards is reduced by 25%.
        Confused        // The costs of your cards are randomized on draw, from 0 to 3.
    }

    [Serializable]
    public enum StatusStacks
    {
        No = 0,         // Unstackable.
        Intensity,      // effect depends on the amount of stack.
        Duration        // Lost 1 stack every turn.
    }

    [Serializable]
    public enum Element
    {
        NONE = 0,
        EARTH,
        WIND,
        WATER,
        FIRE,
        DARK,
        LIGHT
    }

}

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
[Serializable]
public class Card : ScriptableObject
{
    public int id;
    [HideInInspector] public int uId;   // unique Id
    public new string name;
    public Rarity rarity;
    public CardType cardType;
    public int manaCost = 1;
    public Sprite sprite;
    public Element element;

    /*
     * List of Effects to run
     */
    [SerializeReference, SubclassSelector]
    public List<CardEffect> effects = new List<CardEffect>();

    public string GetEffectDescStr()
    {
        var result = string.Empty;

        foreach(var effect in effects)
        {
            result += effect.EffectDescription + "\n";
        }

        return result;
    }

}
