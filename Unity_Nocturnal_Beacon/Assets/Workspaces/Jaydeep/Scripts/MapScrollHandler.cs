using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapScrollHandler : MonoBehaviour, IDragHandler
{
    [SerializeField] private float scrollSensitivityMultiplier = .5f;
    [SerializeField] private float maxScrollLength;

    private void Start()
    {
        var pos = transform.position;
        pos.y -= 50;
        transform.DOMove(pos, 10f).SetEase(Ease.InOutCubic);
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
