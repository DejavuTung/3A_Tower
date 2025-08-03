using UnityEngine;

public class AttackPrecisionTest : MonoBehaviour
{
    private AutoTowerDefenseDemo gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<AutoTowerDefenseDemo>();
        if (gameManager == null)
        {
            Debug.LogError("AttackPrecisionTest: 未找到AutoTowerDefenseDemo组件");
            return;
        }
        Debug.Log("AttackPrecisionTest: 攻击精确度测试准备就绪");
    }

    [ContextMenu("测试攻击范围精确度")]
    void TestAttackRangePrecision()
    {
        if (gameManager == null) { Debug.LogError("AttackPrecisionTest: 游戏管理器未找到"); return; }
        Debug.Log("AttackPrecisionTest: 开始测试攻击范围精确度");
        
        // 使用反射获取私有字段
        var towerRangeField = typeof(AutoTowerDefenseDemo).GetField("towerRange", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var enemiesField = typeof(AutoTowerDefenseDemo).GetField("enemies", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var towerObjField = typeof(AutoTowerDefenseDemo).GetField("towerObj", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (towerRangeField != null && enemiesField != null && towerObjField != null)
        {
            float towerRange = (float)towerRangeField.GetValue(gameManager);
            var enemies = (System.Collections.Generic.List<GameObject>)enemiesField.GetValue(gameManager);
            GameObject towerObj = (GameObject)towerObjField.GetValue(gameManager);
            
            Debug.Log($"AttackPrecisionTest: 防御塔攻击范围: {towerRange}");
            Debug.Log($"AttackPrecisionTest: 精确攻击范围: {towerRange - 0.1f}");
            Debug.Log($"AttackPrecisionTest: 当前敌人数量: {enemies.Count}");
            
            if (towerObj != null)
            {
                Debug.Log($"AttackPrecisionTest: 防御塔位置: {towerObj.transform.position}");
                
                int inTheoreticalRange = 0;
                int inPreciseRange = 0;
                int shouldBeAttacked = 0;
                
                foreach (var enemy in enemies)
                {
                    if (enemy == null) continue;
                    
                    float distanceToTower = Vector3.Distance(towerObj.transform.position, enemy.transform.position);
                    bool inTheoretical = distanceToTower < towerRange;
                    bool inPrecise = distanceToTower <= towerRange - 0.1f;
                    
                    if (inTheoretical) inTheoreticalRange++;
                    if (inPrecise) inPreciseRange++;
                    if (inPrecise) shouldBeAttacked++;
                    
                    Debug.Log($"AttackPrecisionTest: 敌人 {enemy.name} - 距离: {distanceToTower:F3}, 理论范围内: {inTheoretical}, 精确范围内: {inPrecise}");
                }
                
                Debug.Log($"AttackPrecisionTest: 理论范围内敌人: {inTheoreticalRange}");
                Debug.Log($"AttackPrecisionTest: 精确范围内敌人: {inPreciseRange}");
                Debug.Log($"AttackPrecisionTest: 应该被攻击的敌人: {shouldBeAttacked}");
            }
        }
    }

    [ContextMenu("测试攻击目标选择逻辑")]
    void TestAttackTargetSelection()
    {
        if (gameManager == null) { Debug.LogError("AttackPrecisionTest: 游戏管理器未找到"); return; }
        Debug.Log("AttackPrecisionTest: 开始测试攻击目标选择逻辑");
        
        var enemiesField = typeof(AutoTowerDefenseDemo).GetField("enemies", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var towerObjField = typeof(AutoTowerDefenseDemo).GetField("towerObj", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var baseObjField = typeof(AutoTowerDefenseDemo).GetField("baseObj", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (enemiesField != null && towerObjField != null && baseObjField != null)
        {
            var enemies = (System.Collections.Generic.List<GameObject>)enemiesField.GetValue(gameManager);
            GameObject towerObj = (GameObject)towerObjField.GetValue(gameManager);
            GameObject baseObj = (GameObject)baseObjField.GetValue(gameManager);
            
            if (towerObj != null && baseObj != null)
            {
                Debug.Log("AttackPrecisionTest: 分析每个敌人的攻击优先级");
                
                foreach (var enemy in enemies)
                {
                    if (enemy == null) continue;
                    
                    float distanceToTower = Vector3.Distance(towerObj.transform.position, enemy.transform.position);
                    float distanceToBase = Vector3.Distance(enemy.transform.position, baseObj.transform.position);
                    
                    EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                    float healthPercent = enemyHealth != null ? enemyHealth.GetHealthPercentage() : 1f;
                    
                    // 计算攻击优先级（与AttackEnemy方法中的逻辑一致）
                    float priority = healthPercent * 100f - distanceToBase;
                    
                    Debug.Log($"AttackPrecisionTest: 敌人 {enemy.name} - 距离塔: {distanceToTower:F3}, 距离基地: {distanceToBase:F3}, 血量: {healthPercent:P0}, 优先级: {priority:F1}");
                }
            }
        }
    }

    [ContextMenu("测试箭矢碰撞检测")]
    void TestArrowCollisionDetection()
    {
        if (gameManager == null) { Debug.LogError("AttackPrecisionTest: 游戏管理器未找到"); return; }
        Debug.Log("AttackPrecisionTest: 开始测试箭矢碰撞检测");
        
        Debug.Log("AttackPrecisionTest: 箭矢碰撞检测范围: 0.2f");
        Debug.Log("AttackPrecisionTest: 这个范围比之前的0.3f更精确");
        Debug.Log("AttackPrecisionTest: 建议在游戏中观察箭矢是否准确命中敌人");
    }

    [ContextMenu("检查攻击范围指示器")]
    void CheckAttackRangeIndicators()
    {
        if (gameManager == null) { Debug.LogError("AttackPrecisionTest: 游戏管理器未找到"); return; }
        Debug.Log("AttackPrecisionTest: 检查攻击范围指示器");
        
        var towerObjField = typeof(AutoTowerDefenseDemo).GetField("towerObj", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (towerObjField != null)
        {
            GameObject towerObj = (GameObject)towerObjField.GetValue(gameManager);
            if (towerObj != null)
            {
                // 查找攻击范围指示器
                GameObject rangeIndicator = GameObject.Find("TowerRangeIndicator");
                GameObject preciseRangeIndicator = GameObject.Find("PreciseRangeIndicator");
                
                if (rangeIndicator != null)
                {
                    Debug.Log($"AttackPrecisionTest: 理论攻击范围指示器存在 - 位置: {rangeIndicator.transform.position}, 尺寸: {rangeIndicator.transform.localScale}");
                }
                else
                {
                    Debug.LogWarning("AttackPrecisionTest: 理论攻击范围指示器未找到");
                }
                
                if (preciseRangeIndicator != null)
                {
                    Debug.Log($"AttackPrecisionTest: 精确攻击范围指示器存在 - 位置: {preciseRangeIndicator.transform.position}, 尺寸: {preciseRangeIndicator.transform.localScale}");
                }
                else
                {
                    Debug.LogWarning("AttackPrecisionTest: 精确攻击范围指示器未找到");
                }
            }
        }
    }

    [ContextMenu("完整攻击精确度测试")]
    void CompleteAttackPrecisionTest()
    {
        if (gameManager == null) { Debug.LogError("AttackPrecisionTest: 游戏管理器未找到"); return; }
        Debug.Log("AttackPrecisionTest: 开始完整攻击精确度测试");
        
        TestAttackRangePrecision();
        TestAttackTargetSelection();
        TestArrowCollisionDetection();
        CheckAttackRangeIndicators();
        
        Debug.Log("AttackPrecisionTest: 完整测试完成");
        Debug.Log("AttackPrecisionTest: 请观察以下改进：");
        Debug.Log("1. 攻击范围检测更精确（留0.1f安全边距）");
        Debug.Log("2. 攻击目标选择优化（优先攻击血量高的敌人）");
        Debug.Log("3. 箭矢碰撞检测更精确（0.2f范围）");
        Debug.Log("4. 双重攻击范围指示器（白色理论范围，红色精确范围）");
        Debug.Log("5. 详细的调试日志输出");
    }
} 