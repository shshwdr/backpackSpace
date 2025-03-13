using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadActionBase : MonoBehaviour
{
    
    public bool isFriendly = true; // 我方为 true，敌方为 false

    protected float timer;

    protected BagItem bagitem;
    protected ItemInfo itemInfo;
    protected BagManager bagManager;

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

    public virtual void DeadAction()
    {
        
    }
}
