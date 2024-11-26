using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.UI;

public  class MapEvntNodeScreen : MapNonBattleNodeScreen
{
    [Header("DEBUG")]
    [SerializeField] MapEvent _mapEvent;

    [Header("Assets")]
    [SerializeField] TextMeshProUGUI _title;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _description;


    public void Start()
    {
        _title.text = _mapEvent.title;
        _description.text = _mapEvent.eventDescription;
        _icon.material.mainTexture = _mapEvent.image;
    }


    public override void ActivateNonBattleNodeScreen()
    {

    }


    public override void DeactivateNonBattleNodeScreen()
    {
        _manager.HideContinue();
    }
}
