using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_Confused : BattleStatusEffect
{
    public override void OnTurnEnd()
    {
        _duration++;
    }
}
