using System;
using System.Collections.Generic;

public class Utils
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
}