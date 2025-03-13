using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawCardsMenu : MenuBase
{
    public Transform parent;
    
    public RectTransform discardArea;

    public GameObject detailObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    
    public void Refresh()
    {
        GameRoundManager.Instance.SpendGold(2);
        GameRoundManager.Instance.DoTrade();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Show()
    {
        base.Show();
        foreach (Transform trans in parent)
        {
            Destroy(trans.gameObject);
        }

        var all = CSVLoader.Instance.ItemInfoDict.Values.Where(x=>x.allfinished).ToList();
        
        for(int i = 0;i<3;i++)
        {
            var info = all.PickItem();
            var prefab = Resources.Load<GameObject>("SpaceShip/" + info.identifier);
             GameObject newGo = Instantiate(prefab, parent);
             newGo.AddComponent<BagItemDragHandler>();
             newGo.GetComponent<BagItem>().PlaceInShop();
        }
        
        
        discardArea.gameObject.SetActive(false);
    }

}
