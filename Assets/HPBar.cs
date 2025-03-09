using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image fill;
    public TMP_Text text;
    
    public  void SetHP(float hp, float maxHP)
    {
        fill.fillAmount = hp / maxHP;
        if (text)
        {
            text.text = (int)hp + "/" + (int)maxHP;
        }
    }

    public void SetHP(float hp, float maxHP, float scale, float offset)
    {
        fill.fillAmount = (hp / maxHP)*scale+offset;
        if (text)
        {
            text.text = (int)hp + "/" + (int)maxHP;
        }
    }
}