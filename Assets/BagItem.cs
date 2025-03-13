using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;

/// <summary>
/// 背包中的物体，定义了该物体在背包中占用的格子。
/// 例如，shape 中可以定义 [(0,0), (1,0), (0,1)] 表示物体占用一个 L 形区域（锚点为左上角或其他约定位置）。
/// </summary>
public class BagItem : MonoBehaviour
{
    [Tooltip("物体在背包中占用的格子偏移，相对于锚点 (0,0)")]
    public List<Vector2Int> shape = new List<Vector2Int>();

    [HideInInspector]
    public Vector2Int gridPosition = new Vector2Int(-1, -1);
    
    public string identifier;
    
    public int maxHP = 5;
    public int currentHP;
    public int cost = 4;

    private HPBar hpbar;
    public bool isDead = false;
    private BagManager bagManager;
    public bool isOwned = true;
    public GameObject costLabel;

    public bool isGenerated = false;

    private ItemInfo info;

    void Awake()
    {
        info = CSVLoader.Instance.ItemInfoDict[identifier];
        maxHP = info.hp;
        currentHP = maxHP;
        hpbar = GetComponentInChildren<HPBar>();
        bagManager = GetComponentInParent<BagManager>();
        if (bagManager == null)
        {
            bagManager = BattleManager.Instance.playerBagManager;
        }
        EventPool.OptIn("updateGold", UpdateGold);
    }

    void UpdateGold()
    {
        costLabel.GetComponentInChildren<TMP_Text>().color = GameRoundManager.Instance.hasEnoughGold(cost) ? Color.white : Color.red;
    }
    public void PlaceInShop()
    {
        isOwned = false;
        costLabel.SetActive(true);
        costLabel.GetComponentInChildren<TMP_Text>().text = cost.ToString();
        UpdateGold();
    }

    public void Sell()
    {
        if (!isOwned)
        {
            return;
        }
        GameRoundManager.Instance.AddGold(cost/2);
        GameRoundManager.Instance.DoTrade();
    }


    public void Purchase()
    {
        if (isOwned)
        {
            return;
        }
        
        costLabel.SetActive(false);
        
        GameRoundManager.Instance.SpendGold(cost);
        isOwned = true;
        
        GameRoundManager.Instance.DoTrade();
    }
    public bool hpNotFull()
    {
        return currentHP < maxHP;
    }

    public void Heal(int value)
    {
         if (value <= 0)
         {
             return;
         }
         //if (hpNotFull())
         {
             currentHP += value;
             currentHP = Mathf.Clamp(currentHP, 0, maxHP);
             hpbar.SetHP(currentHP,maxHP);
         }
    }
    public void TakeDamage(int dmg)
    {
        if (dmg <= 0)
        {
            return;
        }
        if (identifier != "bear")
        {
            foreach (var item in bagManager.bagItems)
            {
                if (!item.isDead && item.identifier == "bear")
                {
                    item.TakeDamage(1);
                    dmg--;
                    if (dmg <= 0)
                    {
                        break;
                    }
                }
            }

        }
        
        
        
        currentHP -= dmg;
        Debug.Log(gameObject.name + " takes " + dmg + " damage. HP: " + currentHP);
        if (currentHP <= 0)
        {
            die();
        }
        hpbar.SetHP(currentHP,maxHP);
    }

    void die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        bagManager.deadCount++;
        
        if (GetComponent<DeadActionBase>())
        {
            GetComponent<DeadActionBase>().DeadAction();
        }
            
        RemoveFromBattlefield();

        if (bagManager == BattleManager.Instance.enemyBagManager)
        {
            //var walleCount = 0;
            foreach (var item in BattleManager.Instance.playerBagManager.bagItems)
            {
                if (item.identifier == "collect")
                {
                    GameRoundManager.Instance.AddGold(1);
                }
            }
        }
    }

    void RemoveFromBattlefield()
    {
        Debug.Log(gameObject.name + " is removed from battlefield.");
        // 可以添加动画、特效、延时移除等效果
        gameObject.SetActive(false);
        isDead = true;
        if (bagManager.aliveBagItems.Contains(this))
        {
            bagManager.aliveBagItems.Remove(this);
        }
    }
    
    public void Reset()
    {
        isDead = false;
        gameObject.SetActive(true);
        currentHP = maxHP;
        hpbar.SetHP(currentHP,maxHP);
    }
}