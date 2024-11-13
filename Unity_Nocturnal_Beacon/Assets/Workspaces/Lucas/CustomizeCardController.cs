using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using System;
using DoTween.Animation;

public class CustomizeCardController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<GameObject> editableNotices = new List<GameObject>();
    [SerializeField] List<EffectSlot> effectSlots = new List<EffectSlot>();
    [SerializeField] CardEffectList cardEffectList;
    [SerializeField] CardManaList cardManaList;
    [SerializeField] CardManaSetting cardManaSetting;
    [SerializeField] ScrollingNumber manaScrollingNumber;

    ReactiveProperty<EffectSlot> selectingSlot = new ReactiveProperty<EffectSlot>();

    List<CardEffect> cardEffects = new List<CardEffect>();

    // Start is called before the first frame update
    void Start()
    {
        //var effects = TypeHelper.GetAllDerivedTypes<CardEffect>();

        //foreach(var v in effects)
        //{
        //    Debug.Log(v.Name);

        //    var inst = Activator.CreateInstance(v) as CardEffect;
        //    cardEffects.Add(inst);
        //}

        if(cardManaSetting == null)
        {
            cardManaSetting = Resources.Load<CardManaSetting>("Settings");
        }

        cardManaList.Setup(cardManaSetting.CardManaCosts);
        cardManaList.Selecting.Subscribe(x =>
        {
            if (x == null) return;

            if (x.data.mana >= 0)
                manaScrollingNumber.SetVal(x.data.mana);
            else
                manaScrollingNumber.SetVal("X");

            // TODO
            // handle resources - cost
        }).AddTo(this);

        cardEffects = CardEffectManager.Instance.CardEffectList;

        cardEffectList.Setup(cardEffects);
        cardEffectList.Selecting.Subscribe(x =>
        {
            if(selectingSlot.Value != null)
            {
                if (x == null)
                {
                    selectingSlot.Value.SetCardEffect(null);
                    return;
                }

                selectingSlot.Value.SetCardEffect(x.data);
            }

        }).AddTo(this);

        cardEffectList.EffectValue().Subscribe(x =>
        {
            if(selectingSlot.HasValue)
            {
                selectingSlot.Value.cardEffect.SetMainValue(x);
                
            }
        }).AddTo(this);

        selectingSlot.Subscribe(x =>
        {
            if (x == null) return;

            if(x.cardEffect != null)
            {
                cardEffectList.SetEffectValue(x.cardEffect.GetMainValue());
                cardEffectList.SetSelecting(x.cardEffect);
            }

            // TODO
            // handle resources - cost

        }).AddTo(this);
    }

    public void SetActiveAllEditableNotice(bool set)
    {
        editableNotices.ForEach(x => x.SetActive(set));
        
    }

    public void SetSelectingSlot(EffectSlot slot = null)
    {
        if (selectingSlot.HasValue && selectingSlot.Value == slot) return;

        selectingSlot.Value = slot;

        if (slot != null)
        {
            cardEffectList.SetSelecting(slot.cardEffect);
            cardEffectList.Show();
        }

        foreach (var s in effectSlots)
        {
            s.SetSelecting(s == slot);
        }

        //SetActiveAllEditableNotice(false);

    }

    public void Reset()
    {
        SetSelectingSlot(null);
        //SetActiveAllEditableNotice(true);
    }

    public List<CardEffect> GetCardEffects()
    {
        return cardEffects;
    }


}
