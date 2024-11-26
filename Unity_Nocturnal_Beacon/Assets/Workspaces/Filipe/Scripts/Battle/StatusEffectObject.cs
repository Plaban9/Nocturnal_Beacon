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
    [SerializeField] public string name;
    [SerializeField] public string description;
    [SerializeField] public Sprite icon;
    [SerializeField] public bool isPositive;

    public StatusEffectObject(StatusEffect statusEffect)
    {
        this.statusEffect = statusEffect;
    }

    public string GetName()
    {
        if(name != null)
        {
            return name;
        }
        else
        {
            Debug.LogError($"Missing name on {this.name}");
            return "Missing name";
        }
    }
}
