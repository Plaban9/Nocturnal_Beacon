using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CommonButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Vector3 oriScale = Vector3.one;
    [SerializeField] float onPointScale = 1.2f;
    [SerializeField] float pointerEnterScaleTime = 0f;
    [SerializeField] float pointerExitScaleTime = 0.2f;

    RectTransform rectTransform;
    Button button;
    Tween tween;
    public void OnPointerClick(PointerEventData eventData)
    {
        button.onClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tween.Kill();
        tween = rectTransform.DOScale(oriScale * onPointScale, pointerEnterScaleTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tween.Kill();
        tween = rectTransform.DOScale(oriScale, pointerExitScaleTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();    
    }
}
