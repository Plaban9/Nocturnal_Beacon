using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BattleUnit : MonoBehaviour
{
    private HPData _hpData;
    private UnitStatusData _statusEffectData;
    private BattleUnit _battleUnit;

    [SerializeField] UnitData _unitData;
    [SerializeField] PlayerUnitData _playerUnitData;

    [Header("Prefab Only")]
    [SerializeField] public SpriteRenderer _sprite;
    [SerializeField] public TextMeshProUGUI _hpText;
    [SerializeField] public SpriteRenderer _hpSprite;
    [SerializeField] public TextMeshProUGUI _name;

    [Header("Only for Enemies")]
    [SerializeField] public GameObject _intentHolder;
    [SerializeField] public GameObject _intentObject;
    [SerializeField] public GameObject _intentObject2;
    [SerializeField] public GameObject _intentObject3;

    private void Awake()
    {
        _hpData = GetComponent<HPData>();
        _statusEffectData = GetComponent<UnitStatusData>();
        _battleUnit = GetComponent<BattleUnit>();

    }

    public void SetupUnit(MonsterData _monsterData)
    {
        _unitData = _monsterData; 
        _hpData.InitializeMaxHP(_monsterData);
        SetUnitVisuals();
        SetupHealth();
    }

    public void SetupPlayerUnit(PlayerUnitData _playerUnitData)
    {
        _hpData.InitializeMaxHp(_playerUnitData);
        _unitData = _playerUnitData.GetUnitData();
        SetUnitVisuals();
        SetupHealth();
        _intentHolder.SetActive(false);
    }
    private void SetupHealth()
    {
        SpriteRenderer spr = _hpSprite.GetComponent<SpriteRenderer>();
        _hpData.SetupAssets(_sprite, _hpText, spr.material);
    }

    private void SetUnitVisuals()
    {
        if (!_unitData.sprite) {
            throw (new Exception($"{_unitData.name} missing sprite."));
        }
        _sprite.sprite = _unitData.sprite;
        _sprite.transform.parent.localScale = new Vector3(_unitData.scale, 1f, _unitData.scale);
        _sprite.flipX = _unitData.flipSprite;

        if(_unitData is MonsterData)
        {
            MonsterData _mdata = _unitData as MonsterData;
            _intentObject.SetActive(false); 
            _intentObject2.SetActive(false);
            _intentObject3.SetActive(false);
            _sprite.color = _mdata.recolor;
        }

        _name.text = _unitData.unitName;
    }

    public HPData GetHPData()
    {
        return _hpData;
    }

    public UnitData GetUnitData()
    {
        return _unitData;
    }

    public UnitStatusData GetUnitStatusData()
    {
        return _statusEffectData;
    }


    //Monster bullshit
    private GameObject GetIntentSlot(int intentSlot)
    {
        return intentSlot switch
        {
            2 => _intentObject3,
            1 => _intentObject2,
            _ => _intentObject,
        };
    }

    public void SetNextTurnIntent(int intentSlot, Card card)
    {
        GameObject intentObj = GetIntentSlot(intentSlot);

        intentObj.GetComponent<EnemyCardIntentHoverable>().SetCard(card);
        intentObj.GetComponent<CardDisplay>().Setup(card);
    }

    public void HighlightIntent(int intentSlot)
    {
        GameObject intentObj = GetIntentSlot(intentSlot);
        intentObj.transform.DOScale(0.6f, 0.3f);
        StartCoroutine(Revert(intentObj));
    }

    public IEnumerator Revert(GameObject intentObj)
    {
        yield return new WaitForSeconds(0.5f);
        intentObj.transform.DOScale(0.5f, 0.3f);
    }

    public void ShowIntent(int intentSlot)
    {
        GameObject intentObj = GetIntentSlot(intentSlot);
        intentObj.SetActive(true); 
    }

    public void HideIntent(int intentSlot)
    {
        GameObject intentObj = GetIntentSlot(intentSlot);
        intentObj.SetActive(false);
    }

}
