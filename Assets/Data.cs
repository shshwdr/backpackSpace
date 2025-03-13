using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BagItemData
{
    public string identifier; // 物体标识符
    public int posX;          // 格子坐标X
    public int posY;          // 格子坐标Y
    public int level;
}

[Serializable]
public class BagData
{
    public int wave;                  // 当前波次
    public List<BagItemData> items;   // 该波次下所有物体数据
}

[Serializable]
public class BagDataList
{
    public List<BagData> bagDataList;
}