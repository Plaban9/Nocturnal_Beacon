using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_Vulnerable : BattleStatusEffect
{

    public override int OnTakeDamage(BattleUnit unit, int damage)
    {
        int finalDamage = (int) Mathf.Floor(damage * 1.25f);
        return finalDamage;
    }

    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
