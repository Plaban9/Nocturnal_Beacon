using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipElement : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] TextMeshProUGUI _elementName;
    [SerializeField] UnityEngine.UI.Image _elementIcon;

    [SerializeField] GameObject elementPrefab;
    [SerializeField] GameObject prefabHolder;

    public void SetupElement(Element element)
    {

        _elementIcon.sprite = ElementalTable.GetElementalIcon(element);
        _elementName.text = element.ToString(); 
        foreach (Transform t in prefabHolder.transform)
        {
            Destroy(t.gameObject);
        }

        foreach (Element targetElement in Enum.GetValues(typeof(Element)))
        {
            GameObject newElementRow = Instantiate(elementPrefab, prefabHolder.transform);
            newElementRow.GetComponent<ElementListItem>().SetEfficiency(targetElement, element);
        }
    }



}
