using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public BagManager playerBagManager;
    public Bag playerBag;
    public BagManager enemyBagManager;
    public Bag enemyBag;
    private BagLoader bagLoader;

    public int wave;
    // Start is called before the first frame update
    public void Init()
    {
        bagLoader = GetComponent<BagLoader>();
        playerBagManager.Init();
        playerBag = playerBagManager.bag;
        enemyBagManager.Init();
        enemyBag = enemyBagManager.bag;

        //bagLoader.LoadRandomBagDataFromWave(wave);
    }

    public void LoadBagData()
    {
        bagLoader.LoadRandomBagDataFromWave(wave);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBattle()
    {
        BagData currentBagData = new BagData()
        {
            wave = wave,

        };
        
        var itemsData = new  List<BagItemData>();
        foreach (var item in playerBag.GetItems())
        {
            itemsData.Add(new BagItemData()
            {
                identifier = item.identifier,
                posX = item.gridPosition.x,
                posY = item.gridPosition.y
            });
        }
        currentBagData.items = itemsData;
        BagSaver.SaveBagData(currentBagData);

    }
}
