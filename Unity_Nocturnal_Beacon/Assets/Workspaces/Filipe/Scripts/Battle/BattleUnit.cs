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

    [SerializeField] BattleUnitData _unitData;

    [Header("Prefab Only")]
    [SerializeField] public SpriteRenderer _sprite;
    [SerializeField] public TextMeshProUGUI _hpText;
    [SerializeField] public SpriteRenderer _hpSprite;

    private void Start()
    {
        _hpData = GetComponent<HPData>();
        _battleUnit = GetComponent<BattleUnit>();

        _hpData.InitializeMaxHP(_unitData.startingHp);

        SetSprite();
        SetupHealth();
    }

    private void SetupHealth()
    {
        SpriteRenderer spr = _hpSprite.GetComponent<SpriteRenderer>();
        _hpData.SetupAssets(_hpText, spr.material);
    }

    private void SetSprite()
    {
        if (!_unitData.sprite) {
            throw (new Exception($"{_unitData.name} missing sprite."));
        }
        Texture2D texture = _unitData.sprite;
        Sprite newSprite = Sprite.Create(texture,
        new Rect(0, 0, texture.width, texture.height),
                                         new Vector2(0.5f, 0.5f));
        _sprite.sprite = newSprite;
    }
}
