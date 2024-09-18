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
    public void InitializeMaxHP(int maxHP)
    {
        _maxHp = maxHP;
        _currentHp = maxHP;
        _shield = 0;
    }
    public void DealDamage(int amount)
    {
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
}
