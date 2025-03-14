using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Captain : Singleton<Captain>
{
    public Sprite happySprite;
    public Sprite sadSprite;
    public Sprite normalSprite;
    public Sprite smileSprite;
    
    public Image image;


    public GameObject dialogue;

    public void HideDialogue()
    {
        dialogue.SetActive(false);
    }
    public void ShowDialogue(string text,int face)
    {
        dialogue.SetActive(true);
        dialogue.GetComponentInChildren<TMP_Text>().text = text;
        switch (face)
        {
            case 0:
                SetHappy();
                break;
            case 1:
                SetSmile();
                break;
            case 2:
                SetNormal();
                break;
            case 3:
                SetSad();
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
    
    public void SetHappy()
    {
        image.sprite = happySprite;
    }
    public void SetSad()
    {
        image.sprite = sadSprite;
    }
    public void SetNormal()
    {
        image.sprite = normalSprite;
    }
    public void SetSmile()
    {
        image.sprite = smileSprite;
    }
}
