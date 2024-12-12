using DG.Tweening;
using Minimalist.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPData : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _unitRenderer;
    private BattleUnit _battleUnit;
    private UnitData _unitData;
    private int _maxHp;
    private int _currentHp;
    private int _shield;
    private TextMeshProUGUI _hpText;
    private Material _hpMaterial;
    private bool isPlayer = false;

    public void InitializeMaxHP(MonsterData _monsterData, BattleUnit unit)
    {
        _unitData = _monsterData;
        _maxHp = _monsterData.maxHp;
        _currentHp = _monsterData.maxHp;
        _battleUnit = unit;
        _shield = 0;
    }

    public void InitializeMaxHp(PlayerUnitData _playerData, BattleUnit unit)
    {
        isPlayer = true;
        _unitData = _playerData.GetUnitData();
        _maxHp = _playerData.GetMaxHP();
        _currentHp = _playerData.GetCurrentHP();
        _battleUnit = unit;
        _shield = 0;
    }
    public void DealDamage(BattleUnit? damageOrigin, int amount, bool noReflect = false, float delay = 0f)
    {
        StartCoroutine(PerformDealDamage(damageOrigin, amount, noReflect, delay));
    }

    private IEnumerator PerformDealDamage(BattleUnit? damageOrigin, int amount, bool noReflect = false, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        int finalAmount = amount;
        foreach (BattleStatusEffect status in _battleUnit.GetUnitStatusData().GetStatusEffects())
        {
            if (status is StatusEffect_Thorns && noReflect)
            {
                // Avoid infinite damage rebound from thorns.
            }
            else
            {
                finalAmount = status.OnTakeDamage(damageOrigin, finalAmount);
            }
        }

        if (finalAmount == 0) yield break;
        _unitRenderer.color = new Color(1.0f, 0f, 0f);
        _unitRenderer.transform.parent.DOScaleY(_unitData.scale/5f, 0.1f);
        _unitRenderer.transform.parent.DOScaleY(_unitData.scale, 0.4f);
        if (_unitData is MonsterData)
        {
            _unitRenderer.DOColor((_unitData as MonsterData).recolor, 0.4f);
        }
        else
        {
            _unitRenderer.DOColor(Color.white, 0.3f);
        }

        amount = Mathf.Abs(finalAmount);
        if (_currentHp == 0) yield break;
        EffectManager.Instance.CreateNumber(
            EffectManager.EFFECTS_NUMBER.DAMAGE,
            transform.gameObject,
            amount);
        int hpDamage = amount;
        if (_shield > 0)
        {
            hpDamage -= _shield;
            if (hpDamage < 0)
                _shield = -hpDamage;
            else if (hpDamage >= 0)
                _shield = 0;
        }
        if (hpDamage > 0)
        {
            int result = _currentHp - hpDamage;
            if (result > 0)
            {
                _currentHp = result;
            }
            else
            {
                _currentHp = 0;
                BattleManager.Instance.CheckIfBattleIsOver();
            }
        }
        UpdateVisual();
    }

    public void RecoverHealth(int amount)
    {
        _unitRenderer.color = new Color(0.2f, 1.0f, 0.2f);
        if(_unitData is MonsterData)
        {
            _unitRenderer.DOColor((_unitData as MonsterData).recolor, 0.6f);
        }
        else
        {
            _unitRenderer.DOColor(Color.white, 0.6f);
        }


        EffectManager.Instance.CreateNumber(
            EffectManager.EFFECTS_NUMBER.HEAL,
            transform.gameObject,
            amount);
        int result = _currentHp + amount;
        if (result > _maxHp)
            _currentHp = _maxHp;
        else
            _currentHp = result;
        UpdateVisual();
    }

    public void AddShield(int amount)
    {

        _unitRenderer.color = new Color(0.2f, 0.2f, 1.0f);
        if (_unitData is MonsterData)
        {
            _unitRenderer.DOColor((_unitData as MonsterData).recolor, 0.5f);
        }
        else
        {
            _unitRenderer.DOColor(Color.white, 0.5f);
        }

        EffectManager.Instance.CreateNumber(
        EffectManager.EFFECTS_NUMBER.GUARD,
        transform.gameObject,
        amount);
        _shield += amount;
        UpdateVisual();
    }

    public void RemoveShield(int amount)
    {
        int displayedValue = _shield;
        _shield -= amount;
        if (_shield >= 0)
            displayedValue = amount; 
        EffectManager.Instance.CreateNumber(
        EffectManager.EFFECTS_NUMBER.CORROSION,
        transform.gameObject,
        displayedValue);
        if (_shield < 0) _shield = 0; ;
        UpdateVisual();
    }

    public void SetupAssets(SpriteRenderer unitRenderer, TextMeshProUGUI text, Material hpMaterial)
    {
        _unitRenderer = unitRenderer;
        _hpMaterial = hpMaterial;
        _hpText = text;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        int sum = _currentHp + _shield;
        _hpText.color = Color.yellow;
        if (_shield > 0)
            _hpText.color = Color.cyan;

        int curHp = int.Parse(_hpText.text);
        DOTween.To(() => curHp,
            x => curHp = x, sum, 0.5f).OnUpdate(() =>
            {
                _hpText.text = $"{curHp}"; 
            }
        );


        float curHpPercent = _hpMaterial.GetFloat("_pctHealthNShield");
        float curShPercent = _hpMaterial.GetFloat("_pctShield");
        float maxHPPercent = ((float)_currentHp) / ((float)_maxHp);
        float maxSHPercent = ((float)_shield) / ((float) _currentHp);

        Debug.Log($"{curHpPercent} -> {maxHPPercent}, {curShPercent} -> {maxSHPercent}");

        DOTween.To(() => curHpPercent,
            x => curHpPercent = x , maxHPPercent, 0.5f).OnUpdate(() =>
            {
                _hpMaterial.SetFloat("_pctHealthNShield", curHpPercent);
            }
        );
        DOTween.To(() => curShPercent,
            x => curShPercent = x, maxSHPercent, 0.5f).OnUpdate(() =>
            {
                _hpMaterial.SetFloat("_pctShield",curShPercent);
            }
        );

        if (isPlayer)
        {
            NoctBeaconRunData.Instance.SetHp(curHp);
        }

    }

    public bool IsDead()
    {
        if (_currentHp == 0)
        {
            ;
            return true;
        }
        return false;
    }

    public void EndTurnFlushShield()
    {
        _shield = 0;
        UpdateVisual();
    }

    public int GetCurrentHP()
    {
        return _currentHp;
    }
}
