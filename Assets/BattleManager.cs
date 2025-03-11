using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleManager : Singleton<BattleManager>
{
    public Transform bulletParent;
    public BagManager playerBagManager;
    public Bag playerBag;
    public BagManager enemyBagManager;
    public Bag enemyBag;
    public HPBar playerHPBar;
    public HPBar enemyHPBar;
    private BagLoader bagLoader;

    [HideInInspector]
    public RectTransform friendMainItemRect;
    [HideInInspector]
    public RectTransform enemyMainItemRect;

    public bool isBattling = false;

    public TMP_Text waveText;
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
        bagLoader.LoadRandomBagDataFromWave(GameRoundManager.Instance.currentWave);
    }
    // Update is called once per frame
    void Update()
    {
        // if (isBattling)
        // {
        //     foreach (var attackModule in FindObjectsOfType<AttackModule>())
        //     {
        //         attackModule.BattleUpdate(Time.deltaTime);
        //     }
        // }
    }

    public void StartBattle()
    {
        BagData currentBagData = new BagData()
        {
            wave = GameRoundManager.Instance.CurrentWave,

        };
        
        playerBagManager.StartBattle();
        enemyBagManager.StartBattle();
        
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

        enemyCurrentHP = enemyTotalHP;
        friendlyCurrentHP = friendlyTotalHP;
        
        isBattling = true;

    }
    
    
    [Header("总血量")]
    public int friendlyTotalHP = 100;
    public int enemyTotalHP = 100;
    private int enemyCurrentHP;
    private int friendlyCurrentHP;




    public void ApplyDamageToEnemy(int dmg)
    {
        enemyCurrentHP -= dmg;
        if (enemyCurrentHP <= 0)
        {
            Debug.Log("Victory! Enemy defeated.");
            // 处理胜利逻辑
            
            EndBattle(true);
        }
        enemyHPBar.SetHP(enemyCurrentHP, enemyTotalHP);
    }

    public void ApplyDamageToFriendly(int dmg)
    {
        friendlyCurrentHP -= dmg;
        if (friendlyCurrentHP <= 0)
        {
            Debug.Log("Defeat! Friendly forces lost.");
            // 处理失败逻辑
            EndBattle(false);
        }
        playerHPBar.SetHP(friendlyCurrentHP, friendlyTotalHP);
    }

    public CanvasGroup winLoseCanvas;
    public void EndBattle(bool isWin)
    {
        winLoseCanvas.DOFade(1, 0.5f).SetLoops(2, LoopType.Yoyo);
        if (isWin)
        {
            winLoseCanvas.GetComponentInChildren<TMP_Text>().text = "WIN!";
            //GameRoundManager.Instance.GameWin();
        }
        else
        {
            winLoseCanvas.GetComponentInChildren<TMP_Text>().text = "Lose!";
            GameRoundManager.Instance.ReduceHP();
            //GameRoundManager.Instance.GameLose();
        }
        // 清理战斗相关资源
        CLearBattle();
    }
    
    public void CLearBattle()
    {
        isBattling = false;
        GameRoundManager.Instance.currentWave++;
        if (GameRoundManager.Instance.currentWave > GameRoundManager.Instance.maxWave)
        {
            
        }
        else
        {
            waveText.text = "Wave: " + GameRoundManager.Instance.currentWave+"/"+GameRoundManager.Instance.maxWave;
        }
        // 清理战斗相关资源
        
        foreach (var item in playerBagManager.GetComponentsInChildren<BagItem>(true))
        {
            item.Reset();
        }
        foreach (var item in enemyBagManager.GetComponentsInChildren<BagItem>(true))
        {
            Destroy(item.gameObject);
        }

        foreach (Transform transform in bulletParent)
        {
            Destroy(transform.gameObject);
        }
        
        friendlyCurrentHP = friendlyTotalHP;
        enemyCurrentHP = enemyTotalHP;
        playerHPBar.SetHP(friendlyCurrentHP, friendlyTotalHP);
        enemyHPBar.SetHP(enemyCurrentHP, enemyTotalHP);
        
        
        GameRoundManager.Instance.Next();
    }
}
