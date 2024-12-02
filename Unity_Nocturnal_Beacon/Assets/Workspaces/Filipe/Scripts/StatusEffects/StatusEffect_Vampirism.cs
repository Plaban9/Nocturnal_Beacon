using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_Vampirism : BattleStatusEffect
{

    public override int OnDealDamage(int damage)
    {
        int healthRegained = Math.Abs((int) Mathf.Floor(((float)damage * (float)_intensity * 0.25f)));
        Debug.Log($"Health Regained: "+healthRegained);
        owner.GetHPData().RecoverHealth(healthRegained);
        return base.OnDealDamage(damage);
    }


    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
