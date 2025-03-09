using UnityEngine;

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

    void Awake()
    {
        bag = new Bag(bagWidth, bagHeight);
    }

    /// <summary>
    /// 尝试将物体放入背包中，anchorPos 为物体在背包中锚点的位置（格子坐标）。
    /// </summary>
    public bool TryPlaceItem(BagItem item, Vector2Int anchorPos)
    {
        if (bag.PlaceItem(item, anchorPos))
        {
            Debug.Log("物体放置成功，位置：" + anchorPos);
            return true;
        }
        else
        {
            Debug.Log("无法放置物体在 " + anchorPos + "，请检查是否有重叠或超出背包范围");
            return false;
        }
    }

    /// <summary>
    /// 获取物体在背包中的锚点位置
    /// </summary>
    public Vector2Int GetItemPosition(BagItem item)
    {
        return item.gridPosition;
    }
}
