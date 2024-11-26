using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using System;
using DoTween.Animation;
using UnityEngine.UI;
using UnityEditor;

public class CustomizeCardController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<GameObject> editableNotices = new List<GameObject>();
    [SerializeField] Toggle editableNoticeToggle;

    [Header("Points")]
    [SerializeField] NumberText remainPointText;
    [SerializeField] NumberText diffPointText;
    [SerializeField] ReactiveProperty<int> remainPoint = new ReactiveProperty<int>(30);
    [SerializeField] int initPoint;

    [Header("Card")]
    [SerializeField] Card cardSO;
    [SerializeField] CardDisplay cardDisplay;
    [SerializeField] TMPro.TMP_InputField cardNameInput;

    [Header("Card Effect")]
    [SerializeField] List<EffectSlot> effectSlots = new List<EffectSlot>();
    [SerializeField] CardEffectList cardEffectList;

    [Header("Mana")]
    [SerializeField] CardManaList cardManaList;
    [SerializeField] CardManaSetting cardManaSetting;
    [SerializeField] ScrollingNumber manaScrollingNumber;

    ReactiveProperty<EffectSlot> selectingSlot = new ReactiveProperty<EffectSlot>();
    List<CardEffect> cardEffects = new List<CardEffect>();

    Action<Card> onCustomized;
    bool IsInited = false;

    // Start is called before the first frame update
    void Start()
    {
        //Init();
    }

    public void Setup(Card c, int point, Action<Card> onCustomized)
    {
        this.cardSO = c;
        this.onCustomized = onCustomized;
        initPoint = point;
        
        Init();
    }

    private void Init()
    {
        if (IsInited) return;

        cardDisplay.Setup(cardSO);
        cardNameInput.text = cardSO.name;
        manaScrollingNumber.Setup(cardSO.GetManaCost(), 0, 99);

        remainPointText.SetInitValue(initPoint);
        remainPoint.Subscribe(x =>
        {
            var diff = x - initPoint;
            remainPointText.SetTargetValue(x);

            diffPointText.gameObject.SetActive(diff != 0);
            diffPointText.SetTargetValueWithDiff(diff);

        }).AddTo(this);

        remainPoint.Value = initPoint;

        if (cardManaSetting == null)
        {
            cardManaSetting = Resources.Load<CardManaSetting>("Settings");
        }

        CardEffectCostManager.Instance.SetCard(cardSO);
        CardEffectCostManager.Instance.SetCardManaSetting(cardManaSetting);

        cardManaList.Setup(cardManaSetting.CardManaCosts);
        cardManaList.Selecting.Subscribe(x =>
        {
            if (x == null) return;

            if (x.data.mana >= 0)
                manaScrollingNumber.SetVal(x.data.mana);
            else
                manaScrollingNumber.SetVal("X");

            UpdateRemainPoint();
        }).AddTo(this);
        cardManaList.SetSelecting(cardManaSetting.CardManaCosts.First(x => x.mana == cardSO.GetManaCost()));

        cardEffects = CardEffectManager.Instance.CardEffectList;

        cardEffectList.Setup(cardEffects);
        cardEffectList.Selecting.Subscribe(x =>
        {
            if (selectingSlot.Value != null && !cardEffectList.IsClosed())
            {
                if (x == null)
                {
                    selectingSlot.Value.SetCardEffect(null);
                    return;
                }

                if (selectingSlot.Value.cardEffect == null || !selectingSlot.Value.cardEffect.Compare(x.data))
                    selectingSlot.Value.SetCardEffect(x.data);

                UpdateRemainPoint();
            }

        }).AddTo(this);

        cardEffectList.EffectValue().Subscribe(x =>
        {
            if (selectingSlot.Value != null && selectingSlot.Value.cardEffect != null)
            {
                selectingSlot.Value.SetEffectValue(x);
                UpdateRemainPoint();
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
        }).AddTo(this);

        editableNoticeToggle.OnValueChangedAsObservable().Subscribe(x =>
        {
            SetActiveAllEditableNotice(x);
        }).AddTo(this);

        for (int i = 0; i < cardSO.effects.Count; i++)
        {
            if (i >= effectSlots.Count) break;

            var e = cardSO.effects[i];

            effectSlots[i].SetCardEffect(e, true);
        }

        IsInited = true;
    }

    public void SetInitPoint(int val)
    {
        initPoint = val;
        remainPointText.SetInitValue(val);
        remainPoint.Value = val;
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
    }

    public List<CardEffect> GetCardEffects()
    {
        return cardEffects;
    }

    public void UpdateRemainPoint()
    {
        var counter = 0;

        counter += cardManaList.Selecting.Value.GetCost();

        foreach(var slot in effectSlots)
        {
            if (slot.cardEffect == null) continue;

            counter += CardEffectCostManager.Instance.GetEffectCost(slot.cardEffect);
        }

        remainPoint.Value = initPoint - counter;
    }

    public void OnClickContinue()
    {
        if(remainPoint.Value < 0)
        {
            UIManager.Instance.ShowNoticeBar("Not enough points!!");
            return;
        }

        UIManager.Instance.ShowConfirmDialog("Confirm to make changes?").Subscribe(x =>
        {
            if(x)
            {
                SaveChanges();
            }

        }).AddTo(this);
    }

    void SaveChanges()
    {
        var newCard = Instantiate(cardSO);

        newCard.SetBaseManaCost(cardManaList.Selecting.Value.data.mana);
        newCard.name = cardNameInput.text;
        newCard.effects = effectSlots.Select(x => x.cardEffect).Where(x => x != null).ToList();

        CardLibrary.Instance.AddNewCard(newCard);

        int sec = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        AssetDatabase.CreateAsset(newCard, $"Assets/Resources/CardObject/PlayerCards/{newCard.name + "_" + sec}.asset");

        onCustomized?.Invoke(newCard);

        UIManager.Instance.ShowNoticeBar("Card customized successfully!!");

        Debug.Log("Card customized successfully!! Check on the scriptable object.");
    }
}
