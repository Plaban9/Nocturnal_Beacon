using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSlot : MonoBehaviour
{
    [SerializeField] GameObject selectingGO;
    [SerializeField] TMPro.TextMeshProUGUI effectText;

    public CardEffect cardEffect { get; private set; }
    public bool isDefault { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        this.cardEffect = cardEffect;
        this.isDefault = isDefault;

        if(cardEffect != null)
        {
            effectText.text = cardEffect.EffectDescription;
        }
        else
        {
            effectText.text = "";
        }
    }
}
