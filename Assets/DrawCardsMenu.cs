using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawCardsMenu : MenuBase
{
    public Transform parent;
    
    public RectTransform discardArea;
    // Start is called before the first frame update
    void Start()
    {
        
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

        var all = Resources.LoadAll<GameObject>("SpaceShip").ToList();
        for(int i = 0;i<4;i++)
        {
             GameObject newGo = Instantiate(all.PickItem(), parent);
             newGo.AddComponent<BagItemDragHandler>();
             newGo.GetComponent<BagItem>().PlaceInShop();
        }
        
        
        discardArea.gameObject.SetActive(false);
    }

}
