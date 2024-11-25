using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class MapShopNodeScreen : MapNonBattleNodeScreen
{
    

    [Header("Assets")]
    [SerializeField] Button _shopButton;

    public override void ActivateNonBattleNodeScreen()
    {
        _manager.ShowContinue();
        _manager.SetProgressSkip();
    }


    public override void DeactivateNonBattleNodeScreen()
    {
        _manager.HideContinue();
    }
}
