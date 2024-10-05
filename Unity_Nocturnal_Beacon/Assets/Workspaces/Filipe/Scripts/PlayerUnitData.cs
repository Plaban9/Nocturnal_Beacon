using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUnitData", menuName ="PlayerUnitData")]
public class PlayerUnitData : ScriptableObject
{
    [SerializeField] 
    private UnitData _unitData;
    [SerializeField]
    private int _maxHP;
    [SerializeField]
    private int _currentHp;
    [SerializeField]
    private Deck _currentDeck;
    [SerializeField]
    private int _currency;
    [SerializeField]
    private int _maxMana;

    public UnitData GetUnitData()
    {
        return _unitData;
    }

    public int GetCurrentHP()
    {
        return _currentHp;
    }

    public int GetMaxHP()
    {
        return _maxHP;
    }

    public Deck GetCurrentDeck()
    {
        return _currentDeck;
    }

    public int GetCurrency()
    {
        return _currency;
    }

    public int GetMaxMana()
    {
        return _maxMana;
    }
}
