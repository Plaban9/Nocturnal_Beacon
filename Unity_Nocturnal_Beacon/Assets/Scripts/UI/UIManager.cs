using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public static UIManager Instance => instance;

    [SerializeField] GameObject noticeBarPrefab;
    [SerializeField] GameObject confirmDialogPrefab;
    NoticeBar noticeBar;

    Canvas canvas;
    Transform parent;

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        var go = new GameObject("UIManager");
        go.transform.SetParent(canvas.transform);
        go.transform.localPosition = Vector3.zero;
        parent = go.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowNoticeBar(string text, float duration = 2f)
    {
        if(noticeBar == null)
        {
            noticeBar = Instantiate(noticeBarPrefab, parent).GetComponent<NoticeBar>();
            noticeBar.Show(text, duration);
        }
        else if(noticeBar.IsShowing())
        {
            var nb = Instantiate(noticeBarPrefab, parent).GetComponent<NoticeBar>();
            nb.ShowOnce(text, duration);
        }
        else
        {
            noticeBar.Show(text, duration);
        }
    }

    public Subject<bool> ShowConfirmDialog(string text)
    {
        var cd = Instantiate(confirmDialogPrefab, parent).GetComponent<ConfirmDialog>();

        return cd.Show(text);
    }
}
