using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using System;

public class CustomizeCardController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<GameObject> editableNotices = new List<GameObject>();
    [SerializeField] List<EffectSlot> effectSlots = new List<EffectSlot>();
    [SerializeField] CardEffectList cardEffectList;

    ReactiveProperty<EffectSlot> selectingSlot = new ReactiveProperty<EffectSlot>();

    List<CardEffect> cardEffects = new List<CardEffect>();

    // Start is called before the first frame update
    void Start()
    {
        var effects = TypeHelper.GetAllDerivedTypes<CardEffect>();

        foreach(var v in effects)
        {
            Debug.Log(v.Name);

            var inst = Activator.CreateInstance(v) as CardEffect;
            cardEffects.Add(inst);
        }

        cardEffectList.Setup(cardEffects);
        cardEffectList.SelectingCardEffect.Subscribe(x =>
        {
            if(selectingSlot.Value != null)
            {

                if (x == null)
                {
                    selectingSlot.Value.SetCardEffect(null);
                    return;
                }

                selectingSlot.Value.SetCardEffect(x.cardEffect);
            }

        }).AddTo(this);

        selectingSlot.Subscribe(x =>
        {
            
        }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveAllEditableNotice(bool set)
    {
        editableNotices.ForEach(x => x.SetActive(set));
        
    }

    public void SetSelectingSlot(EffectSlot slot = null)
    {
        if (selectingSlot.Value != null && selectingSlot.Value == slot) return;

        selectingSlot.Value = slot;

        foreach(var s in effectSlots)
        {
            s.SetSelecting(s == slot);
        }

        //SetActiveAllEditableNotice(false);
        cardEffectList.Show();

        if(slot != null)
            cardEffectList.SetSelectingEffect(slot.cardEffect);
    }

    public void Reset()
    {
        SetSelectingSlot(null);
        SetActiveAllEditableNotice(true);
    }

    public List<CardEffect> GetCardEffects()
    {
        return cardEffects;
    }


}
