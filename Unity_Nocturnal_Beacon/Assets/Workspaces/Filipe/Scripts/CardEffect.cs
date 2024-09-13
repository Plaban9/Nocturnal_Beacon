using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardEffect 
{

    public virtual void OnUse(int owner, int? target)
    {

    }

    public virtual void  BeforeDealDamage(int owner, int target, int amount)
    {

    }

    public virtual int OnDealDamage(int owner, int target, int amount)
    {
        return amount;
    }

    public virtual void AfterDealDamage(int owner, int target, int amount)
    {

    }

}
