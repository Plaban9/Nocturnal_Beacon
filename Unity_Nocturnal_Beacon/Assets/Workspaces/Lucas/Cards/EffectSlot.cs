using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectSlot : MonoBehaviour
{
    [SerializeField] GameObject selectingGO;
    [SerializeField] TMPro.TextMeshProUGUI effectText;

    public CardEffect cardEffect { get; private set; }

    public int effectValue { get; private set; }

    public bool isDefault { get; private set; }

    public void SetSelecting(bool set)
    {
        selectingGO.SetActive(set);
    }

    public void SetCardEffect(CardEffect cardEffect)
    {
        SetCardEffect(cardEffect, isDefault);
    }

    public void SetCardEffect(CardEffect cardEffect, bool isDefault)
    {
        this.cardEffect = cardEffect != null ? cardEffect.Clone() : null;
        this.isDefault = isDefault;

        if(cardEffect != null)
        {
            effectValue = cardEffect.GetMainValue();
            effectText.text = !isDefault ? $"<color=#FFFF43>{cardEffect.EffectDescription}</color>" : cardEffect.EffectDescription;
        }
        else
        {
            effectText.text = "";
        }
    }

    public void SetEffectValue(int val)
    {
        effectValue = val;
        cardEffect.SetMainValue(val);
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        if (cardEffect != null)
        {
            effectText.text = !isDefault ? $"<color=#FFFF43>{cardEffect.EffectDescription}</color>" : cardEffect.EffectDescription;
        }
        else
        {
            effectText.text = "";
        }
    }
}
