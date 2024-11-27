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
            Refresh();
        }).AddTo(this);
    }

    public void SetPrice(int price) => this.price.Value = price;

    public int GetPrice() => price.Value;

    public void Refresh()
    {
        if (NoctBeaconRunData.Instance.GetGold() >= price.Value)
            priceText.text = price.Value.ToString();
        else
            priceText.text = $"<color=#FF5F00>{price.Value.ToString()}</color>";
    }
}
