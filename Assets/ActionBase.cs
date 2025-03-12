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

    private void Awake()
    {
        bagManager = GetComponentInParent<BagManager>();
        if (GetComponentInParent<BagManager>())
        {
            isFriendly =  GetComponentInParent<BagManager>().isFriendly;
            
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