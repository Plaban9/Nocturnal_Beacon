using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class CardManaList : SelectionList<ManaCostSelectable, CardManaCost>
{
    public override void SetSelecting(CardManaCost data)
    {
        if (data == null)
        {
            selecting.Value = null;
            return;
        }

        if (selecting.Value != null && selecting.Value.data == data)
        {
            // Unselect
            selecting.Value = null;
            return;
        }

        selecting.Value = selectables.First(x => x.data.mana == data.mana);
    }
}
