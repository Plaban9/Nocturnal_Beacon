using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class CardInHand : CardDisplay, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    CanvasGroup canvasGroup;
    RectTransform rt;
    Vector2 oriPos;

    ReactiveProperty<bool> onDrag = new ReactiveProperty<bool>(false);
    Subject<CardInHand> onDeploy = new Subject<CardInHand>();

    bool highlight1Enemy = false;
    bool highlightAllEnemy = false;
    bool highlightSelf = false;

    public BattleUnit hoveredEnemy = null;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();


    }

    // Start is called before the first frame update
    protected override void Start()
    {
        //base.Start();
        highlight1Enemy = card.TargetSingleEnemy();
        highlightAllEnemy = card.TargetAllEnemy();
        highlightSelf = card.TargetSelf();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public RectTransform rectTransform => rt;

    public Vector2 GetOriPos() => oriPos;

    public ReactiveProperty<bool> SubscribeOnDrag() => onDrag;
    public Subject<CardInHand> SubscribeOnDeploy() => onDeploy;

    public void SetOriPos(Vector2 pos)
    {
        oriPos = pos;
        rt.anchoredPosition = pos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0.2f, 0.3f);
        oriPos = rt.anchoredPosition;
        if (!highlight1Enemy && (highlightAllEnemy || highlightSelf))
        {
            BattleManager.Instance.SetNoTargetReticule(true);

        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("BeingDrag");
        onDrag.Value = true;

        rt.anchoredPosition += eventData.delta;

        if (IsPointingDeployArea())
        {
            /*
             * Glowing effect?
             */
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hitInfo.rigidbody != null)
        {
            Debug.Log(hitInfo.rigidbody.gameObject.name);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onDrag.Value = false;
        canvasGroup.DOFade(1f, 0.3f);


        if (IsPointingDeployArea())
        {
            onDeploy.OnNext(this);
            BattleManager.Instance.HideOutlinePlayer();
            BattleManager.Instance.HideOutlineEnemies();
            hoveredEnemy?.HideOutline();
            hoveredEnemy = null;
        }
        else
        {
            canvasGroup.blocksRaycasts = true;
            rt.anchoredPosition = oriPos;
        }

        if (!highlight1Enemy && (highlightAllEnemy || highlightSelf))
        {
            BattleManager.Instance.SetNoTargetReticule(false);
        }

    }

    bool IsPointingDeployArea()
    {
        var ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);

        if (highlight1Enemy)
        {
            Debug.Log("single!");
            foreach (var r in results)
            {
                if (r.gameObject.layer == LayerMask.NameToLayer("DeployCardTarget"))
                {
                    var newTarget = r.gameObject.transform.parent.parent.gameObject.GetComponent<BattleUnit>();
                    if (highlightSelf)
                    {
                        BattleManager.Instance.OutlinePlayer();
                    }
                    if (newTarget != hoveredEnemy)
                    {
                        hoveredEnemy = newTarget;
                        hoveredEnemy.Outline();
                    }
                    return true;
                }
            }
        }
        else if (highlightAllEnemy)
        {
            Debug.Log("multi!");

            foreach (var r in results)
            {
                if (r.gameObject.layer == LayerMask.NameToLayer("DeployCardNoTarget"))
                {
                    if (highlightSelf)
                    {
                        BattleManager.Instance.OutlinePlayer();
                    }
                    BattleManager.Instance.OutlineEnemies();
                    return true;
                }
            }
        }
        else
        {
            Debug.Log("SKILL!");
            foreach (var r in results)
            {
                if (r.gameObject.layer == LayerMask.NameToLayer("DeployCardNoTarget"))
                {
                    if (highlightSelf)
                    {
                        BattleManager.Instance.OutlinePlayer();
                    }
                    return true;
                }
            }
        }

        

        
        if(hoveredEnemy != null)
        {
            BattleManager.Instance.HideOutlinePlayer();
            BattleManager.Instance.HideOutlineEnemies();
            hoveredEnemy.HideOutline();
            hoveredEnemy = null;
        }
        return false;
    }

    public void ResetToOriPos()
    {
        canvasGroup.blocksRaycasts = true;
        rt.anchoredPosition = oriPos;
    }

    public IEnumerator PerformDiscard()
    {
        var fadeTime = 0.2f;

        rt.DOScale(0.1f, 0.1f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.1f);
        GetComponent<CanvasGroup>().DOFade(0, fadeTime);
        rt.DOAnchorPos(new Vector2(850, 0), fadeTime);
        yield return new WaitForSeconds(fadeTime);
    }

    public IEnumerator PerformDrawFromPile()
    {
        var aniTime = 0.2f;

        rt.DOScale(0, 0);
        rt.anchoredPosition = new Vector2(-850, 0);
        rt.DOScale(0.5f, aniTime).SetEase(Ease.InBack);
        rt.DOAnchorPos(new Vector2(0, -50), aniTime);
        yield return new WaitForSeconds(aniTime);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
