using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using System.Linq;

public class HandZone : MonoBehaviour
{
    [SerializeField] GameObject HandCardPrefab;

    List<CardInHand> cardInHands = new List<CardInHand>();

    PointerEventData pointerEventData;
    CardInHand pointingCard;

    bool isDraggingCard = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDraggingCard)
        {
            DetectCardPointedByMouse();
            OutstandPointingCard();
        }
    }

    #region For debug testing
    [ContextMenu("Test Add Card")]
    public void TestAddCard()
    {
        AddCard(CardLibrary.Instance.GetCardById(10001));
    }

    [ContextMenu("Test Add 10 Card")]
    public void TestAdd10Card()
    {
        for(int i=0; i<10; i++)
            AddCard(CardLibrary.Instance.GetCardById(10001));
    }
    #endregion

    public void AddCard(Card card)
    {
        StartCoroutine(PerformAddCard(card));
    }

    IEnumerator PerformAddCard(Card card)
    {
        var handCard = Instantiate(HandCardPrefab, transform).GetComponent<CardInHand>();
        handCard.Setup(card);
        handCard.SubscribeOnDrag().Subscribe(x => isDraggingCard = x).AddTo(handCard.gameObject);
        handCard.SubscribeOnDeploy().Subscribe(x =>
        {
            if (!DeployCard(x))
                handCard.ResetToOriPos();
        }).AddTo(handCard);
        
        cardInHands.Add(handCard);

        //yield return StartCoroutine(handCard.PerformDrawFromPile());  //Temp disabled

        Resize();

        yield return null;
    }

    #region For display
    void Resize()
    {
        int size = cardInHands.Count;
        float s = size % 2 == 0 ? ((size-1)/2 + 0.5f) : ((size - 1) / 2);
        if(size >= 1)
        {
            int interval = Mathf.Clamp(1200 / size, 0, 300);
            
            for(int i=0; i<size; i++)
            {
                float index = i - s;
                cardInHands[i].SetOriPos(new Vector2(interval * index, -50));
                cardInHands[i].transform.SetSiblingIndex(i);
            }

        }
    }

    void DetectCardPointedByMouse()
    {
        pointerEventData = new PointerEventData(EventSystem.current);
        if (pointerEventData.dragging) return;


        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (var r in results)
        {
            if(r.gameObject.TryGetComponent(out CardInHand c))
            {
                if(pointingCard != c || pointingCard == null)
                {
                    Resize();
                    pointingCard = c;
                    Debug.Log("Pointing");
                }

                return;
            }

        }

        if(pointingCard != null)
        {
            Resize();
            pointingCard.transform.SetSiblingIndex(cardInHands.IndexOf(pointingCard));
            pointingCard = null;
            Debug.Log("Not Pointing");
        }    
    }

    void OutstandPointingCard()
    {
        if(pointingCard != null)
        {
            pointingCard.rectTransform.anchoredPosition = new Vector2(pointingCard.GetOriPos().x, 50);
            pointingCard.transform.SetAsLastSibling();

            int index = cardInHands.IndexOf(pointingCard);
            int size = cardInHands.Count;

            int i = index - 1;
            int j = index + 1;
            float l = Mathf.Clamp(size*size, 0, 150f);
            float r = l;

            while(i >= 0 || j < size)
            {
                if(i >= 0)
                {
                    cardInHands[i].rectTransform.anchoredPosition = new Vector2(cardInHands[i].GetOriPos().x - l, -50);
                    l *= 0.8f;
                    i--;
                }

                if(j < size)
                {
                    cardInHands[j].rectTransform.anchoredPosition = new Vector2(cardInHands[j].GetOriPos().x + r, -50);
                    r *= 0.8f;
                    j++;
                }
            }
        }
    }
    #endregion

    bool DeployCard(CardInHand cardInHand)
    {

        if (BattleManager.Instance._cardManager.DeployCard(cardInHand.GetCard()))
        {
            RemoveCard(cardInHand);
            return true;
        }
        else
        {
            // TODO: Show player why cant use this
            return false;
        }

    }

    public List<Card> GetCards()
    {
        return cardInHands.Select(x => x.GetCard()).ToList();
    }

    public List<CardInHand> GetCardsInHand()
    {
        return cardInHands.ToList();
    }

    public void RemoveCard(CardInHand cardInHand)
    {
        cardInHands.Remove(cardInHand);
        cardInHand.Destroy();
    }
}
