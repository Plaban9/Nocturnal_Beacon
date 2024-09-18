using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitData : ScriptableObject
{
    [Header("Unit Data")]
    [SerializeField] public string unitName = "Monster Name";
    [SerializeField] public Texture2D sprite;
    [SerializeField] public int startingHp = 100;

}
