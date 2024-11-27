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
    [SerializeField] TextMeshProUGUI _eventOutcome;
    [SerializeField] GameObject _cardHolder;
    [SerializeField] GameObject _cardPrefab;
    [SerializeField] GameObject _resultCard;
    Animator _animator;


    public void Start()
    {
        _title.text = _mapEvent.title;
        _description.text = _mapEvent.eventDescription;
        _icon.material.mainTexture = _mapEvent.image;
        _animator = transform.GetChild(0).GetComponent<Animator>();
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
