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

public class BattleUnit : MonoBehaviour
{
    private HPData _hpData;
    private UnitStatusData _statusEffectData;
    private BattleUnit _battleUnit;

    [SerializeField] UnitData _unitData;
    [SerializeField] PlayerUnitData _playerUnitData;

    [Header("Prefab Only")]
    [SerializeField] public SpriteRenderer _sprite;
    private Material _materialSprite;
    [SerializeField] public TextMeshProUGUI _hpText;
    [SerializeField] public SpriteRenderer _hpSprite;
    [SerializeField] public TextMeshProUGUI _name;

    [Header("Elemental Affinity")]
    [SerializeField] public CanvasGroup _effectivityWindow;
    [SerializeField] public TextMeshProUGUI _sign;
    [SerializeField] public TextMeshProUGUI _effectivity;
    [SerializeField] public Image _color;


    [Header("Only for Enemies")]
    [SerializeField] public GameObject _intentHolder;
    [SerializeField] public GameObject _intentObject;
    [SerializeField] public GameObject _intentObject2;
    [SerializeField] public GameObject _intentObject3;

    Animator _animator;

    private void Awake()
    {
        _hpData = GetComponent<HPData>();
        _statusEffectData = GetComponent<UnitStatusData>();
        _battleUnit = GetComponent<BattleUnit>();
        _materialSprite = _sprite.material;
        _animator = GetComponent<Animator>();
    }

    public void SetupUnit(MonsterData _monsterData)
    {
        _unitData = _monsterData; 
        _hpData.InitializeMaxHP(_monsterData, this);
        SetUnitVisuals();
        SetupHealth();
    }

    public void SetupPlayerUnit(PlayerUnitData _playerUnitData)
    {
        _hpData.InitializeMaxHp(_playerUnitData, this);
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

    public void SetNextTurnIntent(int intentSlot, Card card, int turnOrder)
    {
        GameObject intentObj = GetIntentSlot(intentSlot);

        intentObj.GetComponent<EnemyCardIntentHoverable>().SetCard(card);
        intentObj.GetComponent<EnemyCardIntentHoverable>().SetTurnOrder(turnOrder);

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

    public void Outline()
    {
        float outline = _materialSprite.GetFloat("_OutlineThickness");
        DOTween.To(() => outline,
        x => outline = x, 2f, 0.2f).OnUpdate(() =>
        {
            _materialSprite.SetFloat("_OutlineThickness", outline);
        }
        );
       
    }

    public void HideOutline()
    {
        float outline = _materialSprite.GetFloat("_OutlineThickness");
        DOTween.To(() => outline,
        x => outline = x, 0f, 0.2f).OnUpdate(() =>
        {
            _effectivityWindow.alpha = outline / 2f;
            _materialSprite.SetFloat("_OutlineThickness", outline);
        }
        );
    }

    public void ShowEffectivity(Card card)
    {
        _effectivityWindow.DOFade(1f, 0.2f);
        float getEffectiveness = card.GetAffinity(GetUnitData().unitElement);
        switch (getEffectiveness)
        {
            case 0.5f:
                _sign.text = "-";
                _sign.fontSize = 36;
                _effectivity.text = "RESISTS";
                _color.color = new Color(0.5f, 0.5f, 0.9f);
                break;
            case 0.75f:
                _sign.text = "-";
                _sign.fontSize = 24;
                _effectivity.text = "INEFFECTIVE";
                _color.color = new Color(0.5f, 0.3f, 0.7f);
                break;
            case 1.0f:
                _sign.text = "~";
                _sign.fontSize = 24;
                _effectivity.text = "NEUTRAL";
                _color.color = new Color(0.5f, 0.5f, 0.5f);
                break;
            case 1.25f:
                _sign.text = "+";
                _sign.fontSize = 24;
                _effectivity.text = "EFFECTIVE";
                _color.color = new Color(0.7f, 0.65f, 0.64f);

                break;
            case 1.5f:
                _sign.text = "+";
                _sign.fontSize = 36;
                _effectivity.text = "VRY EFFECTIVE";
                _color.color = new Color(0.8f, 0.35f, 0.38f);
                break;
            case 2.0f:
                _sign.text = "+";
                _sign.fontSize = 48;
                _effectivity.text = "MAX EFFECTIVE";
                _color.color = new Color(0.86f, 0.3f, 0.3f);
                break;
        }

    }

    public void HideEffectivity()
    {
        _effectivityWindow.DOFade(0f, 0.2f);

    }

    public bool IsDead() {
        bool IsDead = _hpData.IsDead();
        if( IsDead)
        {
            _intentHolder.transform.parent.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);
            if (_unitData is MonsterData)
            {
                _sprite.DOColor(new Color(0f, 0.2f, 0f), 0.5f);
            }
            else
            {
                _sprite.DOColor(new Color(0.2f, 0f, 0f), 0.5f);
            }
            _sprite.transform.parent.DOScaleZ(0f, 1.2f).onComplete = ()=>{
                this.gameObject.SetActive(false);
            };

        }
        return IsDead;
    }

    public void PlayAttackAnimation()
    {
        String anim = (_unitData is MonsterData ? "MonsterAttack" : "PlayerAttack");
        _animator.Play(anim);
    }

    public void PlaySkillAnimation()
    {
        String anim = (_unitData is MonsterData ? "MonsterUseSkill" : "PlayerUseSkill");
        _animator.Play(anim);
    }
}
