using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_Artifact : BattleStatusEffect
{

    public override BattleStatusEffect? OnGainStatus(BattleStatusEffect battleStatusEffect)
    {

        if (battleStatusEffect._status.isPositive)
        {
            return battleStatusEffect;
        }
        else
        {
            _duration -= 1;
            return null;
        }
    }

}
