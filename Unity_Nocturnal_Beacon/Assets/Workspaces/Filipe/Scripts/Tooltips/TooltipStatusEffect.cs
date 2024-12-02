using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipStatusEffect : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] public TextMeshProUGUI _title;
    [SerializeField] public TextMeshProUGUI _description;
    [SerializeField] public Image _icon;

    public void SetStatus(StatusEffectObject status)
    {
        _icon.sprite = status.icon;
        _description.text = status.description;
        _title.text = status.name;
    }
}
