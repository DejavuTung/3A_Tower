using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    
    void Start()
    {
        // 如果血量还没有设置，才设置为最大值
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
        }
    }
    
    public bool TakeDamage(float damage)
    {
        Debug.Log($"EnemyHealth.TakeDamage被调用 - 当前血量: {currentHealth}, 最大血量: {maxHealth}, 伤害: {damage}");
        
        currentHealth -= damage;
        
        // 确保血量不会变成负数
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        
        Debug.Log($"EnemyHealth.TakeDamage执行后 - 当前血量: {currentHealth}, 最大血量: {maxHealth}");
        
        // 更新血条
        UpdateSimpleHealthBar();
        
        if (currentHealth <= 0)
        {
            Debug.Log($"敌人死亡 - 当前血量: {currentHealth}");
            // 销毁血条
            if (healthBarObject != null)
            {
                Destroy(healthBarObject);
            }
            Destroy(gameObject);
            return true; // 敌人死亡
        }
        
        Debug.Log($"敌人存活 - 当前血量: {currentHealth}");
        return false; // 敌人存活
    }
    
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
    
    public GameObject healthBarObject;
    public SpriteRenderer healthBarRenderer;
    

    
    void UpdateSimpleHealthBar()
    {
        if (healthBarObject != null && healthBarRenderer != null)
        {
            // 更新血条宽度
            float healthPercent = GetHealthPercentage();
            Vector3 scale = healthBarObject.transform.localScale;
            scale.x = 1f * healthPercent;
            scale.y = 0.2f;
            scale.z = 1f;
            healthBarObject.transform.localScale = scale;
        }
    }
} 