using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.UI;

public  class MapQstNodeScreen : MapNonBattleNodeScreen
{
    [Header("DEBUG")]
    [SerializeField] MapQuest _mapQuest;

    [Header("Assets")]
    [SerializeField] TextMeshProUGUI _title;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _description;
    [SerializeField] TextMeshProUGUI _eventOutcome;
    [SerializeField] GameObject _cardHolder;
    [SerializeField] GameObject _cardPrefab;
    [SerializeField] GameObject _resultCard;
    Animator _animator;


    public void Start()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();

    }

    public void SetQuest(MapQuest quest)
    {
        _mapQuest = quest;
        _title.text = _mapQuest.title;
        _description.text = _mapQuest.eventDescription;
        _icon.material.mainTexture = _mapQuest.image;
    }


    public override void ActivateNonBattleNodeScreen()
    {
        _animator.Play("EventOpen");

    }


    public override void DeactivateNonBattleNodeScreen()
    {
        _animator.Play("EventClose");

        _manager.HideContinue();
    }
}
