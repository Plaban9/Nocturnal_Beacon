using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LighthouseEffectMocker : MonoBehaviour
{
    [SerializeField] private float _effectDurationPerLoopInSecs = 10f;

    [SerializeField] private Image _panel;

    [SerializeField] private float _initialAlpha;

    [SerializeField] private AnimationCurve _alphaCurve;

    [SerializeField] private float _currentAlpha;


    [SerializeField] private Light2D _leftLight;
    [SerializeField] private Light2D _rightLight;
    [SerializeField] private Color _lightBeamColor;
    [SerializeField] private AnimationCurve _lightBeamAlphaCurve;
    [SerializeField] private float _currentLightBeamAlpha;


    private void Awake()
    {
        _panel = GetComponent<Image>();
        _panel.color = GetPanelColor(_initialAlpha);

        _lightBeamColor = _leftLight.color;
        
    }

    private void Update()
    {
        UpdateAlpha();
    }

    private void UpdateAlpha()
    {
        if (_alphaCurve != null)
        {
            var currentTimeSlice = Mathf.PingPong(Time.time, _effectDurationPerLoopInSecs) / _effectDurationPerLoopInSecs;
            _currentAlpha = _alphaCurve.Evaluate(currentTimeSlice);

            _panel.color = GetPanelColor(_currentAlpha);

            _currentLightBeamAlpha = _lightBeamAlphaCurve.Evaluate(currentTimeSlice);
            _leftLight.color = GetLightBeamColor(_currentLightBeamAlpha);
            _rightLight.color = GetLightBeamColor(_currentLightBeamAlpha);
        }
    }

    private Color GetPanelColor(float alpha)
    {
        return new Color(_panel.color.r, _panel.color.g, _panel.color.b, alpha);
    }
    
    private Color GetLightBeamColor(float alpha)
    {
        return new Color(_leftLight.color.r, _leftLight.color.g, _leftLight.color.b, alpha);
    }
}
