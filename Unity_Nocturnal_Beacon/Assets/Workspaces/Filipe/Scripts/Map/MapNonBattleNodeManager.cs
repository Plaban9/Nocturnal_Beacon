using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class MapNonBattleNodeManager : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] private GameObject _restCanvas;
    [SerializeField] private GameObject _shopCanvas;
    [SerializeField] private GameObject _upgdCanvas;
    [SerializeField] private GameObject _evntCanvas;

    [Header("Configurations")]
    [SerializeField] private float _fadeAnimDuration = 0.5f;

    private NonBattleNodeTypes _currentActive = NonBattleNodeTypes.NONE;
    private GameObject _continueButton;
    private CanvasGroup _continueButtonCanvasGroup;
    private TextMeshProUGUI _continueButtonText;

    private Action<Boolean> _doOnEventConcluded = (skipped) => { };
    private bool _skipping = false;

    void Start()
    {
        _continueButton = transform.Find("ContinueButton").gameObject;
        _continueButtonText = _continueButton.transform.Find("ContinueText").gameObject.GetComponent<TextMeshProUGUI>();
        _continueButton.GetComponent<Button>().onClick.AddListener(() => { OnClickContinue(); });
        _continueButtonCanvasGroup = _continueButton.GetComponent<CanvasGroup>();

        _restCanvas.GetComponent<MapRestNodeScreen>()._manager = this;
        _shopCanvas.GetComponent<MapShopNodeScreen>()._manager = this;
        _upgdCanvas.GetComponent<MapUpgdNodeScreen>()._manager = this;
        _evntCanvas.GetComponent<MapEvntNodeScreen>()._manager = this;


    }

    #region Continue Button
    public void SetOnContinueCallback(Action<bool> _onContinue)
    {
        _doOnEventConcluded = _onContinue;
    }

    public void ShowContinue()
    {
        _continueButtonCanvasGroup.DOFade(1.0f, _fadeAnimDuration);
        _continueButtonCanvasGroup.interactable = true;
        _continueButtonCanvasGroup.blocksRaycasts = true;
    }

    public void HideContinue()
    {
        _continueButtonCanvasGroup.DOFade(0.0f, _fadeAnimDuration);
        _continueButtonCanvasGroup.interactable = false;
        _continueButtonCanvasGroup.blocksRaycasts = false;
    }

    public void SetProgressContinue()
    {
        _continueButtonText.text = "Continue";
        _skipping = false;
    }

    public void SetProgressSkip()
    {
        _continueButtonText.text = "Skip";
        _skipping = true;
    }

    private void OnClickContinue()
    {
        _doOnEventConcluded(_skipping);
        SetEvent(NonBattleNodeTypes.NONE);
    }

    #endregion

    #region Event Canvas Handling

    public void SetEvent(NonBattleNodeTypes type)
    {
        if(_currentActive != NonBattleNodeTypes.NONE)
        {
            DeactivateEvent(_currentActive);
        }
        
        _currentActive = type;

        switch (type)
        {
            case NonBattleNodeTypes.REST:
                ActivateScreen(_restCanvas);
                break;
            case NonBattleNodeTypes.SHOP:
                ActivateScreen(_shopCanvas);
                break;
            case NonBattleNodeTypes.UPGD:
                ActivateScreen(_upgdCanvas);
                break;
            case NonBattleNodeTypes.EVNT:
                ActivateScreen(_evntCanvas);
                break;
        }
    }

    private void DeactivateEvent(NonBattleNodeTypes type)
    {
        switch (type)
        {
            case NonBattleNodeTypes.REST:
                DeactivateScreen(_restCanvas);
                break;
            case NonBattleNodeTypes.SHOP:
                DeactivateScreen(_shopCanvas);
                break;
            case NonBattleNodeTypes.UPGD:
                DeactivateScreen(_upgdCanvas);
                break;
            case NonBattleNodeTypes.EVNT:
                DeactivateScreen(_evntCanvas);
                break;
        }
    }

    private void ActivateScreen(GameObject screen)
    {
        CanvasGroup canvasGroup = screen.GetComponent<CanvasGroup>();
        MapNonBattleNodeScreen mapNonBattleNodeScreen = screen.GetComponent<MapNonBattleNodeScreen>();

        mapNonBattleNodeScreen.ActivateNonBattleNodeScreen();
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1f, _fadeAnimDuration);
    }

    private void DeactivateScreen(GameObject screen)
    {
        CanvasGroup canvasGroup = screen.GetComponent<CanvasGroup>();
        MapNonBattleNodeScreen mapNonBattleNodeScreen = screen.GetComponent<MapNonBattleNodeScreen>();

        mapNonBattleNodeScreen.DeactivateNonBattleNodeScreen();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0f, _fadeAnimDuration);
    }

    #endregion
}

public enum NonBattleNodeTypes
{
    NONE = -1,
    REST,
    SHOP,
    UPGD,
    EVNT,
}
