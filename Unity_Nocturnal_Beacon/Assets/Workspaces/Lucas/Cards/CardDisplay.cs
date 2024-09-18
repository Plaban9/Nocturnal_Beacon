using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] protected Card card;

    [SerializeField] TMP_Text manaText;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text typeText;
    [SerializeField] TMP_Text descText;

    [SerializeField] Image mainImg;

    protected virtual void Start()
    {
        if(card != null)
        {
            Setup(card);
        }
    }

    public virtual void Setup(Card card)
    {
        this.card = card;

        manaText.text = card.manaCost.ToString();
        titleText.text = card.name.ToString();
        typeText.text = card.cardType.ToString();
        descText.text = card.GetEffectDescStr();

        mainImg.sprite = card.sprite;
    }

    public Card GetCard() => card;
}
