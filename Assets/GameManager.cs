using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private Dictionary<int, List<BagData>> groupedData;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        groupedData = BagSaver.LoadBagDataGroupedByWave();
        CSVLoader.Instance.Init();
        BattleManager.Instance.Init();
        StartCoroutine(test());
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(0.1f);
        
        BattleManager.Instance.LoadBagData();
        GameRoundManager.Instance.Init();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
