using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletArriveBase : ActionBase
{
    public GameObject effect;
    public virtual void BulletArrived(RectTransform  pos)
    {
        if (effect)
        {
            var go =Instantiate(effect, GameRoundManager.GetWorldPosition(pos), Quaternion.identity);
            Destroy(go, 1f);
        }
    }
}
public class BulletHeal : BulletArriveBase
{
    public override void BulletArrived(RectTransform  pos)
    {
        base.BulletArrived(pos);
        if (GetComponentInParent<BagItem>())
        {
            GetComponentInParent<BagItem>().Heal(1);
        }
    }
}
