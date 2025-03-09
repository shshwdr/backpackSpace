using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class ListExtension
{
    
    public static T PickItem<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            throw new System.InvalidOperationException("Cannot select a random item from an empty list.");
        }

        Random random = new Random();
        int index = random.Next(0, list.Count);
        var item = list[index];
        list.RemoveAt(index);
        return item;
    }
    public static T RandomItem<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            Debug.LogError("Cannot select a random item from an empty list.");
            return default(T);
        }

        Random random = new Random();
        int index = random.Next(0, list.Count);
        return list[index];
    }

    public static T LastItem<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            throw new System.InvalidOperationException("Cannot select the last item from an empty list.");
        }
        return list[list.Count - 1];
    }
    
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static int GetRnadomIndexWithWeight(this IList<int> weights)
    {
        int totalWeight = 0;
        foreach (int weight in weights)
        {
            totalWeight += weight;
        }

        Random random = new Random();
        int randomNumber = random.Next(totalWeight); // 生成一个 0 到 totalWeight 之间的随机数

        int accumulatedWeight = 0;
        int selectedIndex = -1;
        for (int i = 0; i < weights.Count; i++)
        {
            accumulatedWeight += weights[i];
            if (randomNumber < accumulatedWeight)
            {
                selectedIndex = i;
                break;
            }
        }

        return selectedIndex;
    }
}