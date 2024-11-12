using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New StatusEffectObject", menuName = "Status Effect")]
public class StatusEffectObject : ScriptableObject
{
    [SerializeField] public StatusEffect statusEffect;
    [SerializeField] public Sprite icon;
    [SerializeField] public string description;
    [SerializeField] public string name;
    [SerializeReference, SubclassSelector] public BattleStatusEffect effect;
    
    public StatusEffectObject(StatusEffect statusEffect)
    {
        this.statusEffect = statusEffect;
    }
}
