using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_DrawBonus : BattleStatusEffect
{

    public override int OnDraw(int cardAmount)
    {
        _duration -= 1; 
        return cardAmount+_intensity;
    }

    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
