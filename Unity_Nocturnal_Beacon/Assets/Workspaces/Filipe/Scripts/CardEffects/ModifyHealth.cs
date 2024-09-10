using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardEffectModifyHealth", menuName = "Modify Health Effect")]
[System.Serializable]
public class ModifyHealth : ICardEffect
{
    [SerializeField] int damage = 0;



    public ModifyHealth(int user, int target, int amount)
    {
        damage = amount;
    }




    public void OnUse(int owner, int? target)
    {
        /*
         * DealDamage/HealTarget({target}, {amount}, {user})
         * TODO: Implement
         * Deal {amount} damage/ Heal {amount} damage to the {target}
         */
    }

}
