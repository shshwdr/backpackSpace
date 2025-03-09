using UnityEngine;
using UnityEngine.UI;

public class BackpackUI : MonoBehaviour
{
    [Header("背包设置")]
    public int columns = 5;
    public int rows = 4;
    public GameObject slotPrefab;        // 背包格子预制体（例如 Image 或自定义背包格子）
    public RectTransform backpackPanel;  // 背包区域的 Panel

    [Header("布局设置")]
    public Vector2 cellSize = new Vector2(50, 50);
    public Vector2 spacing = new Vector2(5, 5);

    void Start()
    {
        // 在背包面板上添加 GridLayoutGroup（如果尚未添加）
        GridLayoutGroup grid = backpackPanel.GetComponent<GridLayoutGroup>();
        if (grid == null)
        {
            grid = backpackPanel.gameObject.AddComponent<GridLayoutGroup>();
        }
        grid.cellSize = cellSize;
        grid.spacing = spacing;
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        // 可根据需要设置 grid.startCorner 为 Lower Left（如果想让锚点在左下角）
        grid.startCorner = GridLayoutGroup.Corner.LowerLeft;

        // 根据行数和列数生成所有格子
        int totalSlots = columns * rows;
        for (int i = 0; i < totalSlots; i++)
        {
            Instantiate(slotPrefab, backpackPanel);
        }
    }

    /// <summary>
    /// 根据格子坐标返回对应的背包格子 Image 组件
    /// </summary>
    /// <param name="gridPos">格子坐标，假设左下角为 (0,0)，向右增加 X，向上增加 Y</param>
    /// <returns>对应的 Image 组件，如果坐标超出范围则返回 null</returns>
    public Image GetSlotAt(Vector2Int gridPos)
    {
        // 计算对应的索引，假设格子的排列顺序与 GridLayoutGroup 的 startCorner 为 LowerLeft
        int index = gridPos.y * columns + gridPos.x;
        if (index < 0 || index >= backpackPanel.childCount)
        {
            Debug.LogWarning($"GetSlotAt: gridPos {gridPos} 超出背包范围！");
            return null;
        }
        return backpackPanel.GetChild(index).GetComponent<Image>();
    }
}