using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_Strength : BattleStatusEffect
{

    public override int OnDealDamage(int damage)
    {
        int finalDamage = damage - _intensity;
        return finalDamage;
    }

    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
