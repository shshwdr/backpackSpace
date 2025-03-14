using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包管理器，负责创建和管理一个固定大小的背包，并提供接口进行物体放置和查询。
/// </summary>
public class BagManager : MonoBehaviour
{
    [Header("背包设置")]
    public int bagWidth = 10;
    public int bagHeight = 10;

    [HideInInspector]
    public Bag bag;

    public RectTransform mainItem;
    
    public List<BagItem> bagItems = new List<BagItem>();
    public bool isFriendly;
    public int deadCount = 0;

    public List<BagItem> aliveBagItems;

    public void StartBattle()
    {
        aliveBagItems = bagItems.ToList();
        deadCount = 0;


    }
   public void Init()
    {
        bag = new Bag(bagWidth, bagHeight);
    }

    public void Reset()
    {
        if (!isFriendly)
        {
            
            bag.Reset();
        }
        
        foreach (var disa in GetComponentsInChildren<DisableFront>())
        {
            disa.Reset();
        }
    }
    public void Die()
    {
        foreach (var item in aliveBagItems)
        {
                if (item.info.identifier == "revenge")
                {
                    var position =
                        GameRoundManager.GetWorldPosition(item.GetComponentInChildren<Image>()
                            .GetComponent<RectTransform>());
                    var go = Instantiate(BattleManager.Instance.ghostEffect, position, Quaternion.identity);
                    Destroy(go, 1);
                }
        }
    }

    /// <summary>
    /// 尝试将物体放入背包中，anchorPos 为物体在背包中锚点的位置（格子坐标）。
    /// </summary>
    public bool TryPlaceItem(BagItem item, Vector2Int anchorPos, bool isFlipped)
    {
        if (bag.PlaceItem(item, anchorPos,isFlipped))
        {
            Debug.Log("物体放置成功，位置：" + anchorPos);
            bagItems.Add(item);
            return true;
        }
        else
        {
            Debug.Log("无法放置物体在 " + anchorPos + "，请检查是否有重叠或超出背包范围");
            return false;
        }
    }
    
    public void TryRemoveItem(BagItem item)
    {
        bag.RemoveItem(item);
        {
            Debug.Log("物体移除成功");
            bagItems.Remove(item);
           // return true;
        }
        // else
        // {
        //     Debug.Log("物体移除失败");
        //     return false;
        // }
    }

    /// <summary>
    /// 获取物体在背包中的锚点位置
    /// </summary>
    public Vector2Int GetItemPosition(BagItem item)
    {
        return item.gridPosition;
    }
    /// <summary>
    /// 检查背包中是否有物体占据了给定的格子（考虑物体可能占用多个格子）
    /// </summary>
    public BagItem GetItemAtGridPosition(Vector2Int gridPos)
    {
        foreach (var item in aliveBagItems)
        {
            // 遍历该物体占用的所有格子
            foreach (var offset in item.shape)
            {
                if (item.gridPosition + offset == gridPos)
                {
                    return item;
                }
            }
        }
        return null;
    }
}
