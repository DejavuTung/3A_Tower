using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("移动设置")]
    public float speed = 0.5f; // 敌人移动速度
    
    [Header("停顿管理")]
    public bool isStunned = false; // 是否正在停顿
    public float stunEndTime = 0f; // 停顿结束时间
    
    // 可以在这里添加更多移动相关的功能
    // 比如加速度、减速、特殊移动模式等
    
    void Update()
    {
        // 检查停顿是否应该结束
        if (isStunned && Time.time >= stunEndTime)
        {
            EndStun();
        }
    }
    
    public void StartStun(float stunDuration)
    {
        isStunned = true;
        stunEndTime = Time.time + stunDuration;
        speed = 0f;
        Debug.Log($"敌人开始停顿，持续时间: {stunDuration}秒");
    }
    
    public void EndStun()
    {
        if (isStunned)
        {
            isStunned = false;
            speed = 0.5f; // 恢复默认速度
            Debug.Log($"敌人停顿结束，恢复移动");
        }
    }
    
    // 获取敌人当前速度向量
    public Vector3 GetCurrentVelocity()
    {
        if (isStunned)
        {
            return Vector3.zero; // 停顿时不移动
        }
        
        // 计算朝向基地的方向
        GameObject baseObj = GameObject.Find("Base");
        if (baseObj != null)
        {
            Vector3 directionToBase = (baseObj.transform.position - transform.position).normalized;
            return directionToBase * speed;
        }
        
        // 如果找不到基地，返回零向量
        return Vector3.zero;
    }
} 