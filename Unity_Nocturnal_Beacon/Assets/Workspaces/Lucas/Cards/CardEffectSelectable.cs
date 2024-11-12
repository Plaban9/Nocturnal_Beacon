using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

public class CardEffectSelectable : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI effectText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] GameObject onSelectGO;
    
    public CardEffect cardEffect { get; private set; }
    public Subject<CardEffect> onClick { get; private set; }

    int cost = 0;

    public int GetCost() => cost;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(CardEffect cardEffect, int cost = 3)
    {
        this.cardEffect = cardEffect;
        this.cost = cost;
        onClick = new Subject<CardEffect>();

        effectText.text = cardEffect.EffectDescription;
        costText.text = cost.ToString();
    }

    public void SetCost(int cost)
    {
        this.cost = cost;
    }

    public void SetSelecting(bool set)
    {
        onSelectGO.SetActive(set);
    }

    public void OnClick()
    {
        onClick.OnNext(cardEffect);
    }

    public void UpdateInfo()
    {
        effectText.text = cardEffect.EffectDescription;
        costText.text = cost.ToString();
    }
}
