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
        currentHealth -= damage;
        
        // 更新血条
        UpdateSimpleHealthBar();
        
        if (currentHealth <= 0)
        {
            // 销毁血条
            if (healthBarObject != null)
            {
                Destroy(healthBarObject);
            }
            Destroy(gameObject);
            return true; // 敌人死亡
        }
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