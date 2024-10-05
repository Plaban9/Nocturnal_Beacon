using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : ScriptableObject
{
    [Header("Unit Data")]
    [SerializeField] public string unitName ;
    [SerializeField] public Texture2D sprite;
    [SerializeField] public int startingHp = 100;

}
