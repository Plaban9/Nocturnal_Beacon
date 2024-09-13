using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewCardEffectStatus", menuName = "Modify Status Effect")]

public class ModifyStatuses : ICardEffect
{
    [SerializeField] public int appMechanic;
    [SerializeField] int statusEffect;
    [SerializeField] int duration;
    [SerializeField] int val1, val2, val3, val4;

    ModifyStatuses(int statusEffect, int duration, int appMechanic, int val1 = -1, int val2 = -1, int val3 = -1, int val4 = -1)
    {
        this.appMechanic = appMechanic;
        this.statusEffect = statusEffect;
        this.duration = duration;
        this.val1 = val1;
        this.val2 = val2;
        this.val3 = val3;
        this.val4 = val4;
    }

    ModifyStatuses() { }

    public void OnUse(int owner, int? target)
    {
        if (appMechanic == 1)
        {
            //If is [On Use]
            /*
             * ModifyStatus(statusEffect, duration)
             * If a status effect requires more values (damage dealt by poison per turn, etc), save on val1~val4
             */
        }
    }

    public void AfterDealDamage(int owner, int target, int amount)
    {
        if (appMechanic == 0)
        {
            //If is [On Hit]
            /*
             * ModifyStatus(statusEffect, duration)
             * If a status effect requires more values (damage dealt by poison per turn, etc), save on val1~val4 
             */
        }
    }
}
