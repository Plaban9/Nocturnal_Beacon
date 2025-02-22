using CardAttribute;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipElementHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    public float delay = 0.5f;

    private Element element;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        TooltipManager.OnMouseOut();
    }


    private void ShowTooltip()
    {
        TooltipManager.OnMouseOver(new TooltipManager.TooltipDataElement(element, new Vector2(0,0)));
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(delay);

        ShowTooltip();
    }

    public void SetElement(Element element)
    {
        this.element = element;
    }


}
