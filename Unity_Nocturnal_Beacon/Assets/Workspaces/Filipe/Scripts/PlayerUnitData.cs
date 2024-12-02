using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

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
    [SerializeField]
    private int _currentRun;

    public void Setup(PlayableData unitData, int currency)
    {
        _unitData = unitData;
        _currentDeck = new Deck(true);
        _currentDeck.CloneFromDeck(unitData.startingDeck);

        // Create a new deck and save to local
        var newDeck = Instantiate(_currentDeck);
        int sec = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        newDeck.name = unitData.startingDeck.name + "_" + sec;

#if UNITY_EDITOR
        AssetDatabase.CreateAsset(newDeck, $"Assets/Resources/Deck/{newDeck.name}.asset");
#else
        ScriptableObjectSaver.SaveScriptableObject(newDeck.ToJson(), "PlayerDecks");
#endif

        _currentDeck = newDeck;
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
            _currentHp = _unitData.maxHp; 
        return _currentHp;
    }

    public int GetMaxHP()
    {
        if (_maxHP == -1)
            _maxHP = _unitData.maxHp;
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

    public void ModifyCurrency(int val) => _currency += val;

    public bool TryPurchase(int price)
    {
        if (_currency - price < 0) return false;

        _currency -= price;

        return true;
    }
}
