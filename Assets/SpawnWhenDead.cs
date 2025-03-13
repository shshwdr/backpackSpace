using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWhenDead : DeadActionBase
{
    
    public GameObject prefab;

    public override void DeadAction()
    {
        base.DeadAction();
        var shapes = bagitem.shape;
        var go = Instantiate(prefab, bagitem.transform.position, Quaternion.identity, bagitem.transform.parent);
        bagManager.aliveBagItems.Add(go.GetComponent<BagItem>());
        go.GetComponent<BagItem>().gridPosition = bagitem.gridPosition;
        go.GetComponent<BagItem>().isGenerated = true;
        
        // for(int i = 0;i< 2;i++)
        // {
        //     var position =
        //         bagitem.gridPosition + shapes.PickItem();
        //     if (bagManager.TryPlaceItem(prefab.GetComponent<BagItem>(), position))
        //     {
        //     }
        //     
        // }
    }

}
