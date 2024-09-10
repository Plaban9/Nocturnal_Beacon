using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardEffectInvoker 
{
    public enum Effects
    {
        ModifyHealth = 0,
        ModifyShield,
        ModifyStatuses,
        ModifyCardCount
    }

    [SerializeField] Effects chosenEffect;

}

