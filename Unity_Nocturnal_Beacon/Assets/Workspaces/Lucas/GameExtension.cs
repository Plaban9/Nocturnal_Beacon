using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class GameExtensions
{
    private static readonly System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static Stack<int> ToShuffleIndex<T>(this IList<T> list)
    {
        int n = list.Count;
        HashSet<int> h = new HashSet<int>();

        while (n > 1)
        {
            n--;
            int k;
            do
            {
                k = rng.Next(n + 1);
            } while (!h.Add(k));
        }

        return h.ToStack();
    }

    public static Stack<T> ToStack<T>(this HashSet<T> hashSet)
    {
        Stack<T> s = new Stack<T>();

        foreach (var t in hashSet)
            s.Push(t);

        return s;
    }

    public static T Pop<T>(this IList<T> list)
    {
        var r = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return r;
    }

    public static int GetUIdForNextCard(this Deck deck)
    {

        return GetUIdForNextCard(deck.Export());
    }

    public static int GetUIdForNextCard(this IList<Card> cardList)
    {
        if (cardList.Count == 0)
            return 1000001;
        else
            return cardList.Max(x => x.uId) + 1;
    }

    public static void AddCard(this IList<Card> cardList, Card card)
    {
        //if(card.uId == 0)
        //    card.orderId = cardList.GetUIdForNextCard();

        cardList.Add(card);
    }

    public static void InsertCard(this IList<Card> cardList, Card card, int index)
    {
        //if (card.uId == 0)
        //    card.orderId = cardList.GetUIdForNextCard();

        cardList.Insert(index, card);
    }

    public static List<Card> Clone(this IList<Card> cardList)
    {
        var cl = new List<Card>();

        foreach (var c in cardList)
            cl.Add(c);

        return cl;
    }

}
