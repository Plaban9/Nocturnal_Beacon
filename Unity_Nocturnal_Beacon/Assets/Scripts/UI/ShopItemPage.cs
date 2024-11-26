using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using Unity.VisualScripting;
using System.Linq;

public class ShopItemPage : CommonPage
{
    [SerializeField] GameObject cardDisplayPrefab;
    [SerializeField] Transform content;
    [SerializeField] Transform itemsParent;
    [SerializeField] RandomDialog purchasedDialog;
    [SerializeField] RandomDialog noMoneyDialog;

    [SerializeField] Transform hand;
    [SerializeField] Vector3 handOffset = Vector3.zero;
    [SerializeField] float handHideTime = 1f;

    List<CardDisplay> cardDisplayList = new List<CardDisplay>();
    Deck shopItems;

    CanvasGroup canvasGroup;
    bool isClosing;

    float handTimer = 0f;
    DG.Tweening.Sequence handHideSeq;

    PlayerUnitData pud;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        var pos = content.localPosition;
        pos.y = Screen.height + 100;
        content.localPosition = pos;
    }

    private void Update()
    {
        if (!IsHandHided() && handTimer > 0)
        {
            handTimer -= Time.deltaTime;

            if(handTimer <= 0)
            {
                HideHand();
            }
        }
    }
    public void Setup(Deck deck)
    {
        if(deck == null)
        {
            Debug.LogError("Shop items deck is empty.");
            return;
        }

        shopItems = deck;
        var cards = deck.Export();

        for (int i = 0; i < cards.Count; i++)
        {
            if (i >= cardDisplayList.Count)
            {
                var cd = Instantiate(cardDisplayPrefab, itemsParent).GetComponent<CardDisplay>();
                cd.SetupForClickable(cards[i], OnCardClick, null);
                cd.SetScale(0.4f);
                cd.SetZoomRatio(0.5f);
                cd.EnablePriceTag(true);
                cd.OnPoint().Subscribe(x => OnCardPointed(x)).AddTo(cd);

                cardDisplayList.Add(cd);
            }
            else
            {
                cardDisplayList[i].SetupForClickable(cards[i], OnCardClick, null);
            }
        }
    }

    public void Show()
    {
        if (content.localPosition.y < Screen.height) return;

        gameObject.SetActive(true);

        content.DOLocalMoveY(-50, 0.5f);
        canvasGroup.DOFade(1, 0.5f);
    }

    public override void Close()
    {
        if (isClosing) return;

        isClosing = true;

        content.DOLocalMoveY(Screen.height + 100, 0.5f);
        canvasGroup.DOFade(0, 0.5f).onComplete += () =>
        {
            gameObject.SetActive(false);
            isClosing = false;
        };
    }

    void OnCardClick(CardDisplay cardDisplay)
    {
        bool success = NoctBeaconRunData.Instance.ModifyGold(-cardDisplay.GetCard().price);
        noMoneyDialog.Hide();
        purchasedDialog.Hide();

        var dialog = success ? purchasedDialog : noMoneyDialog;

        if (success)
        {
            var card = cardDisplay.GetCard();

            if(card.cardType == CardAttribute.CardType.Shop)
            {
                var dp = UIManager.Instance.ShowPage(GamePage.DeckPage).GetComponent<DeckPage>();
                dp.SetPointCursor(null);
                dp.Setup(null, (cd) =>
                {
                    var cdp = UIManager.Instance.ShowPage(GamePage.CardDetailPage).GetComponent<CardDetailPage>();
                    cdp.Setup(cd.GetCard());
                    cdp.OnConFirm().Subscribe(x =>
                    {
                        cdp.Close();
                        dp.Kill();
                        var ccp = UIManager.Instance.ShowPage(GamePage.CustomizeCardPage).GetComponent<CustomizeCardPage>();
                        ccp.Setup(x, card.effects.First().GetMainValue()).Subscribe(x =>
                        {
                            Show();

                            var pos = purchasedDialog.transform.localPosition;
                            pos.x = Random.Range(-100, 100);
                            purchasedDialog.transform.localPosition = pos;
                            purchasedDialog.Show(2);

                        }).AddTo(ccp);
                    }).AddTo(cdp);
                    
                });
                dp.SetPromptText("Select a card to customize.");
                dp.SetCloseBtn(false);
                dp.Show();
                Close();
            }
            else
            {
                NoctBeaconRunData.Instance.GetPlayerInformation().GetCurrentDeck().AddCard(card);
            }

            cardDisplay.FadeOut();
            shopItems.RemoveCard(cardDisplay.GetCard());
            Refresh();
        }

        var pos = dialog.transform.localPosition;
        pos.x = Random.Range(-100, 100);
        dialog.transform.localPosition = pos;
        dialog.Show(2);
    }

    void OnCardPointed(CardDisplay card) 
    {
        if(card == null)
        {
            handTimer = handHideTime;
        }
        else
        {
            handTimer = 0f;
            handHideSeq?.Kill();

            if (IsHandHided())
            {
                var p = card.transform.localPosition + handOffset;
                p.y = Screen.height;

                hand.localPosition = p;

                hand.gameObject.SetActive(true);
            }

            hand.DOLocalMove(card.transform.localPosition + handOffset, 1f);
        }
    }

    bool IsHandHided()
    {
        return !hand.gameObject.activeSelf;
    }

    void HideHand()
    {
        handTimer = 0f;
        var curPos = hand.localPosition;
        curPos.y = Screen.height;

        handHideSeq = DOTween.Sequence();
        handHideSeq.Append(hand.DOLocalMove(curPos, 1f)).AppendCallback(() => { hand.gameObject.SetActive(false); });
    }

    public void Refresh()
    {
        cardDisplayList.ForEach(x => x.RefreshPrice());
    }
}
