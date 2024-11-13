using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class SelectionItem<T> : MonoBehaviour where T : class 
{
    [SerializeField] GameObject onSelectGO;
    [SerializeField] GameObject onLockedGO;

    public Subject<T> onClick { get; protected set; }

    public T data { get; protected set; }

    public bool IsLocked() => onLockedGO != null && onLockedGO.activeSelf;

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

    public virtual void SetLocked(bool set)
    {
        if (onLockedGO != null)
            onLockedGO.SetActive(set);

        SetSelecting(false);
    }
    public virtual void Setup(T data)
    {
        this.data = data;
        onClick = new Subject<T>();
    }
}
