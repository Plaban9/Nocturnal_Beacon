using Minimalist.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestCardPickerHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Animator _animator;

    [SerializeField] TextMeshProUGUI _efficiencyText;
    [SerializeField] Image _resultImage;

    private Action<int,Card> onClick = (int i, Card card) => {};

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetOnClick(Action<int,Card> action)
    {
        onClick = action;
    }

    public void Close()
    {
        _animator.SetTrigger("close");
    } 

    public void OnPointerClick(PointerEventData eventData)
    {
        _animator.SetTrigger("select");
        onClick(index,card);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator.SetBool("mouseOver",true);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.SetBool("mouseOver", false);

    }

    private int index = 0;
    private Card card = null;
    public void SetData(int i, Card assignedCard, MapQuest quest)
    {
        card = assignedCard;
        index = i;
        int result = quest.GetResultRewards(card);
        switch (result)
        {
            case 0:
                _efficiencyText.text = "Neutral";
                _resultImage.color = new Color(0.5f, 0.5f, 0.5f);
                break;
            case 1:
                _efficiencyText.text = "Decent";
                _resultImage.color = new Color(0.4f, 0.6f, 0.4f);
                break;
            case 2:
                _efficiencyText.text = "Good";
                _resultImage.color = new Color(0.4f, 0.7f, 0.4f);
                break;
            case 3:
                _efficiencyText.text = "Great";
                _resultImage.color = new Color(0.8f, 0.9f, 0.2f);
                break;
            case 4:
                _efficiencyText.text = "Perfect";
                _resultImage.color = new Color(0.9f, 0.7f, 0.2f);
                break;
        }
    }

}
