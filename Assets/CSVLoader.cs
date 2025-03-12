using System.Collections;
using System.Collections.Generic;
using Sinbad;
using UnityEngine;


public class ItemInfo
{
    public string identifier;
    public int hp;
    public string desc;
    public string name;
    public string story;
    public int cost;
    public int hit;
    public float cooldown;


}
public class CSVLoader : Singleton<CSVLoader>
{
    
    public Dictionary<string, ItemInfo> ItemInfoDict = new Dictionary<string, ItemInfo>();

    public void Init()
    {
        var characterInfos =
            CsvUtil.LoadObjects<ItemInfo>(GetFileNameWithABTest("item"));
        foreach (var info in characterInfos)
        {
            ItemInfoDict[info.identifier] = info;
        }
    }
    
    string GetFileNameWithABTest(string name)
    {
        // if (ABTestManager.Instance.testVersion != 0)
        // {
        //     var newName = $"{name}_{ABTestManager.Instance.testVersion}";
        //     //check if file in resource exist
        //      
        //     var file = Resources.Load<TextAsset>("csv/" + newName);
        //     if (file)
        //     {
        //         return newName;
        //     }
        // }
        return name;
    }
}

