using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardAttribute
{
    public enum Type
    {
        Attack,
        Skill,
        Curse
    }

    public enum Rarity
    {
        Normal = 0,
        Rare, 
        Legendary
    };

    public enum Effect
    {
        DealDamage = 0,
        GainShield,
        GainHealth,
        GainStrength,
        DrawCard,
        E

    };

    public enum EffectTarget
    {
        Self = 0,
        OpponentSingle,
        OpponentRandom,
        OpponentAll,
        Both
    }
    public enum EffectTargetAmount
    {
        Single = 0,
        Random,
        All
    };

}

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
[Serializable]
public class Card : ScriptableObject
{
    public int id;
    public new string name;
    public Rarity rarity;
    public int manaCost;

    public string imgPath;


    // Start is called before the first frame update
    void Start()
    {
        var v = ScriptableObject.CreateInstance<Card>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * List of Effects to run
     */
    [SerializeReference, SubclassSelector]
    public List<ICardEffect> effects = new List<ICardEffect>();
}
