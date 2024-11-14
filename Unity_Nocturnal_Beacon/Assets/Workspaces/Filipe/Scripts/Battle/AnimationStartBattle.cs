using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStartBattle : MonoBehaviour
{
    [SerializeField] BattleManager battleManager;

    public void StartBattle()
    {
        battleManager.StartBattle(); 
    }
}
