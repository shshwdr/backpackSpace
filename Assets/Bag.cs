using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包数据结构，用二维网格记录每个格子是否被占用，以及占用的物体引用。
/// </summary>
public class Bag
{
    public int width;
    public int height;
    // grid[x,y] 为 null 表示空格，否则为占用该格子的物体
    private BagItem[,] grid;

    public Bag(int width, int height)
    {
        this.width = width;
        this.height = height;
        grid = new BagItem[width, height];
    }

    /// <summary>
    /// 检查物体是否能以指定的锚点放置在背包中。
    /// 物体的 shape 是一个相对位置列表（锚点为 (0,0)）。
    /// </summary>
    public bool CanPlaceItem(BagItem item, Vector2Int anchorPos)
    {
        foreach (Vector2Int offset in item.shape)
        {
            Vector2Int cell = anchorPos + offset;
            // 检查是否超出边界
            if (cell.x < 0 || cell.x >= width || cell.y < 0 || cell.y >= height)
                return false;
            // 检查该格子是否已被占用
            if (grid[cell.x, cell.y] != null)
                return false;
        }
        return true;
    }

    /// <summary>
    /// 尝试将物体放置到背包中，放置成功返回 true，否则返回 false
    /// </summary>
    public bool PlaceItem(BagItem item, Vector2Int anchorPos,bool isFlipped)
    {
        if (!CanPlaceItem(item, anchorPos))
            return false;
        foreach (Vector2Int offset in item.shape)
        {
            var newOffset = offset;
            if (isFlipped)
            {
                newOffset.x = -offset.x;
            }
            Vector2Int cell = anchorPos + newOffset;
            grid[cell.x, cell.y] = item;
        }
        item.gridPosition = anchorPos;
        return true;
    }
    
    
    

    /// <summary>
    /// 将物体从背包中移除
    /// </summary>
    public void RemoveItem(BagItem item)
    {
        if (item.gridPosition.x < 0 || item.gridPosition.y < 0)
            return;
        foreach (Vector2Int offset in item.shape)
        {
            Vector2Int cell = item.gridPosition + offset;
            if (cell.x >= 0 && cell.x < width && cell.y >= 0 && cell.y < height)
            {
                if (grid[cell.x, cell.y] == item)
                    grid[cell.x, cell.y] = null;
            }
        }
        item.gridPosition = new Vector2Int(-1, -1);
    }
    
    public List<BagItem> GetItems()
    {
        List<BagItem> items = new List<BagItem>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] != null && !items.Contains(grid[x, y]))
                    items.Add (grid[x, y]);
            }
        }
        return items;
    }
}