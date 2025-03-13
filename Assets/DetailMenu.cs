using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetailMenu : Singleton<DetailMenu>
{
    public TMP_Text label;
    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    public void Show(BagItem item)
    {
        var itemInfo = CSVLoader.Instance.ItemInfoDict[item.identifier];
        label.text = itemInfo.name + 
                     //"\n" + "HP: " + itemInfo.hp+
                     "\n" + string.Format( itemInfo.desc, itemInfo.hit, itemInfo.cooldown);
    }
}
