using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_Thorns : BattleStatusEffect
{

    public override int OnTakeDamage(BattleUnit attacker, int damage)
    {
        attacker.GetHPData().DealDamage(owner,_intensity, true);
        return damage;
    }

    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
