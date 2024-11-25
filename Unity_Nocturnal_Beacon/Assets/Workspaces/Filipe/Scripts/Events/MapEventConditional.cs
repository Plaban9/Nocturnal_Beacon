using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MapEventConditional 
{
    [SerializeField] private string _requirementString = "This event requirements are unknown";
    [SerializeField] private string _successString = "For no reason in particular, you succeeded";
    [SerializeField] private string _failString = "For no reason in particular, you failed";
    [SerializeReference,SubclassSelector] private MapEventCardFilter _filter;

    public int GetResult(Card card)
    {
        return _filter.GetOutcomeSuccess(card);
    }

    public string GetReqString()
    {
        return _requirementString;
    }

    public string GetResString(Card card)
    {
        return GetResult(card) == 1? _successString : _failString;
    }
}
