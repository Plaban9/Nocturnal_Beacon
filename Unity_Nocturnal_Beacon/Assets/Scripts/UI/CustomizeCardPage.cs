using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CustomizeCardPage : CommonPage
{
    [SerializeField] CustomizeCardController cardController;

    Subject<Card> onCustomized = new Subject<Card>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Subject<Card> Setup(Card c, int point)
    {
        cardController.Setup(c, point, (card) =>
        {
            NoctBeaconRunData.Instance.GetPlayerInformation().GetCurrentDeck().ReplaceCard(c.uId, card);
            onCustomized.OnNext(card);

            //close page
            Close();
        });

        return onCustomized;
    }

}
