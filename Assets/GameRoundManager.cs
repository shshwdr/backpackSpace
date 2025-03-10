using System.Collections;
using System.Collections.Generic;
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

    public bool isBattling => currentState == StateType.battle;
    private int gold;
    public int CurrentChapter=>currentChapter;

    private int currentChapter = 2;
    
    
    
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
    }
    public void SpendGold(int value)
    {
        gold -= value;
        goldLabel.text = gold.ToString();
    }

    [HideInInspector]
    public float lastAnimTimer_test = 0;

    public void Init()
    {
        Next();
    }

    public bool shouldShowTutorial()
    {
        return CurrentChapter == 1 && currentWave == 1;
    }
    public void Next()
    {
        switch (currentState)
        {
            case StateType.start:
                AddGold(10+currentWave);
                currentState = StateType.draw;
                FindObjectOfType<DrawCardsMenu>().Show();
                break;
            case StateType.draw:
                BattleManager.Instance.StartBattle();
                FindObjectOfType<DrawCardsMenu>().Hide();
                currentState = StateType.battle;
                break;
            case StateType.battle:
                
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

    public void GameLose()
    {
        

    }

    public void GameWin()
    {

    }
}
