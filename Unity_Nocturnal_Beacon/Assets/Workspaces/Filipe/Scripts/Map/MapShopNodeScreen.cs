using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class MapShopNodeScreen : MapNonBattleNodeScreen
{
    public override void ActivateNonBattleNodeScreen()
    {
        _manager.HideContinue();
        UIManager.Instance.ShowPage(GamePage.ShopPage);
    }

    public override void DeactivateNonBattleNodeScreen()
    {
        _manager.HideContinue();
    }
}
