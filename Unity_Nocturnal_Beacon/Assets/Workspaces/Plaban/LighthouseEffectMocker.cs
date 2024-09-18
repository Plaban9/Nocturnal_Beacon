using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LighthouseEffectMocker : MonoBehaviour
{
    [SerializeField] private float _effectDurationPerLoopInSecs = 10f;

    [SerializeField] private Image _panel;

    [SerializeField] private float _initialAlpha;

    [SerializeField] private AnimationCurve _alphaCurve;

    [SerializeField] private float _currentAlpha;


    private void Awake()
    {
        _panel = GetComponent<Image>();
        _panel.color = GetColor(_initialAlpha);
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

            _panel.color = GetColor(_currentAlpha);
        }
    }

    private Color GetColor(float alpha)
    {
        return new Color(_panel.color.r, _panel.color.g, _panel.color.b, alpha);
    }
}
