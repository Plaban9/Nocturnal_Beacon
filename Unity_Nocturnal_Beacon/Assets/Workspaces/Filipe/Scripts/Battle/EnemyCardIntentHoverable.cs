using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyCardIntentHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public float delay = 0.1f;

    private Card card;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        EnemyCardIntentMouseHover.OnMouseOut();
    }


    private void ShowTooltip()
    {
        EnemyCardIntentMouseHover.OnMouseOver(card, Input.mousePosition);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(delay);

        ShowTooltip();
    }

    public void SetCard(Card card)
    {
        this.card = card;
    }

}
