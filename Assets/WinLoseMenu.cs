using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseMenu : MenuBase
{
    public TMP_Text text;

    public Button restartButton;

    override protected void Start()
    {
        base.Start();
        restartButton.onClick.AddListener(() => { GameManager.Instance.Restart(); });
    }
    
    public void ShowWin()
    {
        Show();

        text.text = " You Win!";
        Captain.Instance.SetHappy();
    }
    
    public void ShowLose()
    {
        Show();

        text.text = " You Lose!";
        Captain.Instance.SetSad();
    }
}
