using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardManaCostSetting", menuName = "Card Mana Cost Setting")]
public class CardManaSetting : ScriptableObject
{
    public List<CardManaCost> CardManaCosts;
   
}

[System.Serializable]
public class CardManaCost
{
    public int mana;
    public int cost;
}