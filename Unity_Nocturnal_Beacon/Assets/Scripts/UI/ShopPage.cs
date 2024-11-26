using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPage : MonoBehaviour
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
}
