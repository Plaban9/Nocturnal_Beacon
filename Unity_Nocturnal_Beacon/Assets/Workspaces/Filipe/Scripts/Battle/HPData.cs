using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPData : MonoBehaviour
{
    private int _maxHp;
    private int _currentHp;
    private int _shield;
    private TextMeshProUGUI _hpText;
    private Material _hpMaterial;

    public void InitializeMaxHP(UnitData _monsterData)
    {
        _maxHp = _monsterData.startingHp;
        _currentHp = _monsterData.startingHp;
        _shield = 0;
    }

    public void InitializeMaxHp(PlayerUnitData _playerData)
    {
        _maxHp = _playerData.GetMaxHP();
        _currentHp = _playerData.GetCurrentHP();
        _shield = 0;
    }
    public void DealDamage(int amount)
    {
        amount = Mathf.Abs(amount);
        if (_currentHp == 0) return;
        EffectManager.Instance.CreateNumber(
            EffectManager.EFFECTS_NUMBER.DAMAGE,
            transform.gameObject,
            amount);
        int hpDamage = amount;
        if(_shield > 0)
        {
            hpDamage -= _shield;
            if (hpDamage < 0)
                _shield = -hpDamage;
            else if (hpDamage > 0)
                _shield -= hpDamage;
            else
                _shield = 0;
        }
        if(hpDamage > 0)
        {
            int result = _currentHp - hpDamage; 
            if(result > 0)
            {
                _currentHp = result;
            }
            else
            {
                _currentHp = 0;
            }
        }
        UpdateVisual();
    }

    public void RecoverHealth(int amount)
    {
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

    public void SetupAssets(TextMeshProUGUI text, Material hpMaterial)
    {
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

        _hpText.text = sum.ToString();

        _hpMaterial.SetFloat("_pctHealthNShield", ((float)_currentHp)/((float)_maxHp));
        _hpMaterial.SetFloat("_pctShield", ((float) _shield) / ((float)_currentHp));
    }

    public bool IsDead()
    {
        if (_currentHp == 0) return true;
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
