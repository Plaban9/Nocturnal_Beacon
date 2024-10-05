using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewPlayableData", menuName = "Unit/Playable")]
[Serializable]
public class PlayableData : UnitData
{

    [Header("Playable Character Data")]
    [SerializeField] public Deck startingDeck;
    [SerializeField] public int startingMana;

    // Start is called before the first frame update

}
