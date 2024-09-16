using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] Card card;

    [SerializeField] TMP_Text manaText;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text typeText;
    [SerializeField] TMP_Text descText;

    [SerializeField] Image mainImg;

    private void Start()
    {
        if(card != null)
        {
            manaText.text = card.manaCost.ToString();
            titleText.text = card.name.ToString();
            typeText.text = card.cardType.ToString();
            descText.text = card.GetEffectDescStr();

            mainImg.sprite = card.sprite;
        }
    }
}
