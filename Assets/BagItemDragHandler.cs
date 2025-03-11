using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas parentCanvas;
    private BagManager bagManager;  // 背包管理器引用
    private BackpackUI backpackUI;
    private BagItem bagItem;        // 当前物体的背包数据
    private GameObject detailObj;
    
    RectTransform discardArea;

    [Tooltip("每个背包格子的像素大小，应与 BackpackUI 中一致")]
    public int cellSize = 100;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        parentCanvas = GetComponentInParent<Canvas>();
        bagItem = GetComponent<BagItem>();

        bagManager = BattleManager.Instance.playerBagManager;
        backpackUI = bagManager.GetComponent<BackpackUI>();
        if (bagManager == null)
        {
            Debug.LogError("未找到 BagManager，请确保场景中存在！");
        }
        discardArea = FindObjectOfType<DrawCardsMenu>(true).discardArea;
        detailObj = FindObjectOfType<DrawCardsMenu>(true).detailObj;
    }


    private bool beginDrag = false;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameRoundManager.Instance.isBattling)
        {
            return;
        }

        if (!bagItem.isOwned && !GameRoundManager.Instance.hasEnoughGold(bagItem.cost))
        {
            return;
        }

        beginDrag = true;
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
        // 开始拖拽时从背包中移除物品（如果已经放置过）
        bagManager.TryRemoveItem(bagItem);
        
        discardArea.gameObject.SetActive(bagItem.isOwned);
    }

    private bool IsPointerOverTarget(PointerEventData eventData)
    {
        if (discardArea == null) return false;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            discardArea,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        return discardArea.rect.Contains(localPoint);
    }
    
    public void OnDrag(PointerEventData eventData)
    {

        if (!beginDrag)
        {
            return;
        }
        
        
        
        
        foreach (var slot in backpackUI.backpackPanel.GetComponentsInChildren<Image>())
        {
            slot.color = new Color(1, 1, 1, 0.2f);
        }
        // 更新拖拽位置
        rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
        
        if (IsPointerOverTarget(eventData))
        {
            discardArea.GetComponentInChildren<Image>().color = Color.green;
            discardArea.GetComponentInChildren<TMP_Text>(true).text = (bagItem.cost / 2).ToString();
            discardArea.GetComponentInChildren<TMP_Text>(true).gameObject.SetActive(true);
            return;

        }
        else
        {
            discardArea.GetComponentInChildren<Image>().color = Color.white;
            discardArea.GetComponentInChildren<TMP_Text>(true).gameObject.SetActive(false);
        }

        // 计算当前鼠标在背包面板内的局部坐标
        RectTransform bagRect = this.backpackUI.backpackPanel.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(bagRect, Input.mousePosition, eventData.pressEventCamera, out localPoint);

        localPoint.x -= cellSize / 2f;
        localPoint.y -= cellSize / 2f;
        // 根据背包面板的左下角作为原点，计算候选格子的坐标（整数）
        Vector2Int gridPos = new Vector2Int(
            Mathf.RoundToInt((localPoint.x - bagRect.rect.xMin) / cellSize),
            Mathf.RoundToInt((localPoint.y - bagRect.rect.yMin) / cellSize)
        );
        
        if (gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= bagManager.bag.width || gridPos.y >= bagManager.bag.height)
        {
            // 超出背包边界，不处理
            return;
        }
        
        //check if mouse is in discardArea
        
        
       
        

        var poses = GetComponent<BagItem>().shape;

        bool canPlaceFull = true;
        var canPlace = bagManager.bag.CanPlaceItem(bagItem, gridPos);

        if (!canPlace)
        {
            canPlaceFull = false;
        }
        

        // 根据放置合法性修改自身的颜色（假设有 Image 组件，或者你也可以修改其他显示效果）
        Image itemImage = GetComponent<Image>();
        if(itemImage != null)
        {
            itemImage.color = canPlaceFull ? Color.green : Color.red;
        }

        // 同时更新对应背包格子预览的颜色
        
        
        
        
        if (backpackUI != null)
        {
            
            foreach (var pos in poses)
            {
                
                Image slotImage = backpackUI.GetSlotAt(gridPos + pos);
                if(slotImage != null)
                {
                    slotImage.color = canPlaceFull ? Color.green : Color.red;
                }
            
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        
        
        if (!beginDrag)
        {
            return;
        }
        beginDrag = false;
        canvasGroup.blocksRaycasts = true;

        // 将屏幕点击位置转换为背包面板内的局部坐标
        RectTransform bagRect = this.backpackUI.backpackPanel.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(bagRect, eventData.position, eventData.pressEventCamera, out localPoint);

        localPoint.x -= cellSize / 2f;
        localPoint.y -= cellSize / 2f;
        // 根据背包面板的左下角作为原点，计算候选格子的坐标（整数）
        Vector2Int gridPos = new Vector2Int(
            Mathf.RoundToInt((localPoint.x - bagRect.rect.xMin) / cellSize),
            Mathf.RoundToInt((localPoint.y - bagRect.rect.yMin) / cellSize)
        );
        
        // if (gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= bagManager.bag.width || gridPos.y >= bagManager.bag.height)
        // {
        //     // 超出背包边界，不处理
        //     return;
        // }

        if (IsPointerOverTarget(eventData))
        {
            bagItem.Sell();
        }
        else
        {
            
        // 同时重置背包预览格子的颜色
        // 尝试放置物品到背包中
        bool placed = bagManager.TryPlaceItem(bagItem, gridPos);
        if (placed)
        {
            // 计算格子中心位置
            float posX = bagRect.rect.xMin + gridPos.x * cellSize;//+ cellSize * 0.5f;
            float posY = bagRect.rect.yMin + gridPos.y * cellSize;//+ cellSize * 0.5f;
            rectTransform.anchoredPosition = new Vector2(posX, posY);
            Image slotImage = backpackUI.GetSlotAt(gridPos);
            var cellSizeTest = cellSize / 2;
            
            //else
            {
                
                bagItem.Purchase();
            }
#if UNITY_EDITOR
#else
                    cellSizeTest += 25;
#endif
            
            rectTransform.position = slotImage.transform.position - new Vector3(cellSizeTest, cellSizeTest);
        }
        else
        {
            // 放置失败还原
            rectTransform.anchoredPosition = originalPosition;
        }

        // 重置自身颜色
        Image img = GetComponent<Image>();
        if(img != null)
        {
            img.color = Color.white;
        }
        
        }
        if(backpackUI != null)
        {
            Image slotImage = backpackUI.GetSlotAt(gridPos);
            if(slotImage != null)
            {
                slotImage.color = Color.white;
            }
        }

        transform.parent = backpackUI.transform;
        
        discardArea.gameObject.SetActive(false);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        detailObj.SetActive(true);
        detailObj.GetComponent<DetailMenu>().Show(bagItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        detailObj.SetActive(false);
    }
}
