using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 简单塔防游戏管理器
/// </summary>
public class SimpleTowerDefense : MonoBehaviour
{
    [Header("游戏设置")]
    public int playerGold = 1000;
    public int currentWave = 0;
    public int totalWaves = 10;
    
    [Header("UI引用")]
    public Text goldText;
    public Text waveText;
    
    [Header("游戏对象")]
    public GameObject mainBase;
    public GameObject spawnPoint;
    
    private void Start()
    {
        Debug.Log("简单塔防游戏启动！");
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        if (goldText != null)
            goldText.text = "金币: " + playerGold;
        
        if (waveText != null)
            waveText.text = "波次: " + currentWave + "/" + totalWaves;
    }
    
    public void AddGold(int amount)
    {
        playerGold += amount;
        UpdateUI();
        Debug.Log("获得金币: " + amount);
    }
    
    public void StartNextWave()
    {
        if (currentWave < totalWaves)
        {
            currentWave++;
            UpdateUI();
            Debug.Log("开始第 " + currentWave + " 波");
        }
        else
        {
            Debug.Log("游戏胜利！");
        }
    }
} 