using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "CardManaCostSetting", menuName = "Card Mana Cost Setting")]
public class CardManaSetting : ScriptableObject
{
    public List<CardManaCost> CardManaCosts;

    public Dictionary<int, int> CardManaCostDict = new Dictionary<int, int>();

    public int GetCardManaCost(int mana)
    {
        if(CardManaCostDict.Count <= 0)
        {
            Init();
        }

        if (CardManaCostDict.ContainsKey(mana))
            return CardManaCostDict[mana];
        else
            return CardManaCosts.First(x => x.mana == mana).cost;
    }

    void Init()
    {
        foreach(var cmc in CardManaCosts)
        {
            CardManaCostDict[cmc.mana] = cmc.cost;
        }
    }
}

[System.Serializable]
public class CardManaCost
{
    public int mana;
    public int cost;
}