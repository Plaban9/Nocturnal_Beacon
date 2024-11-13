using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public abstract class SelectionList<T, T2> : MonoBehaviour where T : SelectionItem<T2> where T2 : class
{
    [Header("Scrollview")]
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected Transform content;

    protected ReactiveProperty<T> selecting = new ReactiveProperty<T>();
    protected List<T> selectables = new List<T>();

    public ReactiveProperty<T> Selecting => selecting;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        selecting.Subscribe(x =>
        {
            if (x == null)
            {
                ShowSelecting(null);
                return;
            }

            ShowSelecting(x);

        }).AddTo(this);
    }

    public virtual void Reset()
    {
        selecting.Value = null;
    }


    public virtual void SetSelecting(T2 data)
    {
        if (data == null)
        {
            selecting.Value = null;
            return;
        }

        if (selecting.Value != null && selecting.Value.data == data)
        {
            // Unselect
            selecting.Value = null;
            return;
        }

        selecting.Value = selectables.First(x => x.data == data);
    }

    public virtual void ShowSelecting(T selecting)
    {
        foreach (var c in selectables)
        {
            c.SetSelecting(c == selecting);
        }
    }


    public virtual void Setup(List<T2> dataList)
    {
        Reset();

        foreach (var e in dataList)
        {
            var selectionItem = Instantiate(prefab, content).GetComponent<T>();
            selectionItem.Setup(e);
            selectionItem.onClick.Subscribe(x => {
                if (x == null || selectionItem.IsLocked()) return;
                SetSelecting(x);
            }).AddTo(selectionItem);
            selectables.Add(selectionItem);
        }
    }

    public virtual void SetLock(T2 data, bool set)
    {
        if(data != null)
        {
            selectables.First(x => x == data).SetLocked(set);
        }
    }
}
