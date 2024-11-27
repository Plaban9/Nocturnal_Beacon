using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapNonBattleNodeScreen : MonoBehaviour
{
    public MapNonBattleNodeManager _manager { protected get; set; }

    public abstract void ActivateNonBattleNodeScreen();


    public abstract void DeactivateNonBattleNodeScreen();


    [ContextMenu("Activate")]
    public void Activate()
    {
        ActivateNonBattleNodeScreen();
    }

    [ContextMenu("Deativate")]
    public void Deactivate()
    {
        DeactivateNonBattleNodeScreen();
    }

}
