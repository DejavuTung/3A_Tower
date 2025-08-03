using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AutoTowerDefenseDemo : MonoBehaviour
{
    [Header("图像资源")]
    public Sprite baseSprite;        // 基地图像
    public Sprite towerSprite;       // 防御塔图像
    public Sprite enemySprite;       // 敌人图像
    public Sprite backgroundMap;     // 背景地图
    
    [Header("动画资源")]
    public Sprite[] enemyWalkSprites;  // 敌人行走动画帧
    
    [Header("Archer资源")]
    public Sprite archerRest1_1;       // Archer休息状态图片1-1
    public Sprite archerRest1_2;       // Archer休息状态图片1-2
    public Sprite archerRest2_1;       // Archer休息状态图片2-1
    public Sprite archerRest2_2;       // Archer休息状态图片2-2
    
    [Header("Archer射击动画资源")]
    public Sprite archerShoot1_1;      // Archer射击动画1-1 (右下角)
    public Sprite archerShoot1_2;      // Archer射击动画1-2 (右下角)
    public Sprite archerShoot1_3;      // Archer射击动画1-3 (右下角)
    public Sprite archerShoot1_4;      // Archer射击动画1-4 (右下角)
    public Sprite archerShoot2_1;      // Archer射击动画2-1 (右上角)
    public Sprite archerShoot2_2;      // Archer射击动画2-2 (右上角)
    public Sprite archerShoot2_3;      // Archer射击动画2-3 (右上角)
    public Sprite archerShoot2_4;      // Archer射击动画2-4 (右上角)
    
         // 游戏数据
     int gold = 0;
     int wave = 1;
     int baseHealth = 10;
     int enemiesPerWave = 5;
     float enemySpeed = 0.5f;
     float waveDuration = 5.0f; // 每波持续5秒
     float waveDurationIncrease = 0.5f; // 每波增加0.5秒
     float towerRange = 1.8f; // 缩小为之前的60%
     float towerCooldown = 0.5f; // 初始0.5秒攻击1次
     float towerDamage = 1f; // 防御塔攻击力
     float nextSpawnTime = 0f;
     int enemiesSpawned = 0;
     float waveStartTime = 0f;
     bool isWaveBreak = false;
     Text waveBreakText;
     float nextAttackTime = 0f; // 下次攻击时间
     
           // 游戏速度控制
      float gameSpeed = 1f; // 默认1倍速
      Button speed05xButton, speed1xButton, speed2xButton, speed3xButton, speed4xButton;
      Text speed05xText, speed1xText, speed2xText, speed3xText, speed4xText;
      
      // 游戏状态控制
      bool gameStarted = false; // 游戏是否已开始
      Button startGameButton;
    
    // 新增：第一波提示状态
    bool firstWaveTextShown = false; // 第一波提示是否已显示
    bool firstWaveTextDestroyed = false; // 第一波提示是否已消失

    // 游戏对象
    GameObject baseObj;
    GameObject towerObj;
    List<GameObject> enemies = new List<GameObject>();

    // UI
    Text topRightText;

    [Header("UI设置")]
    public float uiScaleFactor = 1.0f; // UI缩放因子
    public float minUIScale = 0.8f; // 最小UI缩放
    public float maxUIScale = 1.5f; // 最大UI缩放
    
    // 响应式UI尺寸
    private float screenWidth;
    private float screenHeight;
    private float uiScale;
    
    // UI元素尺寸配置
    private float buttonHeight;
    private float buttonWidth;
    private float textFontSize;
    private float infoPanelHeight;
    private float speedButtonWidth;
    private float speedButtonHeight;

         void Start()
     {
         // 检测屏幕分辨率并计算UI缩放
         CalculateUIScale();
         
         // 创建UI
         CreateUI();
         
         Debug.Log($"游戏启动 - 屏幕分辨率: {screenWidth}x{screenHeight}, UI缩放: {uiScale:F2}");
     }

         void Update()
     {
         // 如果游戏未开始，不执行游戏逻辑
         if (!gameStarted)
         {
             return;
         }
         
         // 如果基地血量为0，游戏结束，停止所有逻辑
         if (baseHealth <= 0)
         {
             return;
         }
        
                 // 波次间休息时间
         if (isWaveBreak)
         {
             if (Time.time >= waveStartTime + (3f / gameSpeed))
             {
                 isWaveBreak = false;
                 if (waveBreakText != null)
                 {
                     // 销毁整个Canvas对象（包含背景和文本）
                     Destroy(waveBreakText.transform.parent.gameObject);
                     waveBreakText = null;
                 }
                 NextWave(); // 开始下一波
                 Debug.Log($"第{wave}波开始！");
             }
             return; // 休息期间不生成敌人
         }
        
                         // 敌人生成 - 在5秒内均匀生成
        if (enemiesSpawned < enemiesPerWave && baseHealth > 0)
        {
            // 第一波特殊处理：等待提示消失后再开始生成敌人
            if (wave == 1 && firstWaveTextShown && !firstWaveTextDestroyed)
            {
                return; // 第一波提示还在显示，不生成敌人
            }
            
            // 计算当前波次应该生成的敌人数量（考虑游戏速度）
            float adjustedWaveDuration = waveDuration / gameSpeed;
            float waveProgress = (Time.time - nextSpawnTime) / adjustedWaveDuration;
            int shouldHaveSpawned = Mathf.FloorToInt(waveProgress * enemiesPerWave);
            
            // 如果应该生成的敌人数量大于已生成的，则生成新敌人
            while (enemiesSpawned < shouldHaveSpawned && enemiesSpawned < enemiesPerWave)
            {
                SpawnEnemy();
                enemiesSpawned++;
            }
        }
        // 敌人移动
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null) continue;
            
            // 检查是否接近防御塔
            float distanceToTower = Vector3.Distance(enemies[i].transform.position, towerObj.transform.position);
            Vector3 targetPosition = baseObj.transform.position;
            
            // 如果敌人接近防御塔，计算绕道路径
            if (distanceToTower < 2f)
            {
                // 计算绕道点
                Vector3 towerPos = towerObj.transform.position;
                Vector3 basePos = baseObj.transform.position;
                Vector3 enemyPos = enemies[i].transform.position;
                
                // 计算从敌人到基地的方向
                Vector3 directionToBase = (basePos - enemyPos).normalized;
                
                // 计算从防御塔到敌人的方向
                Vector3 directionFromTower = (enemyPos - towerPos).normalized;
                
                // 计算绕道点（在防御塔旁边）
                Vector3 detourPoint = towerPos + directionFromTower * 1.5f;
                
                // 如果敌人还没到绕道点，先移动到绕道点
                if (Vector3.Distance(enemies[i].transform.position, detourPoint) > 0.5f)
                {
                    targetPosition = detourPoint;
                }
                else
                {
                    // 如果已经到达绕道点，继续向基地移动
                    targetPosition = basePos;
                }
            }
            
            // 计算移动方向
            Vector3 moveDirection = (targetPosition - enemies[i].transform.position).normalized;
            
            // 检查与其他敌人的碰撞
            Vector3 adjustedMoveDirection = AvoidEnemyCollisions(enemies[i], moveDirection);
            
                         // 移动敌人（使用每个敌人的独立速度）
             EnemyMovement enemyMovement = enemies[i].GetComponent<EnemyMovement>();
             float currentSpeed = enemySpeed; // 默认使用全局速度
             
             if (enemyMovement != null)
             {
                 currentSpeed = enemyMovement.speed;
                 
                 // 如果敌人正在停顿中，速度为0
                 if (enemyMovement.isStunned)
                 {
                     currentSpeed = 0f;
                 }
                 // 安全检查：如果速度为0但不在停顿中，恢复速度
                 else if (currentSpeed <= 0f)
                 {
                     enemyMovement.speed = enemySpeed;
                     currentSpeed = enemySpeed;
                     Debug.LogWarning($"检测到敌人速度为0但不在停顿中，强制恢复速度: {currentSpeed}");
                 }
             }
             
             Vector3 newPosition = enemies[i].transform.position + adjustedMoveDirection * currentSpeed * Time.deltaTime * gameSpeed;
             enemies[i].transform.position = newPosition;
            
            if (Vector3.Distance(enemies[i].transform.position, baseObj.transform.position) < 0.2f)
            {
                Destroy(enemies[i]);
                enemies.RemoveAt(i);
                baseHealth--;
                UpdateUI(); // 更新敌人数量和基地血量
                if (baseHealth <= 0) 
                {
                    GameOver();
                    return; // 游戏结束后立即退出Update
                }
            }
        }
        // 塔攻击 - 使用时间控制攻击频率
        if (baseHealth > 0 && Time.time >= nextAttackTime && towerObj != null)
        {
            AttackEnemy();
            nextAttackTime = Time.time + (towerCooldown / gameSpeed);
        }
        else if (towerObj == null)
        {
            Debug.LogError("防御塔对象丢失！");
        }
                 // 波次结束 - 5秒后或所有敌人被消灭后
         if ((enemiesSpawned == enemiesPerWave && enemies.Count == 0) || 
             (Time.time >= nextSpawnTime + (waveDuration / gameSpeed) && enemies.Count == 0))
         {
             if (baseHealth > 0)
             {
                 StartWaveBreak();
             }
         }
    }

    void CreateBase()
    {
        baseObj = CreateGameObject(new Vector3(5f, 0, 0), baseSprite, Color.blue, "Base");
        // 缩小基地尺寸到1/2
        baseObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    void CreateTower()
    {
        towerObj = CreateGameObject(new Vector3(2, 0, 0), towerSprite, Color.green, "Tower");
        // 缩小防御塔尺寸到1/2
        towerObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        
        // 添加Archer组件
        CreateArcherOnTower();
        
        // 添加调试信息
        Debug.Log($"防御塔创建 - 位置: {towerObj.transform.position}, 尺寸: {towerObj.transform.localScale}");
        if (towerSprite == null)
        {
            Debug.LogWarning("防御塔图像未分配，使用绿色方块");
        }
        else
        {
            Debug.Log("防御塔图像已分配");
        }
        
        // 攻击范围显示已删除
    }

         void SpawnEnemy()
     {
         // 生成单个敌人
         Vector3 spawnPos = GetNonOverlappingSpawnPosition();
         GameObject enemy = CreateGameObject(spawnPos, enemySprite, Color.red, "Enemy");
         
         // 确保敌人显示原始颜色
         SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
         if (sr != null && enemySprite != null)
         {
             sr.color = Color.white; // 保持图像原始颜色
         }
         
         // 添加血量组件
         EnemyHealth enemyHealth = enemy.AddComponent<EnemyHealth>();
         enemyHealth.maxHealth = wave; // 第1波1点血量，第2波2点血量，以此类推
         enemyHealth.currentHealth = wave;
         Debug.Log($"生成敌人，波次：{wave}，血量：{enemyHealth.currentHealth}/{enemyHealth.maxHealth}，攻击力：{towerDamage}");
         
         // 添加敌人移动速度组件
         EnemyMovement enemyMovement = enemy.AddComponent<EnemyMovement>();
         enemyMovement.speed = enemySpeed;
         
         // 直接创建血条
         CreateEnemyHealthBar(enemy);
         
         // 添加动画组件
         if (enemyWalkSprites != null && enemyWalkSprites.Length > 0)
         {
             try
             {
                 SimpleAnimation anim = enemy.AddComponent<SimpleAnimation>();
                 anim.SetAnimationFrames(enemyWalkSprites);
             }
             catch (System.Exception e)
             {
                 Debug.LogError("添加动画失败 - " + e.Message);
             }
         }
         
         enemies.Add(enemy);
         Debug.Log($"敌人生成 - 当前敌人数量: {enemies.Count}");
         UpdateUI(); // 更新敌人数量显示
     }
    
    // 获取不重叠的生成位置
    Vector3 GetNonOverlappingSpawnPosition()
    {
        Vector3 spawnPos;
        int maxAttempts = 50; // 最大尝试次数
        int attempts = 0;
        
        do
        {
            // 从背景图菱形黄色小道的左边1/4部分随机生成
            // 根据背景图的菱形路径，左边1/4部分大约在X: -8f到-6f，Y: -2f到2f的范围内
            // 进一步优化：确保在菱形路径的左边1/4部分生成
            float xPos = Random.Range(-8f, -6f);
            float yPos = Random.Range(-2f, 2f);
            
            // 确保生成位置在菱形路径上（通过限制Y坐标范围）
            // 菱形路径的左边部分，Y坐标应该在一个更窄的范围内
            if (xPos > -7f)
            {
                // 如果X坐标更靠近中心，Y坐标范围应该更窄
                yPos = Random.Range(-1.5f, 1.5f);
            }
            else
            {
                // 如果X坐标更靠左，Y坐标范围可以稍宽一些
                yPos = Random.Range(-2f, 2f);
            }
            
            spawnPos = new Vector3(xPos, yPos, 0);
            attempts++;
            
            // 检查是否与现有敌人重叠
            bool isOverlapping = false;
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    float distance = Vector3.Distance(spawnPos, enemy.transform.position);
                    if (distance < 1.0f) // 最小间距1.0f
                    {
                        isOverlapping = true;
                        break;
                    }
                }
            }
            
            if (!isOverlapping)
            {
                Debug.Log($"敌人生成位置: ({spawnPos.x:F2}, {spawnPos.y:F2}) - 菱形路径左边1/4部分");
                return spawnPos;
            }
            
        } while (attempts < maxAttempts);
        
        // 如果尝试次数过多，返回一个稍微偏移的位置
        Debug.LogWarning("无法找到不重叠的位置，使用随机位置");
        float fallbackX = Random.Range(-8f, -6f);
        float fallbackY = Random.Range(-2f, 2f);
        return new Vector3(fallbackX, fallbackY, 0);
    }
    
    // 避免敌人碰撞
    Vector3 AvoidEnemyCollisions(GameObject currentEnemy, Vector3 originalDirection)
    {
        Vector3 adjustedDirection = originalDirection;
        float collisionRadius = 0.8f; // 碰撞检测半径
        float avoidanceStrength = 2.0f; // 避让强度
        
        foreach (var otherEnemy in enemies)
        {
            if (otherEnemy == null || otherEnemy == currentEnemy) continue;
            
            float distance = Vector3.Distance(currentEnemy.transform.position, otherEnemy.transform.position);
            
            // 如果距离太近，计算避让方向
            if (distance < collisionRadius)
            {
                // 计算从其他敌人到当前敌人的方向
                Vector3 avoidanceDirection = (currentEnemy.transform.position - otherEnemy.transform.position).normalized;
                
                // 根据距离计算避让强度
                float avoidanceFactor = (collisionRadius - distance) / collisionRadius;
                
                // 将避让方向添加到原始方向
                adjustedDirection += avoidanceDirection * avoidanceFactor * avoidanceStrength;
                
                // 重新标准化方向向量
                if (adjustedDirection.magnitude > 0.1f)
                {
                    adjustedDirection = adjustedDirection.normalized;
                }
            }
        }
        
        return adjustedDirection;
    }
    
            void CreateArcherOnTower()
    {
        // 创建Archer对象作为Tower的子对象
        GameObject archer = new GameObject("Archer");
        archer.transform.SetParent(towerObj.transform);
        archer.transform.localPosition = new Vector3(0.1f, 1f, 0); // 增加向上偏移
        
        // 添加SpriteRenderer组件
        SpriteRenderer archerRenderer = archer.AddComponent<SpriteRenderer>();
        archerRenderer.sortingOrder = 10; // 确保显示在最前面
        
        // 设置Archer尺寸（不镜像的原始状态）
        archer.transform.localScale = new Vector3(2f, 2f, 2f); // 设置Archer尺寸，不镜像的原始状态
        
        // 设置初始Sprite
        if (archerRest1_1 != null)
        {
            archerRenderer.sprite = archerRest1_1;
            archerRenderer.color = Color.white;
            Debug.Log("Archer图片1_1已设置");
        }
        else
        {
            Debug.LogWarning("Archer休息状态图片1未分配，使用默认方块");
            // 如果没有图片，创建一个默认的方块
            archerRenderer.sprite = CreateSprite();
            archerRenderer.color = Color.yellow; // 使用黄色作为Archer的默认颜色
        }
        
        // 添加Archer动画组件
        ArcherAnimation archerAnim = archer.AddComponent<ArcherAnimation>();
        archerAnim.archerRest1_1 = archerRest1_1;
        archerAnim.archerRest1_2 = archerRest1_2;
        archerAnim.archerRest2_1 = archerRest2_1;
        archerAnim.archerRest2_2 = archerRest2_2;
        
        // 设置射击动画资源
        archerAnim.archerShoot1_1 = archerShoot1_1;
        archerAnim.archerShoot1_2 = archerShoot1_2;
        archerAnim.archerShoot1_3 = archerShoot1_3;
        archerAnim.archerShoot1_4 = archerShoot1_4;
        archerAnim.archerShoot2_1 = archerShoot2_1;
        archerAnim.archerShoot2_2 = archerShoot2_2;
        archerAnim.archerShoot2_3 = archerShoot2_3;
        archerAnim.archerShoot2_4 = archerShoot2_4;
        
        Debug.Log($"Archer组件已添加到Tower上 - 位置: {archer.transform.position}, 尺寸: {archer.transform.localScale}");
    }
    

    
    void CreateEnemyHealthBar(GameObject enemy)
        {
            // 创建血条对象
            GameObject healthBar = new GameObject("EnemyHealthBar");
            healthBar.transform.position = enemy.transform.position + Vector3.up * 0.5f;
            
            // 创建血条渲染器
            SpriteRenderer healthBarRenderer = healthBar.AddComponent<SpriteRenderer>();
            healthBarRenderer.sprite = CreateSprite();
            healthBarRenderer.color = Color.red;
            healthBarRenderer.sortingOrder = 10000;
            
            // 设置血条尺寸
            healthBar.transform.localScale = new Vector3(1f, 0.2f, 1f);
            
            // 将血条设为敌人的子对象
            healthBar.transform.SetParent(enemy.transform);
            
            // 保存血条引用到敌人对象
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            enemyHealth.healthBarObject = healthBar;
            enemyHealth.healthBarRenderer = healthBarRenderer;
            
            // 设置初始血条宽度
            float healthPercent = enemyHealth.GetHealthPercentage();
            healthBar.transform.localScale = new Vector3(1f * healthPercent, 0.2f, 1f);
        }

    // 预测性瞄准：计算弓箭是否能命中移动的敌人
    bool CanHitMovingTarget(Vector3 archerPosition, GameObject targetEnemy)
    {
        if (targetEnemy == null) return false;
        
        // 获取敌人移动组件
        EnemyMovement enemyMovement = targetEnemy.GetComponent<EnemyMovement>();
        if (enemyMovement == null) return false;
        
        // 获取敌人当前速度
        Vector3 enemyVelocity = enemyMovement.GetCurrentVelocity();
        
        // 如果敌人停顿时，直接瞄准当前位置
        if (enemyVelocity.magnitude < 0.1f)
        {
            float distanceToTowerStunned = Vector3.Distance(towerObj.transform.position, targetEnemy.transform.position);
            if (distanceToTowerStunned <= towerRange - 0.05f)
            {
                Debug.Log($"敌人停顿时直接瞄准当前位置");
                return true;
            }
            return false;
        }
        
        // 弓箭飞行时间（秒）
        float arrowFlightTime = 0.5f / gameSpeed;
        
        // 计算敌人在这段时间内的移动距离
        Vector3 enemyDisplacement = enemyVelocity * arrowFlightTime;
        
        // 预测敌人到达的位置
        Vector3 predictedEnemyPosition = targetEnemy.transform.position + enemyDisplacement;
        
        // 计算从弓箭发射点到预测位置的直线距离
        float distanceToPredicted = Vector3.Distance(archerPosition, predictedEnemyPosition);
        
        // 计算弓箭飞行速度（单位/秒）
        float arrowSpeed = distanceToPredicted / arrowFlightTime;
        
        // 检查预测位置是否仍在攻击范围内
        float distanceToTower = Vector3.Distance(towerObj.transform.position, predictedEnemyPosition);
        if (distanceToTower > towerRange - 0.1f)
        {
            Debug.Log($"预测目标位置超出攻击范围: {distanceToTower:F3} > {towerRange - 0.1f}");
            return false;
        }
        
        // 计算弓箭和敌人的相对速度
        Vector3 arrowDirection = (predictedEnemyPosition - archerPosition).normalized;
        float relativeSpeed = Vector3.Dot(enemyVelocity, arrowDirection);
        
        // 如果敌人移动速度太快，弓箭可能追不上
        if (relativeSpeed > arrowSpeed * 0.8f)
        {
            Debug.Log($"敌人移动速度过快，弓箭无法命中: 相对速度 {relativeSpeed:F3} > 弓箭速度的80% {arrowSpeed * 0.8f:F3}");
            return false;
        }
        
        // 计算命中精度（考虑敌人移动造成的误差）
        float accuracyThreshold = 0.3f; // 命中精度阈值
        float movementError = enemyVelocity.magnitude * arrowFlightTime * 0.5f; // 移动造成的误差
        
        if (movementError > accuracyThreshold)
        {
            Debug.Log($"敌人移动造成的误差过大: {movementError:F3} > {accuracyThreshold}");
            return false;
        }
        
        Debug.Log($"预测性瞄准成功 - 敌人当前位置: {targetEnemy.transform.position}, 预测位置: {predictedEnemyPosition}, 移动距离: {enemyDisplacement.magnitude:F3}");
        return true;
    }
    
    // 计算预测的目标位置
    Vector3 CalculatePredictedTargetPosition(Vector3 archerPosition, GameObject targetEnemy)
    {
        if (targetEnemy == null) return targetEnemy.transform.position;
        
        // 获取敌人移动组件
        EnemyMovement enemyMovement = targetEnemy.GetComponent<EnemyMovement>();
        if (enemyMovement == null) return targetEnemy.transform.position;
        
        // 获取敌人当前速度
        Vector3 enemyVelocity = enemyMovement.GetCurrentVelocity();
        
        // 如果敌人停顿时，瞄准当前位置
        if (enemyVelocity.magnitude < 0.1f)
        {
            return targetEnemy.transform.position;
        }
        
        // 弓箭飞行时间（秒）
        float arrowFlightTime = 0.5f / gameSpeed;
        
        // 计算敌人在这段时间内的移动距离
        Vector3 enemyDisplacement = enemyVelocity * arrowFlightTime;
        
        // 预测敌人到达的位置
        Vector3 predictedPosition = targetEnemy.transform.position + enemyDisplacement;
        
        Debug.Log($"计算预测位置: 当前位置 {targetEnemy.transform.position}, 预测位置 {predictedPosition}, 移动距离 {enemyDisplacement.magnitude:F3}");
        return predictedPosition;
    }

    void AttackEnemy()
    {
        // 找到Archer对象
        Transform archerTransform = towerObj.transform.Find("Archer");
        if (archerTransform == null)
        {
            Debug.LogWarning("未找到Archer对象，无法发射箭矢");
            return;
        }
        
        // 找到最优攻击目标（优先级：1.在攻击范围内 2.离基地最近 3.血量最高）
        GameObject bestTarget = null;
        float bestPriority = float.MinValue;
        
        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            
            // 精确检查敌人是否在防御塔攻击范围内
            float distanceToTower = Vector3.Distance(towerObj.transform.position, enemy.transform.position);
            if (distanceToTower <= towerRange - 0.1f) // 留0.1f的安全边距
            {
                // 计算敌人到基地的距离
                float distanceToBase = Vector3.Distance(enemy.transform.position, baseObj.transform.position);
                
                // 获取敌人血量信息
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                float healthPercent = enemyHealth != null ? enemyHealth.GetHealthPercentage() : 1f;
                
                // 计算攻击优先级：优先攻击血量高的敌人（威胁更大）
                float priority = healthPercent * 100f - distanceToBase;
                
                if (priority > bestPriority)
                {
                    bestPriority = priority;
                    bestTarget = enemy;
                }
            }
        }
        
        // 如果找到了目标敌人，进行预测性瞄准检查
        if (bestTarget != null)
        {
            // 最终精确确认目标仍在攻击范围内
            float finalDistanceCheck = Vector3.Distance(towerObj.transform.position, bestTarget.transform.position);
            if (finalDistanceCheck <= towerRange - 0.05f) // 更精确的边距检查
            {
                // 使用预测性瞄准检查是否能命中移动的敌人
                Vector3 archerPosition = archerTransform.position;
                if (CanHitMovingTarget(archerPosition, bestTarget))
                {
                    // 计算射击方向（瞄准预测位置）
                    Vector3 shootDirection = (bestTarget.transform.position - archerTransform.position).normalized;
                    
                    // 触发射击动画
                    ArcherAnimation archerAnim = archerTransform.GetComponent<ArcherAnimation>();
                    if (archerAnim != null)
                    {
                        archerAnim.TriggerShootAnimation(shootDirection);
                    }
                    
                    // 从Archer位置发射箭矢
                    ShowProjectile(archerPosition, bestTarget.transform.position, bestTarget);
                    Debug.Log($"预测性攻击目标: {bestTarget.name} - 距离: {finalDistanceCheck:F3} - 优先级: {bestPriority:F1}");
                }
                else
                {
                    Debug.LogWarning($"预测性瞄准失败，跳过攻击目标: {bestTarget.name}");
                }
            }
            else
            {
                Debug.LogWarning($"目标 {bestTarget.name} 超出精确攻击范围: {finalDistanceCheck:F3} > {towerRange - 0.05f}");
            }
        }
        else
        {
            Debug.Log("未找到合适的攻击目标");
        }
    }

    // 显示移动的箭
    void ShowProjectile(Vector3 from, Vector3 to, GameObject targetEnemy)
    {
        // 计算预测的目标位置
        Vector3 predictedTarget = CalculatePredictedTargetPosition(from, targetEnemy);
        
        // 创建移动的箭
        GameObject arrow = new GameObject("Arrow");
        arrow.transform.position = from;
        
        // 添加箭的渲染器
        SpriteRenderer arrowRenderer = arrow.AddComponent<SpriteRenderer>();
        arrowRenderer.sprite = CreateArrowSprite();
        arrowRenderer.color = Color.gray;
        arrowRenderer.sortingOrder = 1000;
        
        // 设置箭的朝向（瞄准预测位置）
        Vector3 direction = (predictedTarget - from).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        
                        // 设置箭的尺寸
                arrow.transform.localScale = new Vector3(1f, 0.33f, 1f); // 宽度设为1/3
        
        // 开始移动箭（瞄准预测位置）
        StartCoroutine(MoveArrow(arrow, from, predictedTarget, targetEnemy));
    }

    // 协程：移动箭到目标位置并造成伤害
    System.Collections.IEnumerator MoveArrow(GameObject arrow, Vector3 from, Vector3 to, GameObject targetEnemy)
    {
        float duration = 0.8f; // 增加箭的飞行时间，确保有足够时间进行碰撞检测
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime * gameSpeed;
            float progress = elapsed / duration;
            
            // 移动箭的位置
            Vector3 currentPosition = Vector3.Lerp(from, to, progress);
            arrow.transform.position = currentPosition;
            
            // 检查目标敌人是否还存在
            if (targetEnemy != null)
            {
                // 检查目标敌人是否还在攻击范围内
                float distanceToTower = Vector3.Distance(towerObj.transform.position, targetEnemy.transform.position);
                if (distanceToTower > towerRange - 0.05f) // 精确的边距检查
                {
                    // 目标敌人已经超出精确攻击范围，销毁箭并退出
                    Debug.LogWarning($"箭矢飞行中目标超出范围: {distanceToTower:F3} > {towerRange - 0.05f}");
                    Destroy(arrow);
                    yield break;
                }
                
                // 检查箭是否碰到敌人（增加碰撞检测范围）
                float distanceToEnemy = Vector3.Distance(currentPosition, targetEnemy.transform.position);
                if (distanceToEnemy < 0.4f) // 增大碰撞检测范围，确保不会错过
                {
                    Debug.Log($"箭矢碰撞检测成功 - 距离: {distanceToEnemy:F3}, 敌人位置: {targetEnemy.transform.position}, 箭位置: {currentPosition}");
                    
                    // 造成伤害
                    EnemyHealth enemyHealth = targetEnemy.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        // 记录攻击前的血量
                        float healthBefore = enemyHealth.currentHealth;
                        
                        bool enemyDied = enemyHealth.TakeDamage(towerDamage);
                        
                        // 记录攻击后的血量
                        float healthAfter = enemyHealth.currentHealth;
                        
                        Debug.Log($"敌人受到伤害 - 攻击前血量: {healthBefore}, 攻击后血量: {healthAfter}, 伤害值: {towerDamage}, 敌人死亡: {enemyDied}");
                        
                        // 更新血条
                        if (enemyHealth.healthBarRenderer != null)
                        {
                            float healthPercent = enemyHealth.GetHealthPercentage();
                            Vector3 newScale = new Vector3(1f * healthPercent, 0.2f, 1f);
                            enemyHealth.healthBarObject.transform.localScale = newScale;
                        }
                        
                        if (enemyDied)
                        {
                            // 敌人死亡
                            gold += 10;
                            enemies.Remove(targetEnemy);
                            Debug.Log($"精确攻击命中，敌人死亡 - 当前敌人数量: {enemies.Count}");
                            UpdateUI(); // 在移除敌人后更新UI
                        }
                        else
                        {
                            // 敌人未死亡，停顿时间根据游戏速度调整
                            float stunDuration = 0.5f / gameSpeed; // 2倍速时停顿0.25秒，4倍速时停顿0.125秒
                            EnemyMovement enemyMovement = targetEnemy.GetComponent<EnemyMovement>();
                            if (enemyMovement != null)
                            {
                                enemyMovement.StartStun(stunDuration);
                                Debug.Log($"精确攻击命中，敌人被击中，停顿{stunDuration:F3}秒 (游戏速度: {gameSpeed}X)");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError($"敌人没有EnemyHealth组件: {targetEnemy.name}");
                    }
                    
                    // 箭碰到敌人后立即销毁
                    Destroy(arrow);
                    yield break; // 退出协程
                }
            }
            else
            {
                // 目标敌人已经被销毁，销毁箭
                Debug.LogWarning("目标敌人已被销毁，销毁箭矢");
                Destroy(arrow);
                yield break;
            }
            
            yield return null;
        }
        
        // 如果箭没有碰到敌人，到达目标位置后销毁
        Debug.LogWarning("箭矢飞行结束，未命中目标");
        Destroy(arrow);
    }

    void StartWaveBreak()
    {
        isWaveBreak = true;
        waveStartTime = Time.time;
        
        // 在防御塔上方显示提示文本
        ShowWaveBreakText();
        
        Debug.Log($"第{wave}波结束，休息3秒后开始第{wave + 1}波");
    }
    
    void ShowFirstWaveText()
    {
        // 设置第一波提示已显示标志
        firstWaveTextShown = true;
        firstWaveTextDestroyed = false;
        
        // 创建屏幕空间Canvas用于显示第一波提示文本
        GameObject canvasObj = new GameObject("FirstWaveCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // 创建提示文本
        GameObject textObj = new GameObject("FirstWaveText");
        textObj.transform.SetParent(canvasObj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.anchoredPosition = new Vector2(0, 100); // 往上移动，不挡住防御塔
        textRect.sizeDelta = new Vector2(400, 60);
        
        Text text = textObj.AddComponent<Text>();
        text.text = "第1波敌人即将来袭！";
        // 使用Unity内置的默认字体
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 28;
        text.fontStyle = FontStyle.Normal; // 正常字体，不加粗
        text.color = Color.red;
        text.alignment = TextAnchor.MiddleCenter;
        
        // 3秒后销毁提示
        StartCoroutine(DestroyFirstWaveText(canvasObj));
        
        Debug.Log("显示第一波提示：第1波敌人即将来袭！");
    }
    
             System.Collections.IEnumerator DestroyFirstWaveText(GameObject canvasObj)
    {
        yield return new WaitForSeconds(3f / gameSpeed);
        if (canvasObj != null)
        {
            Destroy(canvasObj);
        }
        
        // 设置第一波提示已消失标志
        firstWaveTextDestroyed = true;
        Debug.Log("第一波提示已消失，开始生成敌人");
    }
    
    void ShowWaveBreakText()
    {
        // 创建屏幕空间Canvas用于显示提示文本
        GameObject canvasObj = new GameObject("WaveBreakCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // 创建提示文本
        GameObject textObj = new GameObject("WaveBreakText");
        textObj.transform.SetParent(canvasObj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.anchoredPosition = new Vector2(0, 100); // 往上移动，不挡住防御塔
        textRect.sizeDelta = new Vector2(400, 60);
        
        Text text = textObj.AddComponent<Text>();
        text.text = $"第{wave + 1}波敌人即将来袭！";
        // 使用Unity内置的默认字体
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 28;
        text.fontStyle = FontStyle.Normal; // 正常字体，不加粗
        text.color = Color.red;
        text.alignment = TextAnchor.MiddleCenter;
        
        waveBreakText = text;
        
        Debug.Log($"显示波次提示：第{wave + 1}波敌人即将来袭！");
    }
    
    void NextWave()
    {
        wave++;
        
        // 每波敌人数量增加5个
        enemiesPerWave += 5;
        
        // 每波敌人移动速度增加10%
        enemySpeed *= 1.1f;
        
        // 每波持续时间增加0.5秒
        waveDuration += waveDurationIncrease;
        
        enemiesSpawned = 0;
        nextSpawnTime = Time.time + 1f;
        UpdateUI();
        
        Debug.Log($"第{wave}波：敌人数量 {enemiesPerWave}，移动速度 {enemySpeed:F2}，{waveDuration:F1}秒内生成完毕");
    }

    void GameOver()
    {
        // 清空所有剩余敌人
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] != null)
            {
                Destroy(enemies[i]);
            }
        }
        enemies.Clear();
        
        // 隐藏游戏信息UI，不显示任何游戏信息
        if (topRightText) topRightText.text = "";
        
        // 显示游戏结束提示
        ShowGameOverText();
    }
    
    void ShowGameOverText()
    {
        // 创建屏幕空间Canvas用于显示游戏结束提示
        GameObject canvasObj = new GameObject("GameOverCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // 创建提示文本
        GameObject textObj = new GameObject("GameOverText");
        textObj.transform.SetParent(canvasObj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.anchoredPosition = new Vector2(0, 50); // 往上移动一点，给按钮留空间
        textRect.sizeDelta = new Vector2(500, 80);
        
        Text text = textObj.AddComponent<Text>();
        text.text = "游戏结束！\n基地被摧毁了！";
        // 使用Unity内置的默认字体
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 36;
        text.fontStyle = FontStyle.Bold;
        text.color = Color.red; // 确保使用红色字体
        text.alignment = TextAnchor.MiddleCenter;
        
        // 创建再来一次按钮
        CreateRestartButton(canvasObj.transform);
        
        Debug.Log("游戏结束！基地被摧毁了！");
    }
    
    void CreateRestartButton(Transform parent)
    {
        // 创建再来一次按钮
        GameObject restartButtonObj = new GameObject("RestartButton");
        restartButtonObj.transform.SetParent(parent);
        
        // 添加Image组件作为按钮背景
        Image buttonImage = restartButtonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.8f, 0.9f, 0.8f); // Windows风格浅绿色
        buttonImage.sprite = CreateButtonSprite();
        buttonImage.type = Image.Type.Simple;
        
        // 添加Button组件
        Button restartButton = restartButtonObj.AddComponent<Button>();
        
        // 设置RectTransform - 放在游戏结束文字下方
        RectTransform rect = restartButtonObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0, -80); // 在游戏结束文字下方
        rect.sizeDelta = new Vector2(200, 60);
        
        // 确保按钮可以交互
        restartButton.interactable = true;
        
        // 设置按钮的导航
        Navigation nav = restartButton.navigation;
        nav.mode = Navigation.Mode.None;
        restartButton.navigation = nav;
        
        // 添加CanvasGroup确保UI交互
        CanvasGroup buttonCanvasGroup = restartButtonObj.AddComponent<CanvasGroup>();
        buttonCanvasGroup.interactable = true;
        buttonCanvasGroup.blocksRaycasts = true;
        
        // 创建文本
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(restartButtonObj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = Vector2.zero;
        
        Text textComponent = textObj.AddComponent<Text>();
        textComponent.text = "再来一次";
        // 使用Unity内置的默认字体
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textComponent.fontSize = 24;
        textComponent.color = Color.black; // 黑色文字，符合Windows风格
        textComponent.alignment = TextAnchor.MiddleCenter;
        
        // 设置按钮点击事件
        restartButton.onClick.AddListener(RestartGame);
        
        Debug.Log("再来一次按钮已创建");
    }
    
    void RestartGame()
    {
        Debug.Log("重新开始游戏");
        
        // 重置游戏状态
        gameStarted = false;
        gold = 0;
        wave = 1;
        baseHealth = 10;
        enemiesPerWave = 5;
        enemySpeed = 0.5f;
        waveDuration = 5.0f;
        waveDurationIncrease = 0.5f;
        towerRange = 1.8f;
        towerCooldown = 0.5f;
        towerDamage = 1f;
        nextSpawnTime = 0f;
        enemiesSpawned = 0;
        waveStartTime = 0f;
        isWaveBreak = false;
        waveBreakText = null;
        nextAttackTime = 0f;
        gameSpeed = 1f;
        
        // 重置第一波提示状态
        firstWaveTextShown = false;
        firstWaveTextDestroyed = false;
        
        // 清空所有敌人
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] != null)
            {
                Destroy(enemies[i]);
            }
        }
        enemies.Clear();
        
        // 销毁基地和防御塔
        if (baseObj != null)
        {
            Destroy(baseObj);
        }
        if (towerObj != null)
        {
            Destroy(towerObj);
        }
        
        // 销毁背景地图
        GameObject backgroundMapObj = GameObject.Find("BackgroundMap");
        if (backgroundMapObj != null)
        {
            Destroy(backgroundMapObj);
            Debug.Log("销毁背景地图");
        }
        
        // 销毁游戏结束Canvas
        GameObject gameOverCanvas = GameObject.Find("GameOverCanvas");
        if (gameOverCanvas != null)
        {
            Destroy(gameOverCanvas);
        }
        
        // 销毁速度控制按钮
        GameObject speedControlContainer = GameObject.Find("SpeedControlContainer");
        if (speedControlContainer != null)
        {
            Destroy(speedControlContainer);
        }
        
        // 销毁旧的Canvas（防止UI重叠）
        GameObject oldCanvas = GameObject.Find("Canvas");
        if (oldCanvas != null)
        {
            Destroy(oldCanvas);
            Debug.Log("销毁旧的Canvas以防止UI重叠");
        }
        
        // 清除所有Archer相关的历史记录
        ClearArcherHistory();
        
        // 重新创建UI（不包含开始游戏按钮）
        CreateUIForRestart();
        
        // 直接开始游戏
        StartGame();
        
        Debug.Log("游戏已重置并直接开始");
    }
    
    // 新增：清除Archer射击历史记录
    void ClearArcherHistory()
    {
        Debug.Log("清除Archer射击历史记录");
        
        // 查找所有ArcherAnimation组件并重置其状态
        ArcherAnimation[] archerAnimations = FindObjectsByType<ArcherAnimation>(FindObjectsSortMode.None);
        foreach (ArcherAnimation archerAnim in archerAnimations)
        {
            if (archerAnim != null)
            {
                // 销毁Archer对象
                Destroy(archerAnim.gameObject);
                Debug.Log("销毁Archer对象: " + archerAnim.gameObject.name);
            }
        }
        
        Debug.Log("Archer射击历史记录清除完成");
    }

                                       void CreateUI()
       {
                     var canvasObj = new GameObject("Canvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            
            // 确保Canvas可以处理UI交互
            CanvasGroup mainCanvasGroup = canvasObj.AddComponent<CanvasGroup>();
            mainCanvasGroup.interactable = true;
            mainCanvasGroup.blocksRaycasts = true;
            
                        // 创建EventSystem（如果不存在）
             if (FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                Debug.Log("EventSystem已创建");
            }
           
           // 创建顶部居中合并的UI文本
           topRightText = CreateUIText(canvasObj.transform, "💰 金币: " + gold + " 💰 | 波次: " + wave + " | 基地血量: " + baseHealth + " | 敌人: " + enemies.Count, new Vector2(0.5f, 1), new Vector2(0, -30), Color.white);
           
           // 创建开始游戏按钮
           CreateStartGameButton(canvasObj.transform);
           
           Debug.Log("UI创建完成，只显示开始游戏按钮");
       }

    // 新增：为重新开始游戏创建UI（不包含开始游戏按钮）
    void CreateUIForRestart()
    {
        var canvasObj = new GameObject("Canvas");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // 确保Canvas可以处理UI交互
        CanvasGroup mainCanvasGroup = canvasObj.AddComponent<CanvasGroup>();
        mainCanvasGroup.interactable = true;
        mainCanvasGroup.blocksRaycasts = true;
        
        // 创建EventSystem（如果不存在）
        if (FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            Debug.Log("EventSystem已创建");
        }
       
        // 创建顶部居中合并的UI文本
        topRightText = CreateUIText(canvasObj.transform, "💰 金币: " + gold + " 💰 | 波次: " + wave + " | 基地血量: " + baseHealth + " | 敌人: " + enemies.Count, new Vector2(0.5f, 1), new Vector2(0, -30), Color.white);
        
        // 创建速度控制按钮
        CreateSpeedControlButtons(canvasObj.transform);
        
        Debug.Log("重新开始游戏UI创建完成，速度控制按钮已创建，直接开始游戏");
    }

    void UpdateUI()
    {
        if (topRightText) 
        {
            topRightText.text = "💰 金币: " + gold + " 💰 | 波次: " + wave + " | 基地血量: " + baseHealth + " | 敌人: " + enemies.Count;
            Debug.Log($"UI更新 - 敌人数量: {enemies.Count}");
        }
        
        // 调试信息：显示攻击范围检测状态
        if (enemies.Count > 0)
        {
            Debug.Log($"=== 攻击范围检测状态 ===");
            Debug.Log($"防御塔位置: {towerObj.transform.position}");
            Debug.Log($"理论攻击范围: {towerRange}");
            Debug.Log($"精确攻击范围: {towerRange - 0.1f}");
            
            int inRangeCount = 0;
            int inPreciseRangeCount = 0;
            foreach (var enemy in enemies)
            {
                if (enemy == null) continue;
                
                float distanceToTower = Vector3.Distance(towerObj.transform.position, enemy.transform.position);
                float distanceToBase = Vector3.Distance(enemy.transform.position, baseObj.transform.position);
                
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                float healthPercent = enemyHealth != null ? enemyHealth.GetHealthPercentage() : 1f;
                
                bool inTheoreticalRange = distanceToTower < towerRange;
                bool inPreciseRange = distanceToTower <= towerRange - 0.1f;
                
                if (inTheoreticalRange)
                {
                    inRangeCount++;
                    if (inPreciseRange) inPreciseRangeCount++;
                    Debug.Log($"敌人 {enemy.name}: 距离塔 {distanceToTower:F3}, 距离基地 {distanceToBase:F3}, 血量 {healthPercent:P0}, 理论范围内: {inTheoreticalRange}, 精确范围内: {inPreciseRange}");
                }
            }
            
            Debug.Log($"理论范围内敌人数量: {inRangeCount}/{enemies.Count}");
            Debug.Log($"精确范围内敌人数量: {inPreciseRangeCount}/{enemies.Count}");
            Debug.Log($"=== 攻击范围检测状态结束 ===");
        }
    }

    GameObject CreateGameObject(Vector3 pos, Sprite sprite, Color fallbackColor, string name)
    {
        GameObject go = new GameObject(name);
        go.transform.position = pos;
        var sr = go.AddComponent<SpriteRenderer>();
        
        if (sprite != null)
        {
            sr.sprite = sprite;
            // 如果使用图像，保持原始颜色
            sr.color = Color.white;
        }
        else
        {
            // 如果没有图像，使用纯色方块作为后备
            sr.sprite = CreateSprite();
            sr.color = fallbackColor;
        }
        
        return go;
    }

    Sprite CreateSprite()
    {
        Texture2D tex = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.white;
        tex.SetPixels(pixels);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
    }
    
         Sprite CreateArrowSprite()
     {
         // 创建一个箭头形状的纹理
         Texture2D tex = new Texture2D(32, 32);
         Color[] pixels = new Color[32 * 32];
         
         // 初始化所有像素为透明
         for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.clear;
         
         // 绘制箭头主体（更粗更明显）
         for (int y = 12; y < 20; y++)
         {
             for (int x = 6; x < 26; x++)
             {
                 pixels[y * 32 + x] = Color.white;
             }
         }
         
         // 绘制箭头头部（更大的三角形）
         for (int y = 10; y < 22; y++)
         {
             for (int x = 26; x < 32; x++)
             {
                 if (y >= 12 && y < 20) // 箭头主体
                 {
                     pixels[y * 32 + x] = Color.white;
                 }
                 else if (x == 26 + (y - 10)) // 箭头头部
                 {
                     pixels[y * 32 + x] = Color.white;
                 }
             }
         }
         
         tex.SetPixels(pixels);
         tex.Apply();
         return Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.2f, 0.5f));
     }
     
           Sprite CreateButtonSprite()
      {
          // 创建简单的按钮纹理
          int size = 32;
          Texture2D tex = new Texture2D(size, size);
          Color[] pixels = new Color[size * size];
          
          // 初始化所有像素为白色
          for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.white;
          
          // 绘制简单的按钮边框
          for (int y = 0; y < size; y++)
          {
              for (int x = 0; x < size; x++)
              {
                  // 边框（深灰色）
                  if (x == 0 || x == size - 1 || y == 0 || y == size - 1)
                  {
                      pixels[y * size + x] = new Color(0.6f, 0.6f, 0.6f);
                  }
                  // 内部区域（白色）
                  else
                  {
                      pixels[y * size + x] = Color.white;
                  }
              }
          }
          
          tex.SetPixels(pixels);
          tex.Apply();
          
          // 设置9-slice边框
          return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, new Vector4(4, 4, 4, 4));
      }

    
     
           void CreateSpeedControlButtons(Transform parent)
      {
          // 创建速度控制按钮容器
          GameObject speedControlContainer = new GameObject("SpeedControlContainer");
          speedControlContainer.transform.SetParent(parent);
          RectTransform containerRect = speedControlContainer.AddComponent<RectTransform>();
          containerRect.anchorMin = new Vector2(1, 1); // 右上角
          containerRect.anchorMax = new Vector2(1, 1);
                     containerRect.anchoredPosition = new Vector2(-100, -30); // 调整位置以适应5个按钮
           containerRect.sizeDelta = new Vector2(125, 25); // 增加宽度以适应5个按钮
          
          // 添加CanvasGroup组件来确保UI可以交互
          CanvasGroup canvasGroup = speedControlContainer.AddComponent<CanvasGroup>();
          canvasGroup.interactable = true;
          canvasGroup.blocksRaycasts = true;
         
                              // 创建0.5X按钮
           speed05xButton = CreateSpeedButton(speedControlContainer.transform, "0.5X", new Vector2(0, 0), new Vector2(0.2f, 1f), new Vector2(0, 0), new Color(0.9f, 0.9f, 0.9f));
          speed05xText = speed05xButton.GetComponentInChildren<Text>();
          Debug.Log($"0.5X按钮创建完成 - 可交互: {speed05xButton.interactable}, 位置: {speed05xButton.GetComponent<RectTransform>().anchoredPosition}");
          
          // 创建1X按钮
           speed1xButton = CreateSpeedButton(speedControlContainer.transform, "1X", new Vector2(0.2f, 0), new Vector2(0.4f, 1f), new Vector2(0, 0), new Color(0.8f, 0.9f, 0.8f));
          speed1xText = speed1xButton.GetComponentInChildren<Text>();
          Debug.Log($"1X按钮创建完成 - 可交互: {speed1xButton.interactable}, 位置: {speed1xButton.GetComponent<RectTransform>().anchoredPosition}");
          
                     // 创建2X按钮
           speed2xButton = CreateSpeedButton(speedControlContainer.transform, "2X", new Vector2(0.4f, 0), new Vector2(0.6f, 1f), new Vector2(0, 0), new Color(0.9f, 0.9f, 0.9f));
          speed2xText = speed2xButton.GetComponentInChildren<Text>();
          Debug.Log($"2X按钮创建完成 - 可交互: {speed2xButton.interactable}");
          
                     // 创建3X按钮
           speed3xButton = CreateSpeedButton(speedControlContainer.transform, "3X", new Vector2(0.6f, 0), new Vector2(0.8f, 1f), new Vector2(0, 0), new Color(0.9f, 0.9f, 0.9f));
          speed3xText = speed3xButton.GetComponentInChildren<Text>();
          Debug.Log($"3X按钮创建完成 - 可交互: {speed3xButton.interactable}");
          
                     // 创建4X按钮
           speed4xButton = CreateSpeedButton(speedControlContainer.transform, "4X", new Vector2(0.8f, 0), new Vector2(1f, 1f), new Vector2(0, 0), new Color(0.9f, 0.9f, 0.9f));
          speed4xText = speed4xButton.GetComponentInChildren<Text>();
          Debug.Log($"4X按钮创建完成 - 可交互: {speed4xButton.interactable}");
         
                   // 设置按钮点击事件
          speed05xButton.onClick.AddListener(() => SetGameSpeed(0.5f));
          speed1xButton.onClick.AddListener(() => SetGameSpeed(1f));
          speed2xButton.onClick.AddListener(() => SetGameSpeed(2f));
          speed3xButton.onClick.AddListener(() => SetGameSpeed(3f));
          speed4xButton.onClick.AddListener(() => SetGameSpeed(4f));
          
          Debug.Log($"按钮点击事件已设置 - 1X: {speed1xButton.onClick.GetPersistentEventCount()} 个事件");
         
         // 默认选中1X
         SetGameSpeed(1f);
         
         Debug.Log("速度控制按钮已创建完成");
     }
     
           Button CreateSpeedButton(Transform parent, string text, Vector2 anchorMin, Vector2 anchorMax, Vector2 sizeDelta, Color color)
      {
          GameObject buttonObj = new GameObject("SpeedButton_" + text);
          buttonObj.transform.SetParent(parent);
          
          // 添加Image组件作为按钮背景
          Image buttonImage = buttonObj.AddComponent<Image>();
          buttonImage.color = color;
          
                     // 设置简单的按钮外观
           buttonImage.sprite = CreateButtonSprite();
           buttonImage.type = Image.Type.Simple;
          
                     // 添加Button组件
           Button button = buttonObj.AddComponent<Button>();
           
           // 设置RectTransform
           RectTransform rect = buttonObj.GetComponent<RectTransform>();
           rect.anchorMin = anchorMin;
           rect.anchorMax = anchorMax;
           rect.anchoredPosition = Vector2.zero;
           rect.sizeDelta = sizeDelta;
           
           // 确保按钮可以交互
           button.interactable = true;
           
           // 设置按钮的导航
           Navigation nav = button.navigation;
           nav.mode = Navigation.Mode.None;
           button.navigation = nav;
          
          // 添加CanvasGroup确保UI交互
          CanvasGroup buttonCanvasGroup = buttonObj.AddComponent<CanvasGroup>();
          buttonCanvasGroup.interactable = true;
          buttonCanvasGroup.blocksRaycasts = true;
         
         // 创建文本
         GameObject textObj = new GameObject("Text");
         textObj.transform.SetParent(buttonObj.transform);
         RectTransform textRect = textObj.AddComponent<RectTransform>();
         textRect.anchorMin = Vector2.zero;
         textRect.anchorMax = Vector2.one;
         textRect.anchoredPosition = Vector2.zero;
         textRect.sizeDelta = Vector2.zero;
         
         Text textComponent = textObj.AddComponent<Text>();
         textComponent.text = text;
         // 使用Unity内置的默认字体
         textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
         textComponent.fontSize = 9; // 缩小字体到1/3
         textComponent.color = Color.black;
         textComponent.alignment = TextAnchor.MiddleCenter;
         
         return button;
     }
     
           void SetGameSpeed(float speed)
      {
          gameSpeed = speed;
          
          // 更新按钮颜色 - Windows风格
          Color selectedColor = new Color(0.8f, 0.9f, 0.8f); // 浅绿色
          Color normalColor = new Color(0.9f, 0.9f, 0.9f);   // 浅灰色
          
          speed05xButton.GetComponent<Image>().color = (speed == 0.5f) ? selectedColor : normalColor;
          speed1xButton.GetComponent<Image>().color = (speed == 1f) ? selectedColor : normalColor;
          speed2xButton.GetComponent<Image>().color = (speed == 2f) ? selectedColor : normalColor;
          speed3xButton.GetComponent<Image>().color = (speed == 3f) ? selectedColor : normalColor;
          speed4xButton.GetComponent<Image>().color = (speed == 4f) ? selectedColor : normalColor;
          
                     Debug.Log($"游戏速度设置为: {speed}X");
       }
       
               void CreateStartGameButton(Transform parent)
        {
            // 创建开始游戏按钮
            GameObject startButtonObj = new GameObject("StartGameButton");
            startButtonObj.transform.SetParent(parent);
            
            // 添加Image组件作为按钮背景
            Image buttonImage = startButtonObj.AddComponent<Image>();
            buttonImage.color = new Color(0.8f, 0.9f, 0.8f); // Windows风格浅绿色
            buttonImage.sprite = CreateButtonSprite();
            buttonImage.type = Image.Type.Simple;
            
            // 添加Button组件
            startGameButton = startButtonObj.AddComponent<Button>();
            
            // 设置RectTransform - 放在屏幕中央
            RectTransform rect = startButtonObj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0, 0);
            rect.sizeDelta = new Vector2(200, 60);
            
            // 确保按钮可以交互
            startGameButton.interactable = true;
            
            // 设置按钮的导航
            Navigation nav = startGameButton.navigation;
            nav.mode = Navigation.Mode.None;
            startGameButton.navigation = nav;
            
            // 添加CanvasGroup确保UI交互
            CanvasGroup buttonCanvasGroup = startButtonObj.AddComponent<CanvasGroup>();
            buttonCanvasGroup.interactable = true;
            buttonCanvasGroup.blocksRaycasts = true;
            
            // 创建文本
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(startButtonObj.transform);
            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = Vector2.zero;
            
            Text textComponent = textObj.AddComponent<Text>();
            textComponent.text = "开始游戏";
            // 使用Unity内置的默认字体
            textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            textComponent.fontSize = 24;
            textComponent.color = Color.black; // 改为黑色文字，符合Windows风格
            textComponent.alignment = TextAnchor.MiddleCenter;
            
            // 设置按钮点击事件
            startGameButton.onClick.AddListener(StartGame);
            
            Debug.Log("开始游戏按钮已创建 - Windows风格");
        }
       
        // 创建背景地图
        void CreateBackgroundMap()
        {
            if (backgroundMap != null)
            {
                // 创建背景地图对象
                GameObject backgroundObj = new GameObject("BackgroundMap");
                
                // 添加SpriteRenderer组件
                SpriteRenderer spriteRenderer = backgroundObj.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = backgroundMap;
                spriteRenderer.sortingOrder = -10; // 确保背景在最底层
                
                // 设置位置到世界坐标原点
                backgroundObj.transform.position = Vector3.zero;
                
                // 获取屏幕尺寸
                float screenHeight = Camera.main.orthographicSize * 2f;
                float screenWidth = screenHeight * Camera.main.aspect;
                
                // 获取地图原始尺寸
                float mapWidth = backgroundMap.bounds.size.x;
                float mapHeight = backgroundMap.bounds.size.y;
                
                // 计算拉伸比例以完全铺满屏幕
                float scaleX = screenWidth / mapWidth;
                float scaleY = screenHeight / mapHeight;
                
                // 直接应用拉伸缩放，确保地图完全覆盖屏幕
                backgroundObj.transform.localScale = new Vector3(scaleX, scaleY, 1f);
                
                Debug.Log($"背景地图已拉伸铺满屏幕 - 屏幕尺寸: {screenWidth}x{screenHeight}, 地图尺寸: {mapWidth}x{mapHeight}, 缩放比例: X={scaleX:F2}, Y={scaleY:F2}");
            }
            else
            {
                Debug.LogWarning("背景地图Sprite未设置，跳过背景创建");
            }
        }
       
                               void StartGame()
         {
             if (!gameStarted)
             {
                 gameStarted = true;
                 
                 // 隐藏开始按钮
                 if (startGameButton != null)
                 {
                     startGameButton.gameObject.SetActive(false);
                 }
                 
                 // 创建背景地图
                 CreateBackgroundMap();
                 
                 // 创建游戏元素
                 CreateBase();
                 CreateTower();
                 
                 // 创建速度控制按钮
                 CreateSpeedControlButtons(startGameButton.transform.parent);
                 
                 // 显示第一波即将来袭的提示
                 ShowFirstWaveText();
                 
                 Debug.Log("游戏开始！速度控制按钮已显示");
             }
         }
     
     Text CreateUIText(Transform parent, string text, Vector2 anchor, Vector2 pos, Color color)
     {
         GameObject go = new GameObject("Text");
         go.transform.SetParent(parent);
         var rect = go.AddComponent<RectTransform>();
         rect.anchorMin = anchor;
         rect.anchorMax = anchor;
         rect.anchoredPosition = pos;
         rect.sizeDelta = new Vector2(400, 50);
         var t = go.AddComponent<Text>();
         t.text = text;
         // 使用Unity内置的默认字体
         t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
         t.fontSize = 20;
         t.color = color;
         t.alignment = TextAnchor.MiddleCenter;
         
                      // 添加1像素的黑色光晕效果
             var shadow = go.AddComponent<Shadow>();
             shadow.effectColor = new Color(0f, 0f, 0f, 0.5f); // 黑色，50%透明度
             shadow.effectDistance = new Vector2(1f, 1f); // 1像素偏移
         
         return t;
     }

    // 计算UI缩放比例和元素尺寸
    void CalculateUIScale()
    {
        // 获取屏幕分辨率
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        
        // 基于屏幕分辨率计算UI缩放
        float screenAspect = screenWidth / screenHeight;
        
        // 参考分辨率 (1920x1080)
        float referenceWidth = 1920f;
        float referenceHeight = 1080f;
        float referenceAspect = referenceWidth / referenceHeight;
        
        // 计算缩放比例
        if (screenAspect > referenceAspect)
        {
            // 宽屏，以高度为基准
            uiScale = (screenHeight / referenceHeight) * uiScaleFactor;
        }
        else
        {
            // 窄屏，以宽度为基准
            uiScale = (screenWidth / referenceWidth) * uiScaleFactor;
        }
        
        // 限制缩放范围
        uiScale = Mathf.Clamp(uiScale, minUIScale, maxUIScale);
        
        // 计算UI元素尺寸
        CalculateUIElementSizes();
    }
    
    // 计算UI元素尺寸
    void CalculateUIElementSizes()
    {
        // 基础尺寸（基于1920x1080分辨率）
        float baseButtonHeight = 60f;
        float baseButtonWidth = 200f;
        float baseTextFontSize = 20f;
        float baseInfoPanelHeight = 50f;
        float baseSpeedButtonWidth = 25f;
        float baseSpeedButtonHeight = 25f;
        
        // 应用缩放
        buttonHeight = baseButtonHeight * uiScale;
        buttonWidth = baseButtonWidth * uiScale;
        textFontSize = Mathf.RoundToInt(baseTextFontSize * uiScale);
        infoPanelHeight = baseInfoPanelHeight * uiScale;
        speedButtonWidth = baseSpeedButtonWidth * uiScale;
        speedButtonHeight = baseSpeedButtonHeight * uiScale;
        
        Debug.Log($"UI元素尺寸计算完成 - 按钮: {buttonWidth}x{buttonHeight}, 字体: {textFontSize}, 信息面板: {infoPanelHeight}");
    }
} 