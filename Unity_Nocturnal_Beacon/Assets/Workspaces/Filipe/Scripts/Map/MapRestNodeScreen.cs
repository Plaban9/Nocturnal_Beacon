using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public  class MapRestNodeScreen : MapNonBattleNodeScreen
{

    [Header("Configurations")]
    [Range(0f, 1f)]
    [SerializeField] float _percentageHealed = 0.3f;
    [Header("Assets")]
    [SerializeField] Image _hpImage;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Button _restButton;
    [SerializeField] Button _upgradeButton;

    private Animator _animator;
    private Material _hpMaterial;
    private bool _chose= false;
    private PlayerUnitData _runData;

    private void Start()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
        _restButton.onClick.AddListener(
            ()=>{ 
                UseRest(_percentageHealed); 
            });
        _upgradeButton.onClick.AddListener(
            () =>
            {
                UseUpgrade();
            });
        _hpMaterial = _hpImage.GetComponent<Material>();
    }

    private void UseRest(float amount)
    {
        if (!_chose)
        {
            _manager.HideContinue();
            _chose = true;
            DisableButtons();

            _runData = NoctBeaconRunData.Instance.GetPlayerInformation();
            int newAmount = (int) Mathf.Floor( (float) _runData.GetCurrentHP() + ( (float) _runData.GetMaxHP() * amount));
            _runData.SetCurrentHp(newAmount);
            SetHP(_runData, () =>
            {
                _manager.ShowContinue();
                _manager.SetProgressContinue();
            });
        }

    }
    private void UseUpgrade()
    {
        if (!_chose)
        {
            _chose = true;
            DisableButtons();


        }
    }

    private void DisableButtons()
    {
        _restButton.interactable = false;
        _upgradeButton.interactable = false;
    }

    private void EnableButtons()
    {
        _restButton.interactable = true;
        _upgradeButton.interactable = true;
    }

    private void SetupHPData(PlayerUnitData pud)
    {
        _hpMaterial.SetFloat("_pctShield", 0f);
        SetHP(pud);

        if (pud.GetMaxHP() == pud.GetCurrentHP()) _restButton.interactable = false;
    }

    private void SetHP(PlayerUnitData pud, Action doOnAnimationFinish = null)
    {
        float curHpPercent = 0f;
        float maxHPPercent = ((float)pud.GetCurrentHP()) / ((float)pud.GetMaxHP());

        int curHp = int.Parse(_text.text);
        DOTween.To(() => curHp,
            x => curHp = x, pud.GetCurrentHP(), 1.5f).OnUpdate(() =>
            {
                _text.text = $"{curHp}";
            }
        );

        DOTween.To(() => curHpPercent,
            x => curHpPercent = x, maxHPPercent, 1.5f).OnUpdate(() =>
            {
                _hpMaterial.SetFloat("_pctHealthNShield", curHpPercent);
            }
        ).onComplete = () => { doOnAnimationFinish?.Invoke();
        };
    }


    public override void ActivateNonBattleNodeScreen()
    {
        _manager.SetProgressSkip();
        _manager.ShowContinue();
        _animator.Play("RestOpen");
        _chose = false;
        _runData = NoctBeaconRunData.Instance.GetPlayerInformation();
        EnableButtons();

    }


    public override void DeactivateNonBattleNodeScreen()
    {
        _manager.HideContinue();
        _animator.Play("RestClose");
    }
}
