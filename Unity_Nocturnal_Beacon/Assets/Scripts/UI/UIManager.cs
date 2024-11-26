using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum GamePage
{
    DeckPage,
    CardDetailPage,
    CustomizeCardPage,
    ShopPage,
    ShopItemPage
}

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public static UIManager Instance => instance;

    [Header("Pages")]
    [SerializeField] GameObject deckPagePrefab;
    [SerializeField] GameObject cardDetailPagePrefab;
    [SerializeField] GameObject customizeCardPagePrefab;
    [SerializeField] GameObject shopPagePrefab;
    [SerializeField] GameObject shopItemPagePrefab;

    Dictionary<GamePage, GameObject> pageDict = new Dictionary<GamePage, GameObject>();

    [Header("Common components")]
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
        canvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        DontDestroyOnLoad(canvas);

        var go = new GameObject("UIManager");
        go.transform.SetParent(canvas.transform);
        go.transform.localPosition = Vector3.zero;

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;

        parent = go.transform;
    }

    public GameObject ShowPage(GamePage page)
    {
        GameObject pageGO = null;

        if(pageDict.ContainsKey(page) && pageDict[page] != null)
        {
            pageDict[page].transform.SetAsLastSibling();
            pageDict[page].SetActive(true);
            return pageDict[page];
        }
        else
        {
            switch (page)
            {
                case GamePage.DeckPage:
                    {
                        pageGO = Instantiate(deckPagePrefab, parent);
                        break;
                    }
                case GamePage.CardDetailPage:
                    {
                        pageGO = Instantiate(cardDetailPagePrefab, parent);
                        break;
                    }
                case GamePage.CustomizeCardPage:
                    {
                        pageGO = Instantiate(customizeCardPagePrefab, parent);
                        break;
                    }
                case GamePage.ShopPage:
                    {
                        pageGO = Instantiate(shopPagePrefab, parent);
                        break;
                    }
                case GamePage.ShopItemPage:
                    {
                        pageGO = Instantiate(shopItemPagePrefab, parent);
                        break;
                    }
            }

            pageDict[page] = pageGO;
        }

        return pageGO;
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
