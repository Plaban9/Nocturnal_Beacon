using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_Dexterity : BattleStatusEffect
{
    public override int OnGainBlock(int block)
    {
        return block + _intensity;
    }

    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}