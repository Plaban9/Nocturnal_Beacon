using CardAttribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : ScriptableObject
{
    [Header("Unit Data")]
    [SerializeField] public string unitName ;
    [SerializeField] public Sprite sprite;
    [SerializeField] public int startingHp = 100;


    [SerializeField] public bool flipSprite = true;
    [SerializeField] public float scale = 13f;

    [SerializeField] public Element unitElement = Element.NONE;
}
