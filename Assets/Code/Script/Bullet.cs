using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr; // ✅ 添加 SpriteRenderer 用于修改颜色

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;

    public Transform target;
    public TowerXP shooterXP;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetShooter(TowerXP xpScript)
    {
        shooterXP = xpScript;

        // ✅ 设置弹幕颜色为发射塔的当前颜色
        if (shooterXP != null && sr != null)
        {
            sr.color = shooterXP.GetBulletColor();
        }
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Health health = other.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }
}
