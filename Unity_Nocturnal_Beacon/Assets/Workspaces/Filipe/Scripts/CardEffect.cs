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

    public void OnUse(BattleUnit owner, List<BattleUnit> targets);

    public void BeforeCast(BattleUnit owner, List<BattleUnit> targets);

    public void OnCast(BattleUnit owner, List<BattleUnit> targets);

    public void AfterCast(BattleUnit owner, List<BattleUnit> targets);

    public void BeforeDealDamage(BattleUnit ownerg, List<BattleUnit> targets);

    public int OnDealDamage(BattleUnit owner, List<BattleUnit> targets, int amount);

    public void AfterDealDamage(BattleUnit owner, List<BattleUnit> targets);


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

    public virtual string LocalizationKey => "";
    public virtual string EffectDescription => "";
    public virtual string EffectDetailDescription => "";

    public virtual int GetEffectCost() => 0;

    public void SetMainValue(int val)
    {
        val1 = val1 >= 0 ? val : -val;
    }

    public int GetMainValue() => val1;

    public EffectTarget GetTarget() => target;
    public EffectTargetAmount GetTargetAmount() => targetAmount;

    public EffectType GetEffectType() => effectType;

    public virtual void OnCast(BattleUnit owner, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual int OnDealDamage(BattleUnit owner, List<BattleUnit> targets, int amount)
    {
        return amount;
        //throw new NotImplementedException();
    }

    public virtual void OnUse(BattleUnit owner, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual void AfterCast(BattleUnit owner, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual void AfterDealDamage(BattleUnit owner, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual void BeforeCast(BattleUnit owner, List<BattleUnit> targets)
    {
        //throw new NotImplementedException();
    }

    public virtual void BeforeDealDamage(BattleUnit owner, List<BattleUnit> targets)
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
