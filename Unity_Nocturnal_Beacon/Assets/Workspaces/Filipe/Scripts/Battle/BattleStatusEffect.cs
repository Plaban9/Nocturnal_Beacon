using CardAttribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleStatusEffect 
{
    public StatusEffectObject _status;
    public int _intensity;
    public int _duration;
    public BattleUnit owner;


    public virtual void OnTurnEnd() { }

    public virtual void OnTurnStart()
    {

    }
    
    public virtual int OnDraw(int cardAmount)
    {
        return cardAmount;
    }

    public virtual int OnDealDamage(int damage)
    {
        return damage;
    }

    public virtual int OnTakeDamage(BattleUnit attacker, int damage)
    {
        return damage;
    }

    public virtual int OnGainBlock(int block)
    {
        return block;
    }

    public virtual int OnLoseBlock(int block)
    {
        return block;
    }


    public virtual void AfterDealDamage()
    {

    }

    public virtual BattleStatusEffect OnGainStatus(BattleStatusEffect battleStatusEffect)
    {
        return battleStatusEffect;
    }

    public virtual int OnGetCardCost(int cardManaCost)
    {
        return cardManaCost;
    }

    public virtual ElementalEffectivity OnGetElementalAffinity(Element incoming, ElementalEffectivity previousEffectivity)
    {
        return previousEffectivity;
    }

    public virtual Element OnGetElement(Element incoming, bool additive)
    {
        return incoming;
    }

    public virtual int OnGetNoAct(int i)
    {
        return i;
    }
}
