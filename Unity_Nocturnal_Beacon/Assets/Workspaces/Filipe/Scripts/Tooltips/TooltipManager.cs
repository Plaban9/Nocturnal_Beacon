using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TooltipManager : MonoBehaviour
{
    
    public static TooltipManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [SerializeField] TooltipEnemy _tooltipEnemy;
    [SerializeField] TooltipStatusEffect _tooltipStatus;
    [SerializeField] TooltipElement _tooltipElement;
    [SerializeField] TooltipCard _tooltipCard;


    public static Action<TooltipData> OnMouseOver;
    public static Action OnMouseOut;

    private void Start()
    {
        DisableAllTooltips();
    }
    private void DisableAllTooltips()
    {
        _tooltipElement.gameObject.SetActive(false);
        _tooltipCard.gameObject.SetActive(false);
        _tooltipEnemy.gameObject.SetActive(false);
        _tooltipStatus.gameObject.SetActive(false);
    }

    private void EnableTooltip(GameObject gameObject, Vector2 position)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector2(position.x - 1, position.y - 1);
    }

    public void ShowTooltipMonster(Vector2 position, MonsterData monster)
    {
        DisableAllTooltips();
        EnableTooltip(_tooltipEnemy.gameObject, position);
        _tooltipEnemy.SetMonster(monster);
    }

    public void ShowTooltipStatus(Vector2 position, StatusEffectObject status)
    {
        DisableAllTooltips();
        EnableTooltip(_tooltipStatus.gameObject, position);
        _tooltipStatus.SetStatus(status);
    }

    public void ShowTooltipCard(Vector2 position, Card card)
    {
        DisableAllTooltips();
        EnableTooltip(_tooltipCard.gameObject, position);
        _tooltipCard.SetCard(card);

    }

    public void ShowTooltipElement(Vector2 position, Element element)
    {
        DisableAllTooltips();
        _tooltipElement.gameObject.SetActive(true);
        _tooltipElement.SetupElement(element);
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

    private void ShowTooltip( TooltipData data)
    {
        
        if (data is TooltipDataElement)
        {
            ShowTooltipElement( data.position, (data as TooltipDataElement).element);
        }
        else if (data is TooltipDataCard)
        {
            ShowTooltipCard(data.position, (data as TooltipDataCard).card);
        }
        else if (data is TooltipDataEnemy)
        {
            ShowTooltipMonster(data.position, (data as TooltipDataEnemy).monsterData);
        }
        else if (data is TooltipDataStatus)
        {
            ShowTooltipStatus(data.position, (data as TooltipDataStatus).status);
        }
    }

    private void HideTooltip()
    {
        DisableAllTooltips();
    }

    public abstract class TooltipData
    {
        public Vector2 position;
    }

    public class TooltipDataCard : TooltipData
    {
       
        public Card card;
        public TooltipDataCard(Card card, Vector2 position)
        {
            this.card = card;
            this.position = position;
        }
    }

    public class TooltipDataEnemy : TooltipData
    {
        public MonsterData monsterData;
        public TooltipDataEnemy(MonsterData monsterData, Vector2 position)
        {
            this.monsterData = monsterData;
            this.position = position;
        }
    }

    public class TooltipDataElement : TooltipData
    {
        public Element element;
        public TooltipDataElement(Element element, Vector2 position)
        {
            this.element = element;
            this.position = position;
        }
    }

    public class TooltipDataStatus : TooltipData
    {
        public StatusEffectObject status;
        public TooltipDataStatus(StatusEffectObject status, Vector2 position)
        {
            this.status = status;
            this.position = position;
        }
    }


}
