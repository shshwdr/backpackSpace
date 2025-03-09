using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardsMenu : MenuBase
{
    public Transform parent;
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

        foreach (var go in Resources.LoadAll<GameObject>("SpaceShip"))
        {
             GameObject newGo = Instantiate(go, parent);
             newGo.AddComponent<BagItemDragHandler>();
        }
    }
}
