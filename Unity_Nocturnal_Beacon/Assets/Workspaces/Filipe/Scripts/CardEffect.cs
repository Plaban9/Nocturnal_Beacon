using CardAttribute;
using System;
using System.Collections.Generic;
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



    public void OnUse(Card card, BattleUnit owner, List<BattleUnit> targets);

    public void BeforeCast(Card card, BattleUnit owner, List<BattleUnit> targets);

    public void OnCast(Card card, BattleUnit owner, List<BattleUnit> targets);

    public void AfterCast(Card card, BattleUnit owner, List<BattleUnit> targets);

    public void BeforeDealDamage(Card card, BattleUnit ownerg, List<BattleUnit> targets);

    public int OnDealDamage(Card card, BattleUnit owner, List<BattleUnit> targets, int amount);

    public void AfterDealDamage(Card card, BattleUnit owner, List<BattleUnit> targets);


}

[Serializable]
public class CardEffect : ICardEffect
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
    [SerializeField] protected EffectType effectType;
    [SerializeField] protected EffectTarget target;
    [SerializeField] protected EffectTargetAmount targetAmount;

    public WithManaDoEffect.CardVariable _withManaDoEffectAffecting;

    public virtual string LocalizationKey => "";
    public virtual string EffectDescription => "";
    public virtual string EffectDetailDescription => "";

    public virtual int GetEffectCost() => 0;

    public virtual void SetMainValue(int val)
    {
        val1 = val1 >= 0 ? val : -val;
    }

    public void SetValue(WithManaDoEffect.CardVariable val, int i)
    {
        switch (val)
        {
            case WithManaDoEffect.CardVariable.VAL1:
                val1 = i;
                break;
            case WithManaDoEffect.CardVariable.VAL2:
                val2 = i;
                break;
            case WithManaDoEffect.CardVariable.VAL3:
                val3 = i;
                break;
            case WithManaDoEffect.CardVariable.VAL4:
                val4 = i;
                break;
        }
    }

    public int GetValue(WithManaDoEffect.CardVariable val)
    {
        switch (val)
        {
            case WithManaDoEffect.CardVariable.VAL1:
                return val1;
            case WithManaDoEffect.CardVariable.VAL2:
                return val2;
            case WithManaDoEffect.CardVariable.VAL3:
                return val3;
            case WithManaDoEffect.CardVariable.VAL4:
                return val4;
            default:
                return val1;
        }
    }

    public int GetMainValue() => val1;

    public EffectTarget GetTarget() => target;
    public EffectTargetAmount GetTargetAmount() => targetAmount;

    public EffectType GetEffectType() => effectType;

    public virtual void OnCast(Card card, BattleUnit owner, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual int OnDealDamage(Card card, BattleUnit owner, List<BattleUnit> targets, int amount)
    {
        return amount;
        //throw new NotImplementedException();
    }

    public virtual void OnUse(Card card, BattleUnit owner, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual void AfterCast(Card card, BattleUnit owner, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual void AfterDealDamage(Card card, BattleUnit owner, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual void BeforeCast(Card card, BattleUnit owner, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual void BeforeDealDamage(Card card, BattleUnit owner, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual bool Compare(CardEffect e)
    {
        return GetEffectType() == e.GetEffectType() 
            && GetTarget() == e.GetTarget() 
            && GetTargetAmount() == e.GetTargetAmount();
    }

    public CardEffect Clone()
    {
        return (CardEffect)MemberwiseClone(); // Shallow copy
    }

    
}
