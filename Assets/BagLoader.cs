using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BagLoader : MonoBehaviour
{
    [Header("背包管理器引用")]
   BagManager bagManager; // 在 Inspector 中指定背包管理器（挂在背包面板上）

    private Transform placeParent;
    /// <summary>
    /// 根据指定的波次加载背包数据，从该波次的记录中随机选取一份，并实例化其中的物体到背包中。
    /// </summary>
    /// <param name="wave">指定的波次</param>
    ///
    private void Awake()
    {
        bagManager = GetComponent<BattleManager>().enemyBagManager;
        placeParent = bagManager.transform;
    }

    public void LoadRandomBagDataFromWave(int wave)
    {
        // 读取所有保存的背包数据并按波次分组
        Dictionary<int, List<BagData>> groupedData = BagSaver.LoadBagDataGroupedByWave();
        if (!groupedData.ContainsKey(wave))
        {
            Debug.LogWarning("Wave " + wave + " 没有保存的数据！");
            return;
        }

        List<BagData> dataList = groupedData[wave];
        // 从该波次的数据列表中随机选择一份数据
        int randomIndex = Random.Range(0, dataList.Count);
        BagData chosenData = dataList[randomIndex];

        Debug.Log("加载波次 " + wave + " 的背包数据，随机选取了第 " + randomIndex + " 份记录，共 " + chosenData.items.Count + " 个物体。");

        // 遍历记录中的每个物体数据，实例化预制体并尝试放置到背包中
        foreach (BagItemData itemData in chosenData.items)
        {
            // 实例化物品预制体，挂在背包管理器的 transform 下（或其他合适的父物体下）
            var bagItemPrefab = Resources.Load("SpaceShip/"+itemData.identifier) as GameObject;
            GameObject itemGO = Instantiate(bagItemPrefab, placeParent);
            BagItem bagItem = itemGO.GetComponent<BagItem>();
            if (bagItem == null)
            {
                Debug.LogError("预制体上没有找到 BagItem 组件！");
                continue;
            }
            // 设置物品标识符（确保 BagItem 中有 identifier 字段）
            //bagItem.identifier = itemData.identifier;
            
            // 根据保存的格子坐标，尝试将物品放入背包中
            Vector2Int gridPos = new Vector2Int(itemData.posX, itemData.posY);
            bool placed = bagManager.TryPlaceItem(bagItem, gridPos);
            if (!placed)
            {
                Debug.LogWarning("物品 " + itemData.identifier + " 在位置 " + gridPos + " 放置失败！");
            }

            var backpackUI = placeParent.GetComponent<BackpackUI>();
            // 获取背包面板的 RectTransform
            RectTransform bagRect = backpackUI.backpackPanel.GetComponent<RectTransform>();

            // 计算该格子的左下角位置，假设背包面板的 pivot 为左下角
            // 注意：如果背包面板的 pivot 不是左下角，请参照前面代码进行补偿
            float posX = bagRect.rect.xMin + gridPos.x *backpackUI. cellSize.x;
            float posY = bagRect.rect.yMin + gridPos.y * backpackUI. cellSize.x;

            // 获取物品的 RectTransform
            RectTransform itemRect = itemGO.GetComponent<RectTransform>();
            if (itemRect != null)
            {
                // 这里直接设置 anchoredPosition，使物品的左下角与格子左下角对齐
                itemRect.anchoredPosition = new Vector2(posX, posY);
                
                // 如果需要与格子中心对齐，可额外加上半个 cellSize 的偏移：
                // itemRect.anchoredPosition += new Vector2(cellSize * 0.5f, cellSize * 0.5f);
                
                // 也可以通过获取格子预制体的 Image 组件获得格子中心的 worldPosition
                Image slotImage = backpackUI.GetSlotAt(gridPos);
                if (slotImage != null)
                {
                    // 将物品的 worldPosition 调整到格子中心（再偏移半个 cellSize，使其对齐）
                    var cellSize = backpackUI.cellSize.x / 2 ;
                    #if UNITY_EDITOR
                    #else
                    cellSize += 25;
                    #endif
                    itemRect.position = slotImage.transform.position - new Vector3(cellSize, cellSize,0);
                    
                   }
                
                
            }
        
        }
    }
}