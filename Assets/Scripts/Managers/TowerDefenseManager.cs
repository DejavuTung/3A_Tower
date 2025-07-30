using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 塔防游戏管理器 - 类似部落冲突的核心系统
/// </summary>
public class TowerDefenseManager : MonoBehaviour
{
    [Header("游戏设置")]
    public int playerGold = 1000;
    public int playerGems = 50;
    public int currentWave = 0;
    public int totalWaves = 10;
    public float waveDelay = 5f;
    
    [Header("基地设置")]
    public GameObject mainBase;
    public int baseHealth = 100;
    public int maxBaseHealth = 100;
    
    [Header("UI引用")]
    public Text goldText;
    public Text gemsText;
    public Text waveText;
    public Text baseHealthText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    
    [Header("预制体引用")]
    public GameObject[] towerPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] resourcePrefabs;
    
    [Header("路径设置")]
    public Transform[] pathPoints;
    public Transform spawnPoint;
    public Transform basePoint;
    
    // 私有变量
    private List<GameObject> activeEnemies = new List<GameObject>();
    private List<GameObject> activeTowers = new List<GameObject>();
    private List<GameObject> activeResources = new List<GameObject>();
    
    // 单例模式
    public static TowerDefenseManager Instance { get; private set; }
    
    // 事件
    public System.Action<int> OnGoldChanged;
    public System.Action<int> OnGemsChanged;
    public System.Action<int> OnBaseHealthChanged;
    public System.Action<int> OnWaveChanged;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        InitializeGame();
    }
    
    /// <summary>
    /// 初始化游戏
    /// </summary>
    private void InitializeGame()
    {
        UpdateUI();
        StartNextWave();
    }
    
    /// <summary>
    /// 更新UI显示
    /// </summary>
    private void UpdateUI()
    {
        if (goldText != null) goldText.text = "金币: " + playerGold;
        if (gemsText != null) gemsText.text = "宝石: " + playerGems;
        if (waveText != null) waveText.text = "波次: " + currentWave + "/" + totalWaves;
        if (baseHealthText != null) baseHealthText.text = "基地血量: " + baseHealth + "/" + maxBaseHealth;
        
        OnGoldChanged?.Invoke(playerGold);
        OnGemsChanged?.Invoke(playerGems);
        OnBaseHealthChanged?.Invoke(baseHealth);
        OnWaveChanged?.Invoke(currentWave);
    }
    
    /// <summary>
    /// 开始下一波敌人
    /// </summary>
    public void StartNextWave()
    {
        if (currentWave >= totalWaves)
        {
            Victory();
            return;
        }
        
        currentWave++;
        UpdateUI();
        
        // 生成敌人
        StartCoroutine(SpawnWave(currentWave));
    }
    
    /// <summary>
    /// 生成一波敌人
    /// </summary>
    private System.Collections.IEnumerator SpawnWave(int waveNumber)
    {
        int enemyCount = 5 + waveNumber * 2; // 每波敌人数量递增
        float spawnInterval = 2f - (waveNumber * 0.1f); // 生成间隔递减
        
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
    /// <summary>
    /// 生成敌人
    /// </summary>
    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnPoint == null) return;
        
        // 随机选择敌人类型
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyPrefab = enemyPrefabs[enemyIndex];
        
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        
        // 简化的敌人生成逻辑，避免依赖已删除的EnemyController
        Debug.Log($"生成敌人 {enemyIndex}，波次: {currentWave}");
        
        activeEnemies.Add(enemy);
    }
    
    /// <summary>
    /// 建造防御塔
    /// </summary>
    public bool BuildTower(int towerIndex, Vector3 position)
    {
        if (towerIndex < 0 || towerIndex >= towerPrefabs.Length) return false;
        
        // 简化的建造逻辑，避免依赖已删除的TowerController
        int cost = 100 + towerIndex * 50; // 基础费用 + 索引加成
        if (playerGold >= cost)
        {
            playerGold -= cost;
            GameObject tower = Instantiate(towerPrefabs[towerIndex], position, Quaternion.identity);
            activeTowers.Add(tower);
            UpdateUI();
            Debug.Log($"防御塔建造成功！费用: {cost}");
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// 升级防御塔
    /// </summary>
    public bool UpgradeTower(GameObject tower)
    {
        // 简化的升级逻辑，避免依赖已删除的TowerController
        if (tower == null) return false;
        
        // 基础升级费用
        int upgradeCost = 100;
        if (playerGold >= upgradeCost)
        {
            playerGold -= upgradeCost;
            UpdateUI();
            Debug.Log("防御塔升级成功！");
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// 基地受到伤害
    /// </summary>
    public void TakeBaseDamage(int damage)
    {
        baseHealth -= damage;
        if (baseHealth <= 0)
        {
            baseHealth = 0;
            GameOver();
        }
        UpdateUI();
    }
    
    /// <summary>
    /// 敌人被消灭
    /// </summary>
    public void EnemyDefeated(GameObject enemy, int goldReward)
    {
        activeEnemies.Remove(enemy);
        playerGold += goldReward;
        UpdateUI();
        
        // 检查是否所有敌人都被消灭
        if (activeEnemies.Count == 0)
        {
            Invoke(nameof(StartNextWave), waveDelay);
        }
    }
    
    /// <summary>
    /// 游戏结束
    /// </summary>
    private void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        
        Time.timeScale = 0f;
    }
    
    /// <summary>
    /// 胜利
    /// </summary>
    private void Victory()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
        
        Time.timeScale = 0f;
    }
    
    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        
        // 清理所有对象
        foreach (GameObject obj in activeEnemies) Destroy(obj);
        foreach (GameObject obj in activeTowers) Destroy(obj);
        foreach (GameObject obj in activeResources) Destroy(obj);
        
        activeEnemies.Clear();
        activeTowers.Clear();
        activeResources.Clear();
        
        // 重置游戏状态
        playerGold = 1000;
        playerGems = 50;
        currentWave = 0;
        baseHealth = maxBaseHealth;
        
        UpdateUI();
        StartNextWave();
    }
    
    /// <summary>
    /// 获取当前敌人数量
    /// </summary>
    public int GetActiveEnemyCount()
    {
        return activeEnemies.Count;
    }
    
    /// <summary>
    /// 获取当前防御塔数量
    /// </summary>
    public int GetActiveTowerCount()
    {
        return activeTowers.Count;
    }
} 