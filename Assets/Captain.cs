using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Captain : Singleton<Captain>
{
    public Sprite happySprite;
    public Sprite sadSprite;
    public Sprite normalSprite;
    public Sprite smileSprite;
    
    public Image image;
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
