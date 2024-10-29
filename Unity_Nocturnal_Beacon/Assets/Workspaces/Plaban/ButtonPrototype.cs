using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ButtonPrototype : MonoBehaviour
{
    [SerializeField] private Color _idleColor;
    [SerializeField] private Color _highlightedColor;

    [SerializeField] private Text _buttonText;

    private void Awake()
    {
        if (_buttonText != null)
        {
            _idleColor = _buttonText.color;
        }
    }


    public void OnHoverEnter()
    {
        Debug.Log($"{gameObject.name}:Mouse is over GameObject.");

        if (_buttonText != null)
        {
            _buttonText.color = _highlightedColor;
        }
    }

    public void OnHoverExit()
    {
        Debug.Log($"{gameObject.name}: Mouse is no longer on GameObject.");

        if (_buttonText != null)
        {
            _buttonText.color = _idleColor;
        }
    }
}
