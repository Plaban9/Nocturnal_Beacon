using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Collections;
using UnityEngine;

public class ShopPage : CommonPage
{
    [SerializeField] Deck shopItems = null;

    [SerializeField] public bool SkipAnimation = false;
    [SerializeField] ShopAnimator shopAnimator;

    // Start is called before the first frame update
    void Start()
    {
        if (!SkipAnimation)
            shopAnimator.StartAnimate();
        else
            shopAnimator.SkipAnimate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(Deck shopItems = null)
    {
        if(shopItems != null)
            this.shopItems = shopItems;
    }

    public void OnClickShowShopItemPage()
    {
        Debug.Log("OnClickShowShopItemPage");

        var sip = UIManager.Instance.ShowPage(GamePage.ShopItemPage).GetComponent<ShopItemPage>();
        
        if (sip != null)
        {
            sip.Setup(shopItems);
            sip.Show();
        }
    }

    public void OnClickLeaveButton()
    {
        shopAnimator.LeaveAnimate().Subscribe(x =>
        {
            if(x)
            {
                Close();
            }
        }).AddTo(this);
    }
}
