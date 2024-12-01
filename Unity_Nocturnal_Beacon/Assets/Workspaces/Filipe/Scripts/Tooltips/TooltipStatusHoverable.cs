using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CardAttribute;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipStatusHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float delay = 0.5f;

    private StatusEffectObject status;

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
        TooltipManager.OnMouseOver(new TooltipManager.TooltipDataStatus(status, Input.mousePosition));
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(delay);

        ShowTooltip();
    }

    public void SetStatus(StatusEffectObject data)
    {
        status = data;
    }


}
