using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CardAttribute;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipEnemyHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float delay = 0.5f;

    private MonsterData monsterData;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        TooltipManager.OnMouseOut();
    }


    private void ShowTooltip()
    {
        TooltipManager.OnMouseOver(new TooltipManager.TooltipDataEnemy(monsterData, Input.mousePosition));
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(delay);

        ShowTooltip();
    }

    public void SetMonsterData(MonsterData data)
    {
        monsterData = data;
    }


}
