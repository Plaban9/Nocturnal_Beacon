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
        Legendary,
        Enemy = -1
    };

    [Serializable]
    public enum EffectType
    {
        DealDamage = 0,
        GainHealth,
        GainShield,
        GainStatusStrength,
        GainStatusDexterity,
        GainStatusRegenerate,
        DrawCard,
        DiscardCard,
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
        NoneResist,     // Reduces Affinity to Neutral by n
        FireResist,       // Reduces Affinity to Fire by n
        WaterResist,      // Reduces Affinity to Water by n
        WindResist,       // Reduces Affinity to Wind by n
        EarthResist,      // Reduces Affinity to Earth by n
        LightResist,      // Reduces Affinity to Light by n
        DarkResist,       // Reduces Affinity to Dark by n
        GhostResist,      // Reduces Affinity to Ghost by n
        NoneEleChange,    // Change Element to Neutral
        FireEleChange,       // Change Element to Fire
        WaterEleChange,      // Change Element to Water
        WindEleChange,       // Change Element to Wind
        EarthEleChange,      // Change Element to Earth
        LightEleChange,      // Change Element to Light
        DarkEleChange,       // Change Element to Dark
        GhostEleChange,      // Change Element to Ghost


        /* ============== DeBuffs ============== */
        Poision,        // At the beginning of its turn, the target loses X HP and 1 stack of Poison.
        Vulerable,      // Target takes 50% more damage from attacks.
        Weak,           // Target deals 25% less attack damage.
        NoDraw,         // You may not draw any more cards this turn.
        Frail,          // Block gained from cards is reduced by 25%.
        Confused,       // The costs of your cards are randomized on draw, from 0 to 3.
        NoneWeak,       // Increases Affinity to Neutral by n
        FireWeak,       // Increases Affinity to Fire by n
        WaterWeak,      // Increases Affinity to Water by n
        WindWeak,       // Increases Affinity to Wind by n
        EarthWeak,      // Increases Affinity to Earth by n
        LightWeak,      // Increases Affinity to Light by n
        DarkWeak,       // Increases Affinity to Dark by n
        GhostWeak,      // Increases Affinity to Ghost by n
        NoAct           // Prevents action for a turn (enemy only, player cannot be affected)
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
        LIGHT,
        GHOST
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
    [SerializeField] private int manaCost = 1;
    public Sprite sprite;
    public Element element;
    [Tooltip("Price for shop")] public int price;
    private List<CardStatuses> _statuses = new List<CardStatuses>();

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
            if(effect != null)
                result += effect.EffectDescription + "\n";
        }

        return result;
    }


    public bool TargetSingleEnemy()
    {
        return effects.Find(it => 
           (it.GetTarget() == CardAttribute.EffectTarget.OpponentSingle)
        ) != null;
    }

    public bool TargetAllEnemy()
    {
        return effects.Find(it =>
           (it.GetTarget() == CardAttribute.EffectTarget.OpponentAll ||
          it.GetTarget() == CardAttribute.EffectTarget.OpponentRandom)
        ) != null;
    }

    public bool TargetSelf()
    {
        return effects.Find(it =>
           (it.GetTarget() == CardAttribute.EffectTarget.Self)
        ) != null;
    }


    public void FlushStatuses()
    {
        _statuses.Clear();
    }

    public void AddStatus(CardStatuses status)
    {
        _statuses.Add(status);
    }

    public int GetManaCost()
    {
        int i = manaCost;
        foreach(CardStatuses status in _statuses)
        {
            i = status.GetManaCost(i);
        }
        return i;
    }

    public int GetBaseManaCost()
    {
        return manaCost;
    }

    public void SetBaseManaCost(int i)
    {
        manaCost = i;
    }


}

public enum ElementalEffectivity
{
    UNAFFECTED = 0,
    RESIST,
    INEFFECTIVE,
    NEUTRAL,
    EFFECTIVE,
    VERY_EFFECTIVE,
    MAX_EFFECTIVE
}