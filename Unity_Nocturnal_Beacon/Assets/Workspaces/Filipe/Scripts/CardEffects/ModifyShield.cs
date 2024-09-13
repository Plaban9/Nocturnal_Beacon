using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardEffectShield", menuName = "Modify Shield Effect")]
[Serializable]
public class ModifyShield : ICardEffect
{

    [SerializeField] int modification = 0;
    public ModifyShield(int amount)
    {
        modification = amount;
    }

    public ModifyShield() { }

    public void OnUse(int owner, int? target)
    {
        /*
         * target.AddShield(amount)
         */
    }
}
