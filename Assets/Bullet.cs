using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private int damage;
    private bool isFriendly;
    public float lifeTime = 5f;
    private float timer;
    public ItemInfo info;

    // 为检测目标，我们需要引用对方的背包管理器和主物品区域
    // 假设在场景中存在 BattleManager，可以提供这些引用
    private BagManager targetBagManager;
    private RectTransform targetMainItemRect;

    public void Initialize(Vector2 dir, float spd, int dmg, bool friendly, ItemInfo info)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;
        isFriendly = friendly;
        this.info = info;
        // 根据友军/敌方选择目标背包，假设 BattleManager 有对应方法
        if (isFriendly)
        {
            targetBagManager = BattleManager.Instance.enemyBagManager;
            targetMainItemRect = BattleManager.Instance.enemyMainItemRect;
        }
        else
        {
            targetBagManager = BattleManager.Instance.playerBagManager;
            targetMainItemRect = BattleManager.Instance.friendMainItemRect;
            GetComponentInChildren<Image>().transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Update()
    {
        if (!BattleManager.Instance.isBattling)
        {
            return;
        }
        // 移动子弹
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        // 假设 targetBagManager 是敌方或我方的 BagManager，其背包面板是一个 RectTransform
        if (targetBagManager != null)
        {
            // 获取背包面板的 RectTransform
            RectTransform bagRect = targetBagManager.GetComponent<RectTransform>();
            // 获取子弹的 RectTransform
            RectTransform bulletRect = GetComponent<RectTransform>();

            // 使用 Canvas 的摄像机（这里假设为 Camera.main），获取子弹在屏幕中的位置
            Vector2 bulletScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, bulletRect.position);

            // 将子弹的屏幕坐标转换为背包面板内的局部坐标，注意传入摄像机
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(bagRect, bulletScreenPos, Camera.main, out localPoint);

            float cellSize = targetBagManager.GetComponent<BackpackUI>().cellSize.x;
            // 如果背包面板的 pivot 不是左下角，则需要补偿（此处假设 pivot 为 (0,0)）
            localPoint.x -= cellSize / 2f;
            localPoint.y -= cellSize / 2f;

            // 根据背包面板的左下角作为原点，计算候选格子的坐标（整数）
            float cellSizeVal = targetBagManager.GetComponent<BackpackUI>().cellSize.x;
            Vector2Int gridPos = new Vector2Int(
                Mathf.RoundToInt((localPoint.x - bagRect.rect.xMin) / cellSizeVal),
                Mathf.RoundToInt((localPoint.y - bagRect.rect.yMin) / cellSizeVal)
            );

            // 检查目标背包中是否有物体占用该格子
            BagItem hitItem = targetBagManager.GetItemAtGridPosition(gridPos);
            if (hitItem != null && !hitItem.isDead)
            {
                hitItem.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }


        // 检测是否进入目标主物品区域（MainItem）
        if (targetMainItemRect != null)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            if (RectTransformUtility.RectangleContainsScreenPoint(targetMainItemRect, screenPos, Camera.main))
            {
                // 进入主物品区域，对目标总HP造成伤害
                if (isFriendly)
                    BattleManager.Instance.ApplyDamageToEnemy(damage);
                else
                    BattleManager.Instance.ApplyDamageToFriendly(damage);
                Destroy(gameObject);
                return;
            }
        }
    }
}
