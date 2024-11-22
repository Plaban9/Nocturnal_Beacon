using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NodeInfoCanvas : MonoBehaviour
{
    [SerializeField] private Vector2 cancelPos;
    [SerializeField] private Transform nodeInfoParent; 
    [SerializeField] private NodeInfoSingleUI nodeInfoUITemplate;

    private float cameraBounds;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        nodeInfoUITemplate.gameObject.SetActive(false);
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        float posX = 0f;

        var node = MapBuilderTD.Instance.GetCurrentlyLastProceededNode();
        if (node == null)
            posX = MapBuilderTD.Instance.LastRowPos;
        else
            posX = node.transform.position.x;

        cameraBounds = Camera.main.orthographicSize;
        transform.position = new Vector3(posX, transform.position.y);
    }

    public void OnProceedBtnClicked() => MapBuilderTD.Instance.Proceed();

    public void OnCancelBtnClicked()
    {
        const float Duration = 1f;
        transform.DOMoveY(cancelPos.y, Duration).SetEase(Ease.InSine);
        canvasGroup.DOFade(0f, Duration);
        MapBuilderTD.Instance.DeselectNode();
    }

    public void OnNodeInfoRequestedAtNode(MapNode node)
    {
        SetInfoPanelData(node);

        SetInfoPanelPos(node);
    }

    private void SetInfoPanelData(MapNode node)
    {
        var color = MapBuilderTD.Instance.CurrentGradient.colorKeys[0].color;
        color.a = 0.1f;
        nodeInfoParent.GetComponent<Image>().color = color;

        foreach (Transform child in nodeInfoParent)
        {
            if (child == nodeInfoUITemplate.transform)
                continue;

            Destroy(child.gameObject);
        }

        if (node.GetNodeType() != NodeType.Combat)
            return;

        foreach (var enemy in node.EnemyEncounter.enemies)
        {
            var infoData = Instantiate(nodeInfoUITemplate, nodeInfoParent);
            infoData.SetMonsterSprite(enemy.sprite);
            infoData.gameObject.SetActive(true);
        }
    }

    private void SetInfoPanelPos(MapNode node)
    {
        var nodePos = node.transform.position;
        // 7 Units below the clicked node
        var target = nodePos + (Vector3.down * 7f);
        target.x = Mathf.Clamp(target.x, -cameraBounds, cameraBounds); // Confining within camera bounds

        const float Duration = 1f;
        transform.DOMove(target, Duration).SetEase(Ease.OutSine);
        canvasGroup.DOFade(1f, Duration);
        Debug.Log(nodePos + " is Node pos and target is " + target);
    }
}
