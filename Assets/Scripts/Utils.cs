using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Utils
{
    public const float Tau = (float)(2 * Math.PI);
    public static List<T> Merge<T>(List<T> leftList, List<T> rightList, Comparer<T> comp)
    {
        int i = 0, j = 0;
        List<T> list = new();

        while (i < leftList.Count && j < rightList.Count)
        {
            if (comp.Compare(leftList[i], rightList[j]) <= 0)
            {
                list.Add(leftList[i]);
                i++;
            }
            else
            {
                list.Add(rightList[j]);
                j++;
            }
        }

        while (i < leftList.Count)
        {
            list.Add(leftList[i]);
            i++;
        }

        while (j < rightList.Count)
        {
            list.Add(rightList[j]);
            j++;
        }

        return list;
    }

    public static IEnumerable<(T, T)> Pairwise<T>(this IEnumerable<T> seq)
    {
        using var enumerator = seq.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            yield break;
        }
        var prev = enumerator.Current;
        while (enumerator.MoveNext())
        {
            yield return (prev, enumerator.Current);
            prev = enumerator.Current;
        }
    }
    
    public static IEnumerable<(float, float)> PairwiseRing(this IEnumerable<float> seq)
    {
        using var enumerator = seq.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            yield break;
        }

        var first = enumerator.Current;
        var prev = enumerator.Current;
        while (enumerator.MoveNext())
        {
            yield return (prev, enumerator.Current);
            prev = enumerator.Current;
        }

        yield return (prev, first + Tau);
    }

    public static Vector2 ToCartesian(float r, float theta)
    {
        return r * new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
    }
}