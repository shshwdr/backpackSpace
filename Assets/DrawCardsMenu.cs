using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DrawCardsMenu : MenuBase
{
    public Transform parent;
    
    public RectTransform discardArea;

    public GameObject detailObj;

    public Button refreshButton;
    // Start is called before the first frame update
    void Start()
    {
        refreshButton.onClick.AddListener(Refresh);
    }
    
    
    public void Refresh()
    {
        GameRoundManager.Instance.SpendGold(1);
        //GameRoundManager.Instance.DoTrade();
        Show();
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

        var all = CSVLoader.Instance.ItemInfoDict.Values.Where(x=>x.allfinished && x.unlockWave <=  GameRoundManager.Instance.CurrentWave).ToList();
        
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
