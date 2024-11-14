using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CommonPage : MonoBehaviour
{
    public Subject<bool> OnClose = new Subject<bool>();

    public virtual void Close()
    {
        OnClose.OnNext(true);
        Destroy(gameObject);
    }
}
