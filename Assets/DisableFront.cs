using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFront : MonoBehaviour
{
    public GameObject effectPrefab;
    private List<GameObject> disableTarget = new List<GameObject>();
    private  List<GameObject> effects = new List<GameObject>();

    public void Reset()
    {
        foreach (var effect in effects)
        {
            if (effect)
            {
                Destroy(effect);
            }
        }
        disableTarget.Clear();
        effects.Clear();
    }
    private void Update()
    {
        if (!BattleManager.Instance.isBattling)
        {
            return;
        }
        int i = 0;
        int step = +1;
        int target = 4;
        var bagitem = GetComponent<BagItem>();
        var bagManager = bagitem.GetComponentInParent<BagManager>();
        var enemyBagManager =
            BattleManager.Instance.getEnemyBagManager(bagManager);
        bool isPlayer = bagManager.isFriendly;
        // if (isPlayer)
        // {
        //     i = 4;
        //     step = -1;
        //     target = 0;
        //
        // }


        while (i != target)
        {
            var found = false;
            {
                
                var gridPos = new Vector2Int(i, bagitem.gridPosition.y);
                if (isPlayer)
                {
                    
                }
                BagItem hitItem = enemyBagManager.GetItemAtGridPosition(gridPos);
                if (hitItem)
                {
                    if (!disableTarget.Contains(hitItem.gameObject))
                    {
                        var pos = hitItem.WorldPos;
                        var effect = Instantiate(effectPrefab, pos, Quaternion.identity);
                        disableTarget.Add(hitItem.gameObject);
                        effects.Add(effect);
                        
                        hitItem.isDisabled = true;
                    }
                    found = true;
                }
            }
            

            {
                
                var gridPos = new Vector2Int(i, bagitem.gridPosition.y+1);
                BagItem hitItem = enemyBagManager.GetItemAtGridPosition(gridPos);
                if (hitItem)
                {
                    if (!disableTarget.Contains(hitItem.gameObject))
                    {
                        var pos = hitItem.WorldPos;
                        var effect = Instantiate(effectPrefab, pos, Quaternion.identity);
                        disableTarget.Add(hitItem.gameObject);
                        effects.Add(effect);
                        hitItem.isDisabled = true;
                    }
                    found = true;
                }
            }
            if (found)
            {
                break;
            }
            i+=step;
        }
    }

    private void OnDisable()
    {
        foreach (var effect in effects)
        {
            if (effect)
            {
                Destroy(effect);
            }
        }

        foreach (var target in disableTarget)
        {
            if (target)
            {
                target.GetComponent<BagItem>().isDisabled = true;
            }
        }
    }
}
