using CardAttribute;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifyStatuses : CardEffect
{
    [SerializeField] StatusEffectObject statusEffectObject;
    [SerializeField] StatusEffect statusEffect;
    [SerializeField] StatusStacks statusStacks;

    BattleStatusEffect _bstf;

    StatusEffectObject _statusObj;
    int _duration;
    int _intensity;

    public override string LocalizationKey => "CE_DESC_ModifyStatuses";

    public override string EffectDescription
    {
        get
        {
            string result = string.Empty;

            result = $"Gain {_bstf._intensity} <color=#FB8B48>{statusEffect.ToString()}</color>";

            return result;
        }
    }

    public override string EffectDetailDescription
    {
        get
        {
            string result = string.Empty;

            switch (statusEffect)
            {
                case StatusEffect.Strength:
                    result = (val1 >= 0 ? "Increases " : "Decreases ") + $"attack damage by {val1}.";
                    break;
                case StatusEffect.Dexterity:
                    result = (val1 >= 0 ? "Increases " : "Decreases ") + $"Block gained from cards by {val1}.";
                    break;
                case StatusEffect.Throns:
                    result = $"When attacked, deals {val1} damage back.";
                    break;
                case StatusEffect.Regenerate:
                    result = $"At the end of its turn, heals {val1} HP.";
                    break;

                default:
                    result = $"[ERROR] No such status: {statusEffect.ToString()}";
                    break;
            }
            return result;
        }
    }

    public override int GetEffectCost()
    {
        int val = 0;

        switch (statusEffect)
        {
            case StatusEffect.Strength:
                val = (5 + 3 * (val-1)) * val;
                break;
            case StatusEffect.Dexterity:
                val = (5 + 3 * (val - 1)) * val;
                break;
            case StatusEffect.Regenerate:
                val = (4 + 2 * (val - 1)) * val;
                break;
            default:
                break;
        }

        return val;
    }

    public ModifyStatuses() { }
    public ModifyStatuses(StatusEffectObject statusObj, int duration, AppMechanic appMechanic, int val1 = -1, int val2 = -1, int val3 = -1, int val4 = -1)
    {
        statusEffect = statusObj.statusEffect;
        _bstf = null;
        switch (statusEffect)
        {
            case StatusEffect.Strength:
                _bstf = new StatusEffect_Strength();
                break;
            case StatusEffect.Dexterity:
                _bstf = new StatusEffect_Dexterity();
                break;
            case StatusEffect.Regenerate:
                _bstf = new StatusEffect_Regeneration();
                break;
            default:
                break;
        }

        if (_bstf == null)
            return;
        _bstf._intensity = val1;
        _bstf._duration = val2;

        /* this.appMechanic = appMechanic;
        this.statusEffect = statusEffect;
        this.val1 = val1;
        this.val2 = val2;
        this.val3 = val3;
        this.val4 = val4; 
        */
    }

    public void OnUse(EffectTarget target)
    {
       // if (appMechanic == AppMechanic.OnUse)
        {
            //If is [On Use]
            /*
             * ModifyStatus(statusEffect, duration)
             * If a status effect requires more values (damage dealt by poison per turn, etc), save on val1~val4
             */
        }
    }

    public void AfterCast(EffectTarget target, int amount)
    {
        //if (appMechanic == AppMechanic.AfterCast)
        {
            //If is [On Hit]
            /*
             * ModifyStatus(statusEffect, duration)
             * If a status effect requires more values (damage dealt by poison per turn, etc), save on val1~val4 
             */
        }
    }

    public override void OnUse(BattleUnit owner, List<BattleUnit> targets)
    {
        base.OnUse(owner, targets);
        _bstf = null;
        switch (statusEffect)
        {
            case StatusEffect.Strength:
                _bstf = new StatusEffect_Strength();
                break;
            case StatusEffect.Dexterity:
                _bstf = new StatusEffect_Dexterity();
                break;
            case StatusEffect.Regenerate:
                _bstf = new StatusEffect_Regeneration();
                break;
            default:
                break;
        }

        if (_bstf == null)
            return;
        _bstf._status = statusEffectObject;
        _bstf._intensity = val1;
        _bstf._duration = val2;
        foreach (var unit in targets)
        {
            BattleStatusEffect newEffect = _bstf;
            newEffect.owner = unit;
            unit.GetUnitStatusData().AddStatusEffect(_bstf);
        }
    }
}
