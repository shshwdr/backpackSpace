using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class BagSaver
{
    // 将数据保存到 Resources 文件夹下的 Assets/Resources/BagData/bagdata.json
    private static string folderPath = Path.Combine(Application.dataPath, "Resources", "BagData");
    private static string filePath = Path.Combine(folderPath, "bagdata.json");

    /// <summary>
    /// 保存一份新的背包数据到 Resources 文件夹中（仅编辑器下可写）
    /// </summary>
    public static void SaveBagData(BagData newData)
    {
#if UNITY_EDITOR
        // 确保文件夹存在
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        BagDataList dataList = LoadBagDataList();

        // 检查是否存在完全相同的数据
        foreach (var data in dataList.bagDataList)
        {
            if (IsBagDataEqual(data, newData))
            {
                Debug.Log("已有完全相同的数据，不再保存。");
                return;
            }
        }

        dataList.bagDataList.Add(newData);
        string json = JsonUtility.ToJson(dataList, true);
        File.WriteAllText(filePath, json);
        Debug.Log("背包数据保存成功，路径：" + filePath);
        
        // 刷新编辑器中的资源数据
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    /// <summary>
    /// 从 Resources 文件夹中加载背包数据列表
    /// </summary>
    public static BagDataList LoadBagDataList()
    {
        // 从 Resources 中加载 bagdata.json，对应路径为 "BagData/bagdata"（不带扩展名）
        TextAsset textAsset = Resources.Load<TextAsset>("BagData/bagdata");
        if (textAsset != null)
        {
            BagDataList dataList = JsonUtility.FromJson<BagDataList>(textAsset.text);
            if (dataList == null)
            {
                dataList = new BagDataList();
                dataList.bagDataList = new List<BagData>();
            }
            return dataList;
        }
        else
        {
            Debug.LogWarning("没有在 Resources/BagData 找到 bagdata.json 文件！");
            BagDataList dataList = new BagDataList();
            dataList.bagDataList = new List<BagData>();
            return dataList;
        }
    }


    /// <summary>
    /// 按波次分组加载数据
    /// </summary>
    public static Dictionary<int, List<BagData>> LoadBagDataGroupedByWave()
    {
        BagDataList dataList = LoadBagDataList();
        Dictionary<int, List<BagData>> grouped = new Dictionary<int, List<BagData>>();
        foreach (var data in dataList.bagDataList)
        {
            if (!grouped.ContainsKey(data.wave))
                grouped[data.wave] = new List<BagData>();
            grouped[data.wave].Add(data);
        }
        return grouped;
    }

    private static bool IsBagDataEqual(BagData a, BagData b)
    {
        if (a.wave != b.wave) return false;
        if (a.items == null && b.items == null) return true;
        if (a.items == null || b.items == null) return false;
        if (a.items.Count != b.items.Count) return false;
        for (int i = 0; i < a.items.Count; i++)
        {
            BagItemData itemA = a.items[i];
            BagItemData itemB = b.items[i];
            if (itemA.identifier != itemB.identifier ||
                itemA.posX != itemB.posX ||
                itemA.posY != itemB.posY ||
                 itemA.level != itemB.level)
            {
                return false;
            }
        }
        return true;
    }
}
