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

    public void OnUse(EffectTarget targetting, List<BattleUnit> targets);

    public void BeforeCast(EffectTarget targetting, List<BattleUnit> targets);

    public void OnCast(EffectTarget targetting, List<BattleUnit> targets);

    public void AfterCast(EffectTarget targetting, List<BattleUnit> targets);

    public void BeforeDealDamage(EffectTarget targetting, List<BattleUnit> targets);

    public int OnDealDamage(EffectTarget targetting, List<BattleUnit> targets, int amount);

    public void AfterDealDamage(EffectTarget targetting, List<BattleUnit> targets);


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
    [SerializeField] protected EffectTarget target;

    public virtual string LocalizationKey => "";
    public virtual string EffectDescription => "";

    
    public EffectTarget GetTargetting()
    {
        return target;
    }

    public virtual void OnCast(EffectTarget targetting, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual int OnDealDamage(EffectTarget targetting, List<BattleUnit> targets, int amount)
    {
        return amount;
        //throw new NotImplementedException();
    }

    public virtual void OnUse(EffectTarget targetting, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual void AfterCast(EffectTarget targetting, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual void AfterDealDamage(EffectTarget targetting, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual void BeforeCast(EffectTarget targetting, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual void BeforeDealDamage(EffectTarget targetting, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

}
