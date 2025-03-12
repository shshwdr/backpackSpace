using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameRoundManager : Singleton<GameRoundManager>
{
    enum StateType
    {
        start,
        draw,
        battle,
    };

    public int maxHP = 3;
    private int currentHP = 3;
    public bool isBattling => currentState == StateType.battle;
    private int gold;
    public int CurrentChapter=>currentChapter;

    private int currentChapter = 2;

    public int maxWave = 2;
    
    
    public string CurrentChapterIdentifier =>currentArea  + "_" + currentChapter;
    public string CurrentWaveIdentifier =>  CurrentChapterIdentifier + "_" + currentWave;

    public string WaveIdentifier(int wave)
    {
        return CurrentChapterIdentifier + "_" + wave;
    }
    public int CurrentArea=>currentArea;

    public int currentArea = 1;

    public int CurrentWave => currentWave;
    public int LastWave=> currentWave-1;
    public int currentWave = 1;

    public Transform hpParent;
    
    public int TotalWaves;
    public int Gold => gold;
    private StateType currentState = StateType.start;

    public TMP_Text goldLabel;
    public bool hasEnoughGold(int value)
    {
         return gold >= value;
    }
    public void AddGold(int value)
    {
        gold += value;
        goldLabel.text = gold.ToString();
        EventPool.Trigger("updateGold");
    }

    public void DoTrade()
    {
        foreach (var item in BattleManager.Instance.playerBagManager.bagItems)
        {
            if (item.identifier == "trade")
            {
                GameRoundManager.Instance.AddGold(1);
            }
        }
    }
    
    public void SpendGold(int value)
    {
        gold -= value;
        goldLabel.text = gold.ToString();
        EventPool.Trigger("updateGold");
    }

    [HideInInspector]
    public float lastAnimTimer_test = 0;

    public void Init()
    {
        currentHP = maxHP;
        UpdateHP();
        
        
        BattleManager.Instance.waveText.text = "Wave: " + GameRoundManager.Instance.currentWave+"/"+GameRoundManager.Instance.maxWave;
        Next();
    }

    public void UpdateHP()
    {
        for (int i = 0; i < maxHP; i++)
        {
            if (i >= currentHP)
            {
                hpParent.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                hpParent.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void ReduceHP()
    {
        currentHP--;
        UpdateHP();

        if (currentHP <= 0)
        {
            GameLose();
        }
    }
    public bool shouldShowTutorial()
    {
        return CurrentChapter == 1 && currentWave == 1;
    }
    public void Next()
    {
        if (isFinished)
        {
            return;
        }

        if (currentWave > maxWave)
        {
            GameWin();
            return;
        }
        switch (currentState)
        {
            case StateType.start:
                AddGold(15+currentWave);
                currentState = StateType.draw;
                FindObjectOfType<DrawCardsMenu>().Show();
                break;
            case StateType.draw:
                BattleManager.Instance.StartBattle();
                FindObjectOfType<DrawCardsMenu>().Hide();
                currentState = StateType.battle;
                break;
            case StateType.battle:
                
                AddGold(15+currentWave);
                currentState = StateType.draw;
                FindObjectOfType<DrawCardsMenu>().Show();
                
                
                BattleManager.Instance.LoadBagData();
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isFinished = false;
    public void GameLose()
    {
        isFinished = true;

        FindObjectOfType<WinLoseMenu>() .ShowLose();
    }

    public void GameWin()
    {
        isFinished = true;

        FindObjectOfType<WinLoseMenu>() .ShowWin();
    }
}
