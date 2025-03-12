using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{

    private float timer;
    BagManager bagManager;

    private BagItem bagitem;
    ItemInfo itemInfo;
    private void Awake()
    {
        bagManager = GetComponentInParent<BagManager>();
        if (bagManager == null)
        {
            bagManager = BattleManager.Instance.playerBagManager;
        }
        bagitem = GetComponentInParent<BagItem>();
        itemInfo  =  CSVLoader.Instance.ItemInfoDict[bagitem.identifier];
    }

    // Start is called before the first frame update
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
            var candidates = new List<BagItem>();
            foreach (var item in bagManager.aliveBagItems)
            {
                if (item.hpNotFull())
                {
                    candidates.Add(item);
                }
            }
            if (candidates.Count > 0)
            {
                var target = candidates.RandomItem();
                target.Heal(itemInfo.hit);
            }

            
        }
    }
}
