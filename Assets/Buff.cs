using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public GameObject effectPrefab;

    public List<Buff> buffs;
    public List<GameObject> effects;
    
    public void Reset()
    {
        foreach (var effect in effects)
        {
            if (effect)
            {
                Destroy(effect);
            }
        }
        buffs.Clear();
        effects.Clear();
    }
    private void Update()
    {
        if (!BattleManager.Instance.isBattling)
        {
            return;
        }

        var bagitem = GetComponent<BagItem>();
        var bagManager = bagitem.GetComponentInParent<BagManager>();
        for (int i = 0; i < bagitem.gridPosition.y; i++)
        {
            
            var gridPos = new Vector2Int(bagitem.gridPosition.x,i);
          //  BagItem hitItem = enemyBagManager.GetItemAtGridPosition(gridPos);
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

        foreach (var target in buffs)
        {
            if (target)
            {
                target.GetComponent<BagItem>().isDisabled = true;
            }
        }
    }


}
