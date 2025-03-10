using System;
using UnityEngine;
using DG.Tweening;

public class AttackModule : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float attackInterval = 1f;
    public float bulletSpeed = 5f;
    public int bulletDamage = 1;
    public bool isFriendly = true; // 我方为 true，敌方为 false

    private float timer;

    private void Awake()
    {
        if (GetComponentInParent<BagManager>())
        {
            isFriendly =  GetComponentInParent<BagManager>().isFriendly;
            
        }
    }
    
    void Update()
    {
        if (GameRoundManager.Instance.isBattling)
        {
            BattleUpdate(Time.deltaTime);
        }
    }

    void BattleUpdate(float time)
    {
        timer += time;
        if (timer >= attackInterval)
        {
            timer = 0f;
            Fire();
        }
    }

    void Fire()
    {
        // 从武器位置实例化子弹
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity,BattleManager.Instance.bulletParent);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            // 我方向右，敌方向左
            Vector2 dir = isFriendly ? Vector2.right : Vector2.left;
            bullet.Initialize(dir, bulletSpeed, bulletDamage, isFriendly);
        }
    }
}