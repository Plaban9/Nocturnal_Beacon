using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyCardIntentHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public float delay = 0.1f;

    private Card card;

    private void Start()
    {
        Animator animator = _turnOrder.GetComponent<Animator>();

        animator.speed = UnityEngine.Random.Range(0.1f, 1f);
        animator.playbackTime = UnityEngine.Random.Range(0f, 1f);


    }

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


    [SerializeField] TextMeshProUGUI _turnOrder;

    public void SetTurnOrder(int i)
    {
        _turnOrder.SetText($"{i}");
    }

}
