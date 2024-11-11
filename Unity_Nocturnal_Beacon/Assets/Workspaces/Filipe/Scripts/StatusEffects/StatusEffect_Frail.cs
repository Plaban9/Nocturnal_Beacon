using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_Frail : BattleStatusEffect
{
    public override int OnGainBlock(int block)
    {
        int finalAmount = (int) Mathf.Floor( block * 0.75f);
        return finalAmount;
    }

    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
