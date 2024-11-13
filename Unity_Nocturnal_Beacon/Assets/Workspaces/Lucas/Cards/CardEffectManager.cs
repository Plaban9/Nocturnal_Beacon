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
            new ModifyCardCount(1),
            new ModifyCardCount(-1),
            new ModifyHealth(EffectTarget.Self, 1, 0),
            new ModifyHealth(EffectTarget.Self, -1, 0),
            new ModifyHealth(EffectTarget.OpponentSingle, -1, 0),
            new ModifyHealth(EffectTarget.OpponentRandom, -1, 0),
            new ModifyHealth(EffectTarget.OpponentAll, -1, 0),
            new ModifyShield(),
            new ModifyStatuses(new StatusEffectObject(StatusEffect.Strength), 1, AppMechanic.OnUse, 1),
            new ModifyStatuses(new StatusEffectObject(StatusEffect.Dexterity), 1, AppMechanic.OnUse, 1),
            new ModifyStatuses(new StatusEffectObject(StatusEffect.Regenerate), 1, AppMechanic.OnUse, 1)
        };
    }
}
