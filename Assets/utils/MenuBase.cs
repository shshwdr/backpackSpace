using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuBase : MonoBehaviour
{
    public GameObject menu;
    protected Image blockImage;
    public bool IsActive => menu.activeInHierarchy;
    public Button closeButton;
    public virtual void UpdateMenu()
    {
        
    }
    
    
    protected virtual void Awake()
    {
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        menu.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        blockImage = menu.GetComponent<Image>();
        if (blockImage)
        {
            
            var color = menu.GetComponent<Image>().color;
            color.a = 1f;
            menu.GetComponent<Image>().color = color;
        }

        if (closeButton)
        {
            closeButton.onClick.AddListener(() =>
            {
                Hide();
            });
        }
        
        targetTrans = transform.position;
        targetSizeDelta = transform.localScale.x;
    }

    public virtual void tryInteract()
    {
        
    }
    public static T FindFirstInstance<T>() where T : MenuBase
    {
        T instance = FindObjectOfType<T>();
        if (instance == null)
        {
            Debug.LogWarning($"No instance of {typeof(T).Name} found in the scene.");
        }
        return instance;
    }
    public static void OpenMenu<T>() where T : MenuBase
    {
        var instance = FindFirstInstance<T>();
        if (instance != null)
        {
            instance.Show();
        }
    }
    protected virtual void Start()
    {
        Hide();
    }

    virtual public void Init()
    {
    }

    public RectTransform animatedRect;
    protected Vector3 startTrans;
    protected Vector3 targetTrans;
    protected float targetSizeDelta;
    public float showTime = 0.5f;
    virtual public void ShowAnim(Vector3 startTrans)
    {
        
        animatedRect.position = startTrans;
        animatedRect.localScale =Vector3.zero;
        
        animatedRect.DOMove(targetTrans, showTime).SetEase( Ease.OutQuart);

        // 缩放到目标大小
        animatedRect.DOScale(targetSizeDelta, showTime).SetEase( Ease.OutQuart);
    }
    
    virtual public void Show()
    {
       

        
        
        menu.SetActive(true);
    }
    virtual public void Hide()
    {
        menu.SetActive(false);

    }
}

