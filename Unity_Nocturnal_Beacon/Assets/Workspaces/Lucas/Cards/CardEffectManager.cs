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
        cardEffectList = new List<CardEffect>();

        cardEffectList.Add(new ModifyCardCount());

        foreach (var c in Enum.GetValues(typeof(EffectTarget)))
        {
            cardEffectList.Add(new ModifyHealth((EffectTarget)c, 0, 0));
        }

        cardEffectList.Add(new ModifyShield());

        foreach (var c in Enum.GetValues(typeof(StatusEffect)))
        {
            cardEffectList.Add(new ModifyStatuses((StatusEffect)c, 0, AppMechanic.OnUse, 0));
        }
    }
}
