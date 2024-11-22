using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPage : MonoBehaviour
{
    [SerializeField] ShopAnimator shopAnimator;

    // Start is called before the first frame update
    void Start()
    {
        shopAnimator.StartAnimate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickShowShopItemPage()
    {
        Debug.Log("OnClickShowShopItemPage");
    }
}
