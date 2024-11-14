using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

public class ConfirmDialog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainText;

    Subject<bool> onConfirm = new Subject<bool>();

    bool destroySelf = false;

    public Subject<bool> Show(string text, bool destroyAfterConfirm = true)
    {
        mainText.text = text;
        destroySelf = destroyAfterConfirm;

        return onConfirm;
    }

    public void OnClickConfirm()
    {
        onConfirm.OnNext(true);

        if (destroySelf)
            Destroy(gameObject);
    }

    public void OnClickCancel()
    {
        Destroy(gameObject);
    }
}
