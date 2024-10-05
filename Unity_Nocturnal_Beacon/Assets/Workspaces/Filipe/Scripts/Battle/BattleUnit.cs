using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    private HPData _hpData;
    private BattleUnit _battleUnit;

    [SerializeField] UnitData _unitData;
    [SerializeField] PlayerUnitData _playerUnitData;

    [Header("Prefab Only")]
    [SerializeField] public SpriteRenderer _sprite;
    [SerializeField] public TextMeshProUGUI _hpText;
    [SerializeField] public SpriteRenderer _hpSprite;
    [SerializeField] public TextMeshProUGUI _name;

    private void Awake()
    {
        _hpData = GetComponent<HPData>();
        _battleUnit = GetComponent<BattleUnit>();
    }

    public void SetupUnit(UnitData _unitData)
    {
        this._unitData = _unitData; 
        _hpData.InitializeMaxHP(_unitData);
        SetUnitVisuals();
        SetupHealth();
    }

    public void SetupPlayerUnit(PlayerUnitData _playerUnitData)
    {
        _hpData.InitializeMaxHp(_playerUnitData);
        _unitData = _playerUnitData.GetUnitData();
        SetUnitVisuals();
        SetupHealth();
    }
    private void SetupHealth()
    {
        SpriteRenderer spr = _hpSprite.GetComponent<SpriteRenderer>();
        _hpData.SetupAssets(_hpText, spr.material);
    }

    private void SetUnitVisuals()
    {
        if (!_unitData.sprite) {
            throw (new Exception($"{_unitData.name} missing sprite."));
        }
        Texture2D texture = _unitData.sprite;
        Sprite newSprite = Sprite.Create(texture,
        new Rect(0, 0, texture.width, texture.height),
                                         new Vector2(0.5f, 0.5f));
        _sprite.sprite = newSprite;

        _name.text = _unitData.name;
    }

    public HPData GetHPData()
    {
        return _hpData;
    }

    public UnitData GetUnitData()
    {
        return _unitData;
    }
    
}
