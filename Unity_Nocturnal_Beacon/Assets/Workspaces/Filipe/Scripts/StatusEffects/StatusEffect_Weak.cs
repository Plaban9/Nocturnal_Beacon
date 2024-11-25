using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_Weak : BattleStatusEffect
{

    public override int OnDealDamage(int damage)
    {
        int finalDamage = (int) Mathf.Floor(damage *0.75f);
        return finalDamage;
    }

    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
