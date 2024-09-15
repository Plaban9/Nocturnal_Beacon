using CardAttribute;
using System;
using UnityEngine;

public interface ICardEffect 
{
    /*
     * Key for effect description text
     */
    string LocalizationKey { get; }
    /*
     * Or we can just straightly get the description in script?
     */
    string EffectDescription { get; }

    public virtual void OnUse(EffectTarget target)
    {

    }

    public virtual void BeforeCast(EffectTarget target, int amount)
    {

    }

    public virtual int OnCast(EffectTarget target, int amount)
    {
        return amount;
    }

    public virtual void AfterCast(EffectTarget target, int amount)
    {

    }

}

[Serializable]
public class CardEffect
{
    [SerializeField] protected AppMechanic appMechanic;
    [Tooltip("Main value")]
    [SerializeField] protected int val1 = 0;       // main value
    [Tooltip("Duration / number of times (-1 = cast all mana)")]
    [SerializeField] protected int val2;       // duration / number of times (-1 = cast all mana)
    [Tooltip("Spare value")]
    [SerializeField] protected int val3;       // spare
    [Tooltip("Spare value")]
    [SerializeField] protected int val4;       // spare
    [SerializeField] protected EffectTarget target;

}
