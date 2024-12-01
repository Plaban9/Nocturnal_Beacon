using CardAttribute;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _efficiencyText;
    [SerializeField] private Image _elementIcon;
    [SerializeField] private Image _affinityColor;

    public void  SetEfficiency(Element targetElement, Element thisElement)
    {
        _efficiencyText.text = ElementalTable.GetAffinityText(ElementalTable.GetElementalAffinity(thisElement, targetElement));
        _affinityColor.color = ElementalTable.GetAffinityColor(ElementalTable.GetElementalAffinity(thisElement, targetElement));
        _elementIcon.sprite = ElementalTable.GetElementalIcon(targetElement); 
    }
}
