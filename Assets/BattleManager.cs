using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

    public float maxTime = 120;

    public float currentTimer = 120;

    [HideInInspector]
    public RectTransform friendMainItemRect;
    [HideInInspector]
    public RectTransform enemyMainItemRect;

    public bool isBattling = false;

    public TMP_Text waveText;
    public GameObject ghostEffect;

    public GameObject battleCanvas;
    public Button speedButton;

    private int speedIndex = 0;
    List<int>  speedList = new List<int>() { 1, 2, 4 };
    
    

    public void ApplyDamageToMainItem(BagManager bagManager,int dmg)
    {
        if (bagManager == playerBagManager)
        {
            ApplyDamageToFriendly(dmg);
        }
        else
        {
            ApplyDamageToEnemy(dmg);
        }
    }
    public BagManager getEnemyBagManager(BagManager bagManager)
    {
        return bagManager == playerBagManager ? enemyBagManager : playerBagManager;
    }
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
        speedButton.onClick.AddListener(() =>
            {
                speedIndex++;
                if (speedIndex >= speedList.Count)
                {
                    speedIndex = 0;
                }

                speedButton.GetComponentInChildren<TMP_Text>().text = "x" + speedList[speedIndex].ToString();
                Time.timeScale = speedList[speedIndex];
            }
        );
        battleCanvas.SetActive(false);
        
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
        if (isBattling)
        {
            
            currentTimer -= Time.deltaTime;
            timeLabel.text = currentTimer.ToString("F0");
            if (currentTimer <= 0)
            {
                EndBattle(false);
            }
            
        }
    }

    public TMP_Text timeLabel;
    public void StartBattle()
    {
        MusicManager.Instance.PlayBattleMusic();
        battleCanvas.SetActive(true);
        Captain.Instance.SetNormal();
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
                posY = item.gridPosition.y,
                level = item.level,
            });
        }
        currentBagData.items = itemsData;
        BagSaver.SaveBagData(currentBagData);

        enemyCurrentHP = enemyTotalHP;
        friendlyCurrentHP = friendlyTotalHP;
        
        isBattling = true;

        currentTimer = maxTime;

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
        winLoseCanvas.DOFade(1, 2f).SetLoops(2, LoopType.Yoyo);
        if (isWin)
        {
            winLoseCanvas.GetComponentInChildren<TMP_Text>().text = "WIN!";
            //GameRoundManager.Instance.GameWin();
        }
        else
        {
            if (currentTimer <= 0)
            {
                winLoseCanvas.GetComponentInChildren<TMP_Text>().text = "Time Out!";
            }
            else
            {
                
                winLoseCanvas.GetComponentInChildren<TMP_Text>().text = "Lose!";
            }
            GameRoundManager.Instance.ReduceHP();
            //GameRoundManager.Instance.GameLose();
        }

        if (isWin)
        {
            
            Captain.Instance.SetHappy();
        }
        else
        {
            Captain.Instance.SetSad();
        }
        // 清理战斗相关资源
        CLearBattle();
    }
    
    public void CLearBattle()
    {
        
        MusicManager.Instance.StopBattleMusic();
        battleCanvas.SetActive(false);
        isBattling = false;
        GameRoundManager.Instance.currentWave++;
        if (GameRoundManager.Instance.currentWave > GameRoundManager.Instance.maxWave)
        {
            
        }
        else
        {
            waveText.text = "Wave\n" + GameRoundManager.Instance.currentWave+"/"+GameRoundManager.Instance.maxWave;
        }
        // 清理战斗相关资源
        
        foreach (var item in playerBagManager.GetComponentsInChildren<BagItem>(true))
        {
            if (item.isGenerated)
            {
                Destroy(item.gameObject);
            }
            else
            {
                item.Reset();
            }
        }
        foreach (var item in enemyBagManager.GetComponentsInChildren<BagItem>(true))
        {
            Destroy(item.gameObject);
        }

        foreach (Transform transform in bulletParent)
        {
            Destroy(transform.gameObject);
        }

        enemyBagManager.Reset();
         playerBagManager.Reset();
        enemyBagManager.bagItems.Clear();
        
        friendlyCurrentHP = friendlyTotalHP;
        enemyCurrentHP = enemyTotalHP;
        playerHPBar.SetHP(friendlyCurrentHP, friendlyTotalHP);
        enemyHPBar.SetHP(enemyCurrentHP, enemyTotalHP);
        
        
        GameRoundManager.Instance.Next();
    }
}
