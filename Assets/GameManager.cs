using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private Dictionary<int, List<BagData>> groupedData;
    public GameObject startButton;
    public GameObject refreshButton;
    
    // Start is called before the first frame update
    void Awake()
    {
        CSVLoader.Instance.Init();
        BattleManager.Instance.Init();
        StartCoroutine(test());
    }

    IEnumerator test()
    {
        var discardArea = FindObjectOfType<DrawCardsMenu>(true).discardArea;
        discardArea.gameObject.SetActive(false);
        if (PlayerPrefs.HasKey("tutorial"))
        {
            
            yield return new WaitForSeconds(0.1f);
            groupedData = BagSaver.LoadBagDataGroupedByWave();
            BattleManager.Instance.LoadBagData();
            GameRoundManager.Instance.Init();
        }
        else
        {
            startButton.SetActive(false);
        refreshButton.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        
        Captain.Instance.ShowDialogue("Good morning, Captain! You're finally awake!", 0);
        
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        
        yield return new WaitForSeconds(0.2f);
        Captain.Instance.ShowDialogue("Who am I? Don't you remember? It must be from that hit we took in our last fight...", 2);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        
        groupedData = BagSaver.LoadBagDataGroupedByWave();
        BattleManager.Instance.LoadBagData();
        Captain.Instance.ShowDialogue("Oh no! The enemy is coming! No time for explanations—get ready to fight!", 3);
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        
        GameRoundManager.Instance.Init();
        Captain.Instance.ShowDialogue("Don't worry! It's not that hard. Just drag your spaceships to the left area - your ship area.", 2);
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        Captain.Instance.ShowDialogue("If you drag one ship onto another, they will merge and upgrade.", 2);
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        
        refreshButton.SetActive(true);
        Captain.Instance.ShowDialogue("Click the refresh button if no spaceship meets your requirements.", 1);
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        startButton.SetActive(true);
        Captain.Instance.ShowDialogue("And press 'Start' when you're ready.", 1);
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        Captain.Instance.ShowDialogue("They will automatically fight the enemy on the right. Try it!", 0);
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        Captain.Instance.HideDialogue();
        
        PlayerPrefs.SetInt("tutorial",1);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
