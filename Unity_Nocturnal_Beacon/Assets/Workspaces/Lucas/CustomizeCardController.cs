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

    [Header("Card")]
    [SerializeField] Card cardSO;
    [SerializeField] CardDisplay cardDisplay;

    [Header("Card Effect")]
    [SerializeField] List<EffectSlot> effectSlots = new List<EffectSlot>();
    [SerializeField] CardEffectList cardEffectList;

    [Header("Mana")]
    [SerializeField] CardManaList cardManaList;
    [SerializeField] CardManaSetting cardManaSetting;
    [SerializeField] ScrollingNumber manaScrollingNumber;

    ReactiveProperty<EffectSlot> selectingSlot = new ReactiveProperty<EffectSlot>();
    List<CardEffect> cardEffects = new List<CardEffect>();

    // Start is called before the first frame update
    void Start()
    {
        cardDisplay.Setup(cardSO);

        if(cardManaSetting == null)
        {
            cardManaSetting = Resources.Load<CardManaSetting>("Settings");
        }

        cardManaList.Setup(cardManaSetting.CardManaCosts);
        cardManaList.SetSelecting(cardManaSetting.CardManaCosts.First(x => x.mana == cardSO.manaCost));
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
            if(selectingSlot.Value != null && !cardEffectList.IsClosed())
            {
                if (x == null)
                {
                    selectingSlot.Value.SetCardEffect(null);
                    return;
                }

                if(selectingSlot.Value.cardEffect == null || !selectingSlot.Value.cardEffect.Compare(x.data))
                    selectingSlot.Value.SetCardEffect(x.data);
            }

        }).AddTo(this);

        cardEffectList.EffectValue().Subscribe(x =>
        {
            if(selectingSlot.Value != null && selectingSlot.Value.cardEffect != null)
            {
                selectingSlot.Value.SetEffectValue(x);
            }
        }).AddTo(this);

        selectingSlot.Subscribe(x =>
        {
            if (x == null) return;

            cardEffectList.SetLockedEffect(effectSlots.Where(y => y != x && y.cardEffect != null).Select(x => x.cardEffect).ToList());

            if (x.cardEffect != null)
            {
                cardEffectList.SetEffectValue(x.effectValue);

                if (!x.isDefault)
                    cardEffectList.SetDefaultSelecting(x.cardEffect);
                else
                    cardEffectList.SetSelectingWithLock(x.cardEffect);


            }
            else
            {
                cardEffectList.Reset();
            }
            // TODO
            // handle resources - cost

        }).AddTo(this);

        for(int i=0; i<cardSO.effects.Count; i++)
        {
            if (i >= effectSlots.Count) break;

            var e = cardSO.effects[i];

            effectSlots[i].SetCardEffect(e, true);
        }
    }

    public void SetActiveAllEditableNotice(bool set)
    {
        editableNotices.ForEach(x => x.SetActive(set));
        
    }

    public void SetSelectingSlot(EffectSlot slot = null)
    {
        selectingSlot.Value = slot;

        if (slot != null)
        {
            cardEffectList.Show();
        }

        foreach (var s in effectSlots)
        {
            s.SetSelecting(s == slot);
        }
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
