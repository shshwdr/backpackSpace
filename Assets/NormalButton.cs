using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalButton : MonoBehaviour
{
    private Button targetButton;
    void Awake()
    {
        // 获取或添加 AudioSource 组件
        targetButton = GetComponent<Button>();
    }

    void Start()
    {
        // 为按钮添加一个监听器，确保无论原有 onClick 如何定义，点击时都会播放音效
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(PlayClickSound);
        }
    }

    private void PlayClickSound()
    {
        //SfxManager.Instance.PlaySFX("Click");
    }
}
