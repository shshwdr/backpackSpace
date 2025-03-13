using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingAttack : ActionBase
{
   public GameObject bullet;
   protected override void Fire()
   {
      base.Fire();
      
      if (bagitem.isDisabled)
      {
         return;
      }
      var enemyBagManager = BattleManager.Instance.getEnemyBagManager(bagManager);
      var hit = false;
      HashSet<BagItem> hitItems = new HashSet<BagItem>();

      var worldPos = GetWorldPosition();
      
      var go = Instantiate(bullet, worldPos, Quaternion.identity);
      Destroy(go,0.3f);
      if (!bagManager.isFriendly)
      {
         go.transform.localScale = new Vector3(-1,1,1);
      }
      
      
      
      for (int i = 0; i < 5; i++)
      {
         var gridPos = new Vector2Int(i, bagitem.gridPosition.y);
         BagItem hitItem = enemyBagManager.GetItemAtGridPosition(gridPos);
         if (hitItem != null && !hitItem.isDead)
         {
            if (hitItems.Contains(hitItem))
               continue;
            hitItems.Add(hitItem);
            hitItem.TakeDamage( itemInfo.hit);
            hit = true;
         }
      }

      if (!hit)
      {
         BattleManager.Instance.ApplyDamageToMainItem(enemyBagManager, itemInfo.hit);
      }
      
   }
   
   
}
