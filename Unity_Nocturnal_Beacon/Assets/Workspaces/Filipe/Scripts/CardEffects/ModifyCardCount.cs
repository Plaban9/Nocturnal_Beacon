using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardEffectDraw", menuName = "Modify Draw Effect")]
[System.Serializable]
public class ModifyCardCount : ICardEffect
{
    [SerializeField] int amount_modified = 0;
    public ModifyCardCount(int amount)
    {
        amount_modified = amount;
    }

    public ModifyCardCount() { }

    public void OnUse(int owner, int? target)
    {
        /* owner.Draw/Discard(amount);
         * {owner} draws/discards {amount} cards
         */
    }
}
