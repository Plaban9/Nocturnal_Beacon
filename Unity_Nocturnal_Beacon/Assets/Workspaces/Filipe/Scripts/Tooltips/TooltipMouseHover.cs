using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipMouseHover : MonoBehaviour
{
    public GameObject tooltip;

    public static Action<AbilityInformation, Vector2> OnMouseOver;
    public static Action OnMouseOut;

    private Image img;
    private TextMeshProUGUI title;
    private TextMeshProUGUI desc;

    private void OnEnable()
    {
        OnMouseOver += ShowTooltip;
        OnMouseOut += HideTooltip;
    }

    private void OnDisable()
    {
        OnMouseOver -= ShowTooltip;
        OnMouseOut -= HideTooltip;
    }

    private void Start()
    {
        img = tooltip.transform.Find("TopRow/AbilityIcon").GetComponent<Image>();
        title = tooltip.transform.Find("TopRow/AbilityName").GetComponent<TextMeshProUGUI>();
        desc = tooltip.transform.Find("AbilityDescription").GetComponent<TextMeshProUGUI>();
        HideTooltip();
    }

    private void ShowTooltip(AbilityInformation info, Vector2 position)
    {
        img.sprite = info.icon;
        title.text = info.title;
        desc.text = info.description;

        tooltip.gameObject.SetActive(true);
        tooltip.transform.position = new Vector2(position.x + 1, position.y);
    }

    private void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
