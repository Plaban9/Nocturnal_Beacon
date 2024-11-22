using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeInfoSingleUI : MonoBehaviour
{
    [SerializeField] private Image monsterImg;

    public void SetMonsterSprite(Sprite sprite)
    {
        monsterImg.sprite = sprite;
    }
}
