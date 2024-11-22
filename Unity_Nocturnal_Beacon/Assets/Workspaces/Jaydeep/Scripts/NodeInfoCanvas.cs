using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NodeInfoCanvas : MonoBehaviour
{
    [SerializeField] private Vector2 cancelPos;

    private float cameraBounds;
    private CanvasGroup canvasGroup;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        float posX = 0f;

        var node = MapBuilderTD.Instance.GetCurrentlySelectedNode();
        if (node == null)
            posX = MapBuilderTD.Instance.LastRowPos;
        else
            posX = node.transform.position.x;

        cameraBounds = Camera.main.orthographicSize;
        transform.position = new Vector3(posX, transform.position.y);
    }

    public void OnCancelBtnClicked()
    {
        const float Duration = 1f;
        transform.DOMoveY(cancelPos.y, Duration).SetEase(Ease.InSine);
        canvasGroup.DOFade(0f, Duration);
    }

    public void OnNodeInfoRequestedAtNode(Vector3 nodePos)
    {
        // 7 Units below the clicked node
        var target = nodePos + (Vector3.down * 7f);
        target.x = Mathf.Clamp(target.x, -cameraBounds, cameraBounds); // Confining within camera bounds

        const float Duration = 1f;
        transform.DOMove(target, Duration).SetEase(Ease.OutSine);
        canvasGroup.DOFade(1f, Duration);
        Debug.Log(nodePos + " is Node pos and target is " + target);
    }
}
