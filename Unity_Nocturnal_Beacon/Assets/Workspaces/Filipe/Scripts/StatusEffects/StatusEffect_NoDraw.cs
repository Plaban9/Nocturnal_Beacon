using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_NoDraw : BattleStatusEffect
{

    public override int OnDraw(int cardAmount)
    {
        return 0;
    }

    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}
