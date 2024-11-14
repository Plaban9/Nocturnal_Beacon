
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCardIntentMouseHover : MonoBehaviour
{




    [SerializeField] public CardDisplay _cd ;


    public GameObject tooltip;

    public static Action<Card, Vector2> OnMouseOver;
    public static Action OnMouseOut;


    private void Start()
    {
        HideTooltip();
    }



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

    private void ShowTooltip(Card card, Vector2 position)
    {
        _cd.Setup(card);
        tooltip.gameObject.SetActive(true);
        tooltip.transform.position = new Vector2(position.x - 1, position.y - 1);
    }

    private void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
