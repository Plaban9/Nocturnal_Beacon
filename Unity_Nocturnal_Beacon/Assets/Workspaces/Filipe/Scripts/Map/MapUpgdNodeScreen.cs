using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class MapUpgdNodeScreen : MapNonBattleNodeScreen
{

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
