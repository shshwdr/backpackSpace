using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleManager : Singleton<BattleManager>
{
    public BagManager playerBagManager;
    public Bag playerBag;
    public BagManager enemyBagManager;
    public Bag enemyBag;
    private BagLoader bagLoader;

    [HideInInspector]
    public RectTransform friendMainItemRect;
    [HideInInspector]
    public RectTransform enemyMainItemRect;
    public int wave;
    // Start is called before the first frame update
    public void Init()
    {
        bagLoader = GetComponent<BagLoader>();
        playerBagManager.Init();
        playerBag = playerBagManager.bag;
        enemyBagManager.Init();
        enemyBag = enemyBagManager.bag;

        friendMainItemRect = playerBagManager.mainItem;
        enemyMainItemRect = enemyBagManager.mainItem;
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
    
    [Header("总血量")]
    public int friendlyTotalHP = 100;
    public int enemyTotalHP = 100;




    public void ApplyDamageToEnemy(int dmg)
    {
        enemyTotalHP -= dmg;
        Debug.Log("Enemy Total HP: " + enemyTotalHP);
        if (enemyTotalHP <= 0)
        {
            Debug.Log("Victory! Enemy defeated.");
            // 处理胜利逻辑
        }
    }

    public void ApplyDamageToFriendly(int dmg)
    {
        friendlyTotalHP -= dmg;
        Debug.Log("Friendly Total HP: " + friendlyTotalHP);
        if (friendlyTotalHP <= 0)
        {
            Debug.Log("Defeat! Friendly forces lost.");
            // 处理失败逻辑
        }
    }
}
