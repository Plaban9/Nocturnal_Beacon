using System;

[Serializable]
public class StatusEffect_NoAct : BattleStatusEffect
{
    public override int OnGetNoAct(int i)
    {
        return i + 1;
    }

    public override void OnTurnEnd()
    {
        _duration -= 1;
    }
}