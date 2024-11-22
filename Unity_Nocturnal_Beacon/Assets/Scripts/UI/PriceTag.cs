using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PriceTag : MonoBehaviour
{
    [SerializeField] Image currencyIcon;
    [SerializeField] TextMeshProUGUI priceText;

    [SerializeField] ReactiveProperty<int> price = new ReactiveProperty<int>(0);

    private void Awake()
    {
        price.Subscribe(x =>
        {
            priceText.text = x.ToString();
        }).AddTo(this);
    }

    public void SetPrice(int price) => this.price.Value = price;

    public int GetPrice() => price.Value;
}
