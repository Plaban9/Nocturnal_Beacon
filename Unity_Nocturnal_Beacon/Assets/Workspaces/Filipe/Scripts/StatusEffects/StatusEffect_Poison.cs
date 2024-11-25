using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_Poison : BattleStatusEffect
{
    public override void OnTurnEnd()
    {
        owner.GetHPData().DealDamage(null,_intensity);
        _duration -= 1;
    }
}
