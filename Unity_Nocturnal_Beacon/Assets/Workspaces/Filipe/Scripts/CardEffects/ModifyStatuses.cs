using CardAttribute;
using System;
using UnityEngine;

[Serializable]
public class ModifyStatuses : CardEffect, ICardEffect
{
    [SerializeField] StatusEffect statusEffect;
    [SerializeField] StatusStacks statusStacks;

    public string LocalizationKey => "CE_DESC_ModifyStatuses";
    public string EffectDescription
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

    public ModifyStatuses() { }
    public ModifyStatuses(StatusEffect statusEffect, int duration, AppMechanic appMechanic, int val1 = -1, int val2 = -1, int val3 = -1, int val4 = -1)
    {
        /* this.appMechanic = appMechanic;
        this.statusEffect = statusEffect;
        this.val1 = val1;
        this.val2 = val2;
        this.val3 = val3;
        this.val4 = val4; */
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
}
