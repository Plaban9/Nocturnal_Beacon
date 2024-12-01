using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipEnemy : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] public TextMeshProUGUI _name;
    [SerializeField] public TextMeshProUGUI _hp;
    [SerializeField] public Image _monsterSprite;
    [SerializeField] public Image _elementIcon;

    public void SetMonster(MonsterData data)
    {
        _monsterSprite.sprite = data.sprite;
        _elementIcon.sprite = ElementalTable.GetElementalIcon(data.unitElement);
        _hp.text = data.maxHp.ToString();
        _name.text = data.name;
    }
}
