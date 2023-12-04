using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public static class Utils
{
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
}