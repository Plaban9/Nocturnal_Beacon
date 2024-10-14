using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect_Regeneration : BattleStatusEffect
{
    public override void OnTurnStart()
    {
        owner.GetHPData().RecoverHealth(_intensity);
        _duration -= 1;
    }
}
