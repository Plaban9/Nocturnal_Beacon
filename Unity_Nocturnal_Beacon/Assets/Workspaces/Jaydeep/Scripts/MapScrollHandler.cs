using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapScrollHandler : MonoBehaviour, IDragHandler
{
    [SerializeField] private float scrollSensitivityMultiplier = .5f;
    [SerializeField] private float maxScrollLength;
    [SerializeField, Range(0f, 10f)] private float initialScrollTime = 10f;

    private IEnumerator Start()
    {
        // This will hold the function until the grid is created and current node is set;
        yield return new WaitForSeconds(0.1f);

        float posY = 0f;

        var node = MapBuilderTD.Instance.GetCurrentlySelectedNode();
        if (node == null)
            posY = MapBuilderTD.Instance.LastRowPos;
        else
            posY = node.transform.position.y;

        var currentlySelectedNode = MapBuilderTD.Instance.GetCurrentlySelectedNode();
        var myPos = transform.position;
        var scrollPos = new Vector3(myPos.x, posY, myPos.z);

        //Occurs first time, then it doesnt anymore -FMM
        if(NoctBeaconRunData.Instance.IsNewGameStarted())
        {
            transform.DOMove(scrollPos, initialScrollTime).SetEase(Ease.InOutCubic).OnComplete(() => 
            {
                if (!UITopBarManager.Instance.IsDown())
                {
                    UITopBarManager.Instance.PullDown();
                }
            });
        }
        else
        {
            transform.position = scrollPos;
            if (!UITopBarManager.Instance.IsDown())
            {
                UITopBarManager.Instance.PullDown();
            }
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector3 delta = 0.05f * eventData.delta;
        delta.x = 0;
        transform.position -= scrollSensitivityMultiplier * delta;
        var pos = transform.position;
        var min = Mathf.Min(0, maxScrollLength);
        var max = Mathf.Max(0, maxScrollLength);
        transform.position = new(pos.x, Mathf.Clamp(pos.y, min, max), pos.z);
    }
}
