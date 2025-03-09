using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包中的物体，定义了该物体在背包中占用的格子。
/// 例如，shape 中可以定义 [(0,0), (1,0), (0,1)] 表示物体占用一个 L 形区域（锚点为左上角或其他约定位置）。
/// </summary>
public class BagItem : MonoBehaviour
{
    [Tooltip("物体在背包中占用的格子偏移，相对于锚点 (0,0)")]
    public List<Vector2Int> shape = new List<Vector2Int>();

    [HideInInspector]
    public Vector2Int gridPosition = new Vector2Int(-1, -1);
    
    public string identifier;
}