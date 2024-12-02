using System;
using System.Collections;
using System.Collections.Generic;
using CardAttribute;
using UnityEngine;

public class CardEffectManager : MonoBehaviour
{
    private static CardEffectManager instance;

    List<CardEffect> cardEffectList = new List<CardEffect>();

    public List<CardEffect> CardEffectList => cardEffectList;

    public static CardEffectManager Instance => instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Init();
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Init()
    {
        cardEffectList = new List<CardEffect>
        {
            new ModifyHand(1),
            new ModifyHand(-1),
            new ModifyMana(1),
            new ModifyMana(-1),
            new ModifyHealth(EffectTarget.Self, 1, 0),
            new ModifyHealth(EffectTarget.Self, -1, 0),
            new ModifyHealth(EffectTarget.OpponentSingle, -1, 0),
            new ModifyHealth(EffectTarget.OpponentRandom, -1, 0),
            new ModifyHealth(EffectTarget.OpponentAll, -1, 0),
            new ModifyShield(1),

        };
 
        //foreach (EffectTarget target in Enum.GetValues(typeof(EffectTarget)))
        //{
        //    foreach(StatusEffect statusEffect in Enum.GetValues(typeof(StatusEffect))){
        //        cardEffectList.Add(new ModifyStatuses(new StatusEffectObject(statusEffect), 1, target, AppMechanic.OnUse, 1));
        //    }
        //}
    }
}
