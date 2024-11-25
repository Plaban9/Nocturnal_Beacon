using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStatus_RandomCost : CardStatuses
{
    int manaCostVariation = Random.Range(0, 3);
    public override int GetManaCost(int i)
    {
        return manaCostVariation;
    }
}
