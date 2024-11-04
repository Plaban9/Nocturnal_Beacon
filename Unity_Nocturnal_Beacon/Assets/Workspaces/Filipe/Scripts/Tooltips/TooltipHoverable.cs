using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    

    public AbilityInformation displayedInfo;
    public float delay = 0.5f;

    private Image icon;
    private TextMeshProUGUI displayedName;
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        TooltipMouseHover.OnMouseOut();
    }


    private void ShowTooltip()
    {
        TooltipMouseHover.OnMouseOver(displayedInfo, Input.mousePosition);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(delay);

        ShowTooltip();
    }


    private void Start()
    {
        icon = transform.Find("AbilityIcon").GetComponent<Image>();
        displayedName = transform.Find("AbilityName").GetComponent<TextMeshProUGUI>();

        icon.sprite = displayedInfo.icon;
        displayedName.text = displayedInfo.title;
    }

}
