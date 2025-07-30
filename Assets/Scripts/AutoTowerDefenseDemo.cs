using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AutoTowerDefenseDemo : MonoBehaviour
{
    [Header("图像资源")]
    public Sprite baseSprite;        // 基地图像
    public Sprite towerSprite;       // 防御塔图像
    public Sprite enemySprite;       // 敌人图像
    
    [Header("动画资源")]
    public Sprite[] enemyWalkSprites;  // 敌人行走动画帧
    
    // 游戏数据
    int gold = 0;
    int wave = 1;
    int baseHealth = 10;
    int enemiesPerWave = 5;
    float enemySpeed = 0.5f;
    float waveDuration = 5.0f; // 每波持续5秒
    float waveDurationIncrease = 0.5f; // 每波增加0.5秒
    float towerRange = 3f;
    float towerCooldown = 0.5f; // 初始0.5秒攻击1次
    float towerDamage = 1f; // 防御塔攻击力
    float nextSpawnTime = 0f;
    int enemiesSpawned = 0;
    float waveStartTime = 0f;
    bool isWaveBreak = false;
    Text waveBreakText;
    float nextAttackTime = 0f; // 下次攻击时间

    // 游戏对象
    GameObject baseObj;
    GameObject towerObj;
    List<GameObject> enemies = new List<GameObject>();

    // UI
    Text topRightText;

    void Start()
    {
        CreateBase();
        CreateTower();
        CreateUI();
        
        // 显示第一波即将来袭的提示
        ShowFirstWaveText();
    }

    void Update()
    {
        // 如果基地血量为0，游戏结束，停止所有逻辑
        if (baseHealth <= 0)
        {
            return;
        }
        
        // 波次间休息时间
        if (isWaveBreak)
        {
            if (Time.time >= waveStartTime + 3f)
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
            // 计算当前波次应该生成的敌人数量
            float waveProgress = (Time.time - nextSpawnTime) / waveDuration;
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
            
            // 移动敌人
            Vector3 newPosition = enemies[i].transform.position + adjustedMoveDirection * enemySpeed * Time.deltaTime;
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
            nextAttackTime = Time.time + towerCooldown;
        }
        else if (towerObj == null)
        {
            Debug.LogError("防御塔对象丢失！");
        }
        // 波次结束 - 5秒后或所有敌人被消灭后
        if ((enemiesSpawned == enemiesPerWave && enemies.Count == 0) || 
            (Time.time >= nextSpawnTime + waveDuration && enemies.Count == 0))
        {
            if (baseHealth > 0)
            {
                StartWaveBreak();
            }
        }
    }

    void CreateBase()
    {
        baseObj = CreateGameObject(new Vector3(6, 0, 0), baseSprite, Color.blue, "Base");
    }

    void CreateTower()
    {
        towerObj = CreateGameObject(new Vector3(2, 0, 0), towerSprite, Color.green, "Tower");
        // 缩小防御塔尺寸到1/5
        towerObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        
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
        
        // 创建攻击范围显示
        CreateTowerRangeIndicator();
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
            spawnPos = new Vector3(Random.Range(-10f, -6f), Random.Range(-3f, 3f), 0);
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
                return spawnPos;
            }
            
        } while (attempts < maxAttempts);
        
        // 如果尝试次数过多，返回一个稍微偏移的位置
        Debug.LogWarning("无法找到不重叠的位置，使用随机位置");
        return new Vector3(Random.Range(-10f, -6f), Random.Range(-3f, 3f), 0);
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
    
            void CreateTowerRangeIndicator()
    {
        // 创建攻击范围指示器
        GameObject rangeIndicator = new GameObject("TowerRangeIndicator");
        rangeIndicator.transform.position = towerObj.transform.position;
        
        // 添加圆形精灵渲染器
        SpriteRenderer rangeRenderer = rangeIndicator.AddComponent<SpriteRenderer>();
        rangeRenderer.sprite = CreateCircleSprite();
        rangeRenderer.color = new Color(1f, 1f, 1f, 0.1f); // 10%透明度
        rangeRenderer.sortingOrder = -1; // 显示在防御塔后面
        
        // 设置范围大小（直径等于攻击范围的两倍，因为towerRange是半径）
        float rangeSize = towerRange * 2f;
        rangeIndicator.transform.localScale = new Vector3(rangeSize, rangeSize, 1f);
    }
    
    Sprite CreateCircleSprite()
    {
        // 创建一个更高质量的圆形纹理
        int size = 128; // 增加分辨率
        Texture2D tex = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];
        
        // 初始化所有像素为透明
        for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.clear;
        
        // 绘制更光滑的圆形
        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f - 4f; // 留更多边距
        
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                if (distance <= radius)
                {
                    // 添加边缘抗锯齿效果
                    float edgeDistance = radius - distance;
                    if (edgeDistance < 2f)
                    {
                        // 边缘渐变，使圆圈更光滑
                        float alpha = edgeDistance / 2f;
                        pixels[y * size + x] = new Color(1f, 1f, 1f, alpha);
                    }
                    else
                    {
                        pixels[y * size + x] = Color.white;
                    }
                }
            }
        }
        
        tex.SetPixels(pixels);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
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

    void AttackEnemy()
    {
        // 找到离基地最近的敌人（在攻击范围内）
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;
        
        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            
            // 检查敌人是否在防御塔攻击范围内
            float distanceToTower = Vector3.Distance(towerObj.transform.position, enemy.transform.position);
            if (distanceToTower < towerRange)
            {
                // 计算敌人到基地的距离
                float distanceToBase = Vector3.Distance(enemy.transform.position, baseObj.transform.position);
                
                // 如果这个敌人离基地更近，选择它
                if (distanceToBase < closestDistance)
                {
                    closestDistance = distanceToBase;
                    closestEnemy = enemy;
                }
            }
        }
        
        // 如果找到了目标敌人，进行攻击
        if (closestEnemy != null)
        {
            // 再次确认目标仍在攻击范围内
            float finalDistanceCheck = Vector3.Distance(towerObj.transform.position, closestEnemy.transform.position);
            if (finalDistanceCheck < towerRange)
            {
                // 显示弹道，传递敌人引用
                ShowProjectile(towerObj.transform.position, closestEnemy.transform.position, closestEnemy);
            }
        }
    }

    // 显示移动的箭
    void ShowProjectile(Vector3 from, Vector3 to, GameObject targetEnemy)
    {
        // 创建移动的箭
        GameObject arrow = new GameObject("Arrow");
        arrow.transform.position = from;
        
        // 添加箭的渲染器
        SpriteRenderer arrowRenderer = arrow.AddComponent<SpriteRenderer>();
        arrowRenderer.sprite = CreateArrowSprite();
        arrowRenderer.color = Color.gray;
        arrowRenderer.sortingOrder = 1000;
        
        // 设置箭的朝向
        Vector3 direction = (to - from).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        // 设置箭的尺寸
        arrow.transform.localScale = new Vector3(1f, 1f, 1f);
        
        // 开始移动箭
        StartCoroutine(MoveArrow(arrow, from, to, targetEnemy));
    }

    // 协程：移动箭到目标位置并造成伤害
    System.Collections.IEnumerator MoveArrow(GameObject arrow, Vector3 from, Vector3 to, GameObject targetEnemy)
    {
        float duration = 0.5f; // 缩短箭的飞行时间
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            
            // 移动箭的位置
            Vector3 currentPosition = Vector3.Lerp(from, to, progress);
            arrow.transform.position = currentPosition;
            
            // 检查目标敌人是否还在攻击范围内
            if (targetEnemy != null)
            {
                float distanceToTower = Vector3.Distance(towerObj.transform.position, targetEnemy.transform.position);
                if (distanceToTower > towerRange)
                {
                    // 目标敌人已经超出攻击范围，销毁箭并退出
                    Destroy(arrow);
                    yield break;
                }
                
                // 检查箭是否碰到敌人
                float distanceToEnemy = Vector3.Distance(currentPosition, targetEnemy.transform.position);
                if (distanceToEnemy < 0.3f) // 减小碰撞检测范围
                {
                    // 造成伤害
                    EnemyHealth enemyHealth = targetEnemy.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        bool enemyDied = enemyHealth.TakeDamage(towerDamage);
                        
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
                            Debug.Log($"敌人死亡 - 当前敌人数量: {enemies.Count}");
                            UpdateUI(); // 在移除敌人后更新UI
                        }
                    }
                    
                    // 箭碰到敌人后立即销毁
                    Destroy(arrow);
                    yield break; // 退出协程
                }
            }
            
            yield return null;
        }
        
        // 如果箭没有碰到敌人，到达目标位置后销毁
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
        yield return new WaitForSeconds(3f);
        if (canvasObj != null)
        {
            Destroy(canvasObj);
        }
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
        
        // 更新UI显示
        if (topRightText) topRightText.text = "💰 金币: " + gold + " 💰 | 波次: " + wave + " | 基地血量: 0 (游戏结束) | 敌人: 0";
        
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
        textRect.anchoredPosition = new Vector2(0, 0); // 屏幕中央
        textRect.sizeDelta = new Vector2(500, 80);
        
        Text text = textObj.AddComponent<Text>();
        text.text = "游戏结束！\n基地被摧毁了！";
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 36;
        text.fontStyle = FontStyle.Bold;
        text.color = Color.red;
        text.alignment = TextAnchor.MiddleCenter;
        
        Debug.Log("游戏结束！基地被摧毁了！");
    }

    void CreateUI()
    {
        var canvasObj = new GameObject("Canvas");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // 创建顶部居中合并的UI文本
        topRightText = CreateUIText(canvasObj.transform, "💰 金币: " + gold + " 💰 | 波次: " + wave + " | 基地血量: " + baseHealth + " | 敌人: " + enemies.Count, new Vector2(0.5f, 1), new Vector2(0, -30), Color.white);
    }

    void UpdateUI()
    {
        if (topRightText) 
        {
            topRightText.text = "💰 金币: " + gold + " 💰 | 波次: " + wave + " | 基地血量: " + baseHealth + " | 敌人: " + enemies.Count;
            Debug.Log($"UI更新 - 敌人数量: {enemies.Count}");
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
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize = 20;
        t.color = color;
        t.alignment = TextAnchor.MiddleCenter;
        return t;
    }
} 