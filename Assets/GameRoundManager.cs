using System.Collections;
using System.Collections.Generic;
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

    private int currentArea = 1;

    public int CurrentWave => currentWave;
    public int LastWave=> currentWave-1;
    private int currentWave = 1;
    
    public int TotalWaves;
    public int Gold => gold;
    private StateType currentState = StateType.start;
    

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
                currentState = StateType.draw;
                FindObjectOfType<DrawCardsMenu>().Show();
                break;
            case StateType.draw:
                BattleManager.Instance.StartBattle();
                FindObjectOfType<DrawCardsMenu>().Hide();
                break;
            case StateType.battle:
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
