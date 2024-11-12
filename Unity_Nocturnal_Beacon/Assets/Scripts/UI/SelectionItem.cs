using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class SelectionItem<T> : MonoBehaviour where T : class 
{
    [SerializeField] GameObject onSelectGO;

    public Subject<T> onClick { get; protected set; }

    public T data { get; protected set; }

    protected virtual void Awake()
    {
        onClick = new Subject<T>();
    }

    public virtual void OnClick()
    {
        onClick.OnNext(data);
    }

    public virtual void SetSelecting(bool set)
    {
        onSelectGO.SetActive(set);
    }

    public virtual void Setup(T data)
    {
        this.data = data;
        onClick = new Subject<T>();
    }
}
