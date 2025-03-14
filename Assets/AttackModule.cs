using System;
using UnityEngine;
using DG.Tweening;

public class AttackModule : ActionBase
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;

   override protected void Fire()
    {
        if (bagitem.shape.Count > 2)
        {
            SFXManager.Instance.PlaySFX("ChilliJam_Space_Ship_Attack_Big");
        }
        else
        {
            SFXManager.Instance.PlaySFX("ChilliJam_Space_Ship_Attack_Small");
        }
        base.Fire();
        
        if (bagitem.isDisabled)
        {
            return;
        }
        // 从武器位置实例化子弹
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity,BattleManager.Instance.bulletParent);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            // 我方向右，敌方向左
            Vector2 dir = isFriendly ? Vector2.right : Vector2.left;

            var value = itemInfo.hit;
            if (bagitem.identifier == "revenge")
            {
                value += bagManager.deadCount;
            }

        bullet.Initialize(dir, bulletSpeed, value , isFriendly,itemInfo,GetComponent<BulletArriveBase>());
        }
    }
}