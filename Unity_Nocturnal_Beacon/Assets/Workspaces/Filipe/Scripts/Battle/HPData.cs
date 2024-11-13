using DG.Tweening;
using Minimalist.Audio;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HPData : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _unitRenderer;
    private UnitData _unitData;
    private int _maxHp;
    private int _currentHp;
    private int _shield;
    private TextMeshProUGUI _hpText;
    private Material _hpMaterial;

    public void InitializeMaxHP(MonsterData _monsterData)
    {
        _unitData = _monsterData;
        _maxHp = _monsterData.startingHp;
        _currentHp = _monsterData.startingHp;
        _shield = 0;
    }

    public void InitializeMaxHp(PlayerUnitData _playerData)
    {
        _unitData = _playerData.GetUnitData();
        _maxHp = _playerData.GetMaxHP();
        _currentHp = _playerData.GetCurrentHP();
        _shield = 0;
    }
    public void DealDamage(int amount)
    {
        AudioManager.PlaySFX(Minimalist.Audio.Sound.SoundType.Player_Hit);

        _unitRenderer.color = new Color(1.0f, 0f, 0f);
        _unitRenderer.transform.parent.localScale = new Vector3(_unitData.scale, _unitData.scale * 0.1f, 1f);
        _unitRenderer.transform.parent.DOScale(_unitData.scale, 0.4f);
        if (_unitData is MonsterData)
        {
            _unitRenderer.DOColor((_unitData as MonsterData).recolor, 0.4f);
        }
        else
        {
            _unitRenderer.DOColor(Color.white, 0.3f);
        }

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
        AudioManager.PlaySFX(Minimalist.Audio.Sound.SoundType.Companion_DogBark);
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
        AudioManager.PlaySFX(Minimalist.Audio.Sound.SoundType.Companion_DogInteract);

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

        _hpText.text = sum.ToString();

        _hpMaterial.SetFloat("_pctHealthNShield", ((float)_currentHp)/((float)_maxHp));
        _hpMaterial.SetFloat("_pctShield", ((float) _shield) / ((float)_currentHp));
    }

    public bool IsDead()
    {
        if (_currentHp == 0)
        {
            if(_unitData is MonsterData)
            {
                _unitRenderer.DOColor(new Color(0f, 0.2f, 0f), 0.5f);
            }
            else
            {
                _unitRenderer.DOColor(new Color(0.2f, 0f, 0f), 0.5f);
            }
            _unitRenderer.transform.parent.DOScaleY(0f, 1.2f);

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
