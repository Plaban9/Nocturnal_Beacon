using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUnitData", menuName ="PlayerUnitData")]
public class PlayerUnitData : ScriptableObject
{
    [SerializeField] 
    private PlayableData _unitData;
    [SerializeField]
    private int _maxHP = -1;
    [SerializeField]
    private int _currentHp = -1;
    [SerializeField]
    private Deck _currentDeck;
    [SerializeField]
    private int _currency;
    [SerializeField]
    private int _maxMana;

    public void Setup(PlayableData unitData, int currency)
    {
        _unitData = unitData;
        _currentDeck = unitData.startingDeck;
        _currency = currency;
        _maxMana = unitData.startingMana;          
    }

    public UnitData GetUnitData()
    {
        return _unitData;
    }

    public int GetCurrentHP()
    {
        if (_currentHp == -1)
            _currentHp = _unitData.startingHp; 
        return _currentHp;
    }

    public int GetMaxHP()
    {
        if (_maxHP == -1)
            _maxHP = _unitData.startingHp;
        return _maxHP;
    }

    public void SetCurrentHp(int currentHp)
    {
        _currentHp = currentHp;
        if (_currentHp > _maxHP)
            _currentHp = _maxHP;
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

    public void InitDeck()
    {
        _currentDeck.InitDeck();
    }
}
