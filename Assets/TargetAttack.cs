using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetAttack : ActionBase
{
    public GameObject effect;
    protected override void Fire()
    {
        base.Fire();
        if (bagitem.isDisabled)
        {
            return;
        }
        
        var enemyBagManager =  BattleManager.Instance.getEnemyBagManager(bagManager);
        if (enemyBagManager.aliveBagItems.Count == 0)
        {
            BattleManager.Instance.ApplyDamageToMainItem(enemyBagManager, itemInfo.hit);
            
            var pos = GameRoundManager.GetWorldPosition(enemyBagManager.mainItem.GetComponent<RectTransform>());
            var go = Instantiate(effect, pos, Quaternion.identity);
            Destroy(go,1);
            return;
        }

        var minHP = 100;
        BagItem target = null;
        foreach (var bagItem in enemyBagManager.aliveBagItems)
        {
            if (bagItem.currentHP < minHP)
            {
                minHP = bagitem.currentHP;
                target = bagItem;
            }
        }

        if (target)
        {
            target.TakeDamage(itemInfo.hit);
            var pos = GameRoundManager.GetWorldPosition(target.GetComponentInChildren<Image>().GetComponent<RectTransform>());
            var go = Instantiate(effect, pos, Quaternion.identity);
            Destroy(go,1);
        }
        
    }
}
