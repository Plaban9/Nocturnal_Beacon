using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CardDetailPage : CommonPage
{
    [SerializeField] CardDisplay cardDisplay;
    [SerializeField] GameObject debugToCustomizeCard;
    [SerializeField] GameObject confirmBtn;

    Card cardSO;
    Subject<Card> onConfirm = new();

    // Start is called before the first frame update
    void Start()
    {
        debugToCustomizeCard.SetActive(Debug.isDebugBuild || Application.isEditor);
    }

    public void Setup(Card c)
    {
        cardSO = c;
        cardDisplay.Setup(c);
    }

    public void OnClickGoToCustomizeCard()
    {
        var ccp = UIManager.Instance.ShowPage(GamePage.CustomizeCardPage).GetComponent<CustomizeCardPage>();
        ccp.Setup(cardSO).Subscribe(x =>
        {
            Setup(x);
        }).AddTo(ccp);
    }

    public void OnClickBackground()
    {
        Close();
    }

    public void OnClickConfirm()
    {
        onConfirm.OnNext(cardSO);
    }

    public Subject<Card> OnConFirm()
    {
        confirmBtn.SetActive(true);
        return onConfirm;
    }
}
