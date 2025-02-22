using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class MapShopNodeScreen : MapNonBattleNodeScreen
{
    public override void ActivateNonBattleNodeScreen()
    {
        _manager.HideContinue();
        var sp = UIManager.Instance.ShowPage(GamePage.ShopPage).GetComponent<ShopPage>();
        sp.Setup();
    }

    public override void DeactivateNonBattleNodeScreen()
    {
        _manager.HideContinue();
    }
}
