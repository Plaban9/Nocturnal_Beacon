using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifyCards : CardEffect
{
    public override string LocalizationKey => "CE_DESC_ModifyCards";
    public override string EffectDescription
    {
        get
        {
            string result = string.Empty;

            result += $"Customize a card with <color=#5AFF00>{val1}</color> points.";

            return result;
        }
    }

    public ModifyCards()
    {

    }
}
