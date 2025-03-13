using System;
using UnityEngine;
using DG.Tweening;

public class ActionBase : MonoBehaviour
{
    public bool isFriendly = true; // 我方为 true，敌方为 false

    protected float timer;

    protected BagItem bagitem;
    protected ItemInfo itemInfo;
    protected BagManager bagManager;


    public Vector3 GetWorldPosition()
    {
        return GameRoundManager.GetWorldPosition(GetComponent<RectTransform>());
        Vector3 uiWorldPos = transform.position;
        uiWorldPos.z = 0;
      
        Camera canvasCamera = Camera.main;

// 如果你想转换 uiRect 的屏幕坐标到世界坐标：
        Vector2 screenPos = transform.position;//RectTransformUtility.WorldToScreenPoint(canvasCamera, transform.position);
        Vector3 worldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(GetComponent<RectTransform>(), screenPos, canvasCamera, out worldPos);
        return worldPos;
    }
    private void Awake()
    {
        bagManager = GetComponentInParent<BagManager>();
        if (bagManager == null)
        {
            bagManager = BattleManager.Instance.playerBagManager;
        }
        if (bagManager)
        {
            isFriendly =  bagManager.isFriendly;
            
        }
        bagitem = GetComponentInParent<BagItem>();
        itemInfo  =  CSVLoader.Instance.ItemInfoDict[bagitem.identifier];
    }
    
    void Update()
    {
        if (GameRoundManager.Instance.isBattling)
        {
            BattleUpdate(Time.deltaTime);
        }
    }

    void BattleUpdate(float time)
    {
        timer += time;
        if (timer >= itemInfo.cooldown)
        {
            timer = 0f;
            Fire();
        }
    }

    protected virtual void Fire()
    {
        
    }
}