using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_Buffer : BattleStatusEffect
{

    public override int OnTakeDamage(BattleUnit unit, int damage)
    {
        _duration -= 1;
        return 0;
    }


}
