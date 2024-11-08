using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T GetRandom<T>(this List<T> list)
    {
        if (list == null)
        {
            return default;
        }

        if (list.Count == 0)
        {
            return default;
        }

        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }
}
