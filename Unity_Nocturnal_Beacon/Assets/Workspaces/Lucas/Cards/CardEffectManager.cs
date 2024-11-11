using System;
using System.Collections;
using System.Collections.Generic;
using CardAttribute;
using UnityEngine;

public class CardEffectManager : MonoBehaviour
{
    static List<CardEffect> cardEffectList = new List<CardEffect>();

    public static List<CardEffect> CardEffectList => cardEffectList;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        cardEffectList = new List<CardEffect>();

        cardEffectList.Add(new ModifyCardCount());

        foreach(var c in Enum.GetValues(typeof(EffectTarget)))
        {
            
        }

    }
}
