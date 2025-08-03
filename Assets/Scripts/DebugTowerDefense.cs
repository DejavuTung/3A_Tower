using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugTowerDefense : MonoBehaviour
{
    [Header("图像资源")]
    public Sprite baseSprite;        // 基地图像
    public Sprite towerSprite;       // 防御塔图像
    public Sprite enemySprite;       // 敌人图像
    
    [Header("动画资源")]
    public Sprite[] enemyWalkSprites;  // 敌人行走动画帧
    
    // 游戏数据
    int gold = 500;
    int wave = 1;
    int baseHealth = 10;
    int enemiesPerWave = 5;
    float enemySpeed = 1.5f;
    float spawnInterval = 1.0f;
    float towerRange = 3f;
    float towerCooldown = 1f;
    float nextSpawnTime = 0f;
    int enemiesSpawned = 0;

    // 游戏对象
    GameObject baseObj;
    GameObject towerObj;
    List<GameObject> enemies = new List<GameObject>();

    // UI
    Text goldText, waveText, healthText;

    void Start()
    {
        Debug.Log("=== DebugTowerDefense: 开始初始化 ===");
        Debug.Log($"基地图像: {(baseSprite != null ? "已分配" : "未分配")}");
        Debug.Log($"防御塔图像: {(towerSprite != null ? "已分配" : "未分配")}");
        Debug.Log($"敌人图像: {(enemySprite != null ? "已分配" : "未分配")}");
        Debug.Log($"敌人动画帧: {(enemyWalkSprites != null ? enemyWalkSprites.Length + "帧" : "未分配")}");
        
        CreateBase();
        CreateTower();
        CreateUI();
        Debug.Log("=== DebugTowerDefense: 初始化完成 ===");
    }

    void Update()
    {
        // 敌人生成
        if (enemiesSpawned < enemiesPerWave && Time.time >= nextSpawnTime && baseHealth > 0)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
            enemiesSpawned++;
        }
        
        // 敌人移动
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null) continue;
            enemies[i].transform.position = Vector3.MoveTowards(enemies[i].transform.position, baseObj.transform.position, enemySpeed * Time.deltaTime);
            if (Vector3.Distance(enemies[i].transform.position, baseObj.transform.position) < 0.2f)
            {
                Destroy(enemies[i]);
                enemies.RemoveAt(i);
                baseHealth--;
                UpdateUI();
                if (baseHealth <= 0) GameOver();
            }
        }
        
        // 塔攻击
        if (baseHealth > 0 && Time.frameCount % (int)(towerCooldown * 60) == 0)
        {
            AttackEnemy();
        }
        
        // 波次结束
        if (enemiesSpawned == enemiesPerWave && enemies.Count == 0 && baseHealth > 0)
        {
            NextWave();
        }
    }

    void CreateBase()
    {
        baseObj = CreateGameObject(new Vector3(6, 0, 0), baseSprite, Color.blue, "Base");
        Debug.Log("基地创建完成");
    }

    void CreateTower()
    {
        towerObj = CreateGameObject(new Vector3(2, 0, 0), towerSprite, Color.green, "Tower");
        Debug.Log("防御塔创建完成");
    }

    void SpawnEnemy()
    {
        Debug.Log($"生成敌人 - enemySprite: {(enemySprite != null ? "已分配" : "未分配")}");
        
        GameObject enemy = CreateGameObject(new Vector3(-6, Random.Range(-2f, 2f), 0), enemySprite, Color.red, "Enemy");
        
        // 检查敌人的SpriteRenderer
        SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Debug.Log($"敌人SpriteRenderer - Sprite: {(sr.sprite != null ? "已设置" : "未设置")}, Color: {sr.color}");
            
            // 强制设置为白色以显示原始颜色
            sr.color = Color.white;
            Debug.Log("敌人颜色已设置为白色");
        }
        
        // 添加动画组件
        if (enemyWalkSprites != null && enemyWalkSprites.Length > 0)
        {
            try
            {
                SimpleAnimation anim = enemy.AddComponent<SimpleAnimation>();
                anim.SetAnimationFrames(enemyWalkSprites);
                Debug.Log("敌人动画已添加");
                
                // 确保动画使用正确的颜色
                if (sr != null)
                {
                    sr.color = Color.white;
                    Debug.Log("动画敌人颜色已设置为白色");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("添加动画失败 - " + e.Message);
            }
        }
        else
        {
            Debug.Log("没有动画帧，使用静态图像");
        }
        
        enemies.Add(enemy);
        Debug.Log($"敌人已添加，当前敌人数量: {enemies.Count}");
    }

    void AttackEnemy()
    {
        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            if (Vector3.Distance(towerObj.transform.position, enemy.transform.position) < towerRange)
            {
                Destroy(enemy);
                gold += 10;
                UpdateUI();
                enemies.Remove(enemy);
                break;
            }
        }
    }

    void NextWave()
    {
        wave++;
        enemiesPerWave += 2;
        enemySpeed += 0.2f;
        enemiesSpawned = 0;
        nextSpawnTime = Time.time + 1f;
        UpdateUI();
    }

    void GameOver()
    {
        healthText.text = "基地血量: 0 (游戏结束)";
    }

    void CreateUI()
    {
        var canvasObj = new GameObject("Canvas");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        goldText = CreateUIText(canvasObj.transform, "💰 金币: " + gold + " 💰", new Vector2(0.5f, 1), new Vector2(0, -50), Color.yellow);
        waveText = CreateUIText(canvasObj.transform, "波次: " + wave, new Vector2(0.5f, 1), new Vector2(0, -100), Color.white);
        healthText = CreateUIText(canvasObj.transform, "基地血量: " + baseHealth, new Vector2(0.5f, 1), new Vector2(0, -150), Color.cyan);
    }

    void UpdateUI()
    {
        if (goldText) goldText.text = "💰 金币: " + gold + " 💰";
        if (waveText) waveText.text = "波次: " + wave;
        if (healthText) healthText.text = "基地血量: " + baseHealth;
    }

    GameObject CreateGameObject(Vector3 pos, Sprite sprite, Color fallbackColor, string name)
    {
        GameObject go = new GameObject(name);
        go.transform.position = pos;
        var sr = go.AddComponent<SpriteRenderer>();
        
        Debug.Log($"创建游戏对象 {name} - Sprite: {(sprite != null ? "已分配" : "未分配")}");
        
        if (sprite != null)
        {
            sr.sprite = sprite;
            sr.color = Color.white;
            Debug.Log($"{name} 使用分配的Sprite，颜色设为白色");
        }
        else
        {
            sr.sprite = CreateSprite();
            sr.color = fallbackColor;
            Debug.Log($"{name} 使用后备颜色: {fallbackColor}");
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

    Text CreateUIText(Transform parent, string text, Vector2 anchor, Vector2 pos, Color color)
    {
        GameObject go = new GameObject("Text");
        go.transform.SetParent(parent);
        var rect = go.AddComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.anchoredPosition = pos;
        rect.sizeDelta = new Vector2(300, 50);
        var t = go.AddComponent<Text>();
        t.text = text;
        // 使用默认字体，避免DontSaveInEditor冲突
        t.font = null; // Unity会自动使用默认字体
        t.fontSize = 24;
        t.color = color;
        t.alignment = TextAnchor.MiddleCenter;
        return t;
    }
} 