using UnityEngine;

public class GameRootTest : MonoBehaviour
{
    void Start()
    {
        // 查找GameRoot对象
        GameObject gameRoot = GameObject.Find("GameRoot");
        
        if (gameRoot != null)
        {
            Debug.Log("✅ GameRoot found successfully!");
            Debug.Log($"GameRoot position: {gameRoot.transform.position}");
            
            // 检查是否有AutoTowerDefenseDemo脚本
            AutoTowerDefenseDemo towerDefense = gameRoot.GetComponent<AutoTowerDefenseDemo>();
            if (towerDefense != null)
            {
                Debug.Log("✅ AutoTowerDefenseDemo script found on GameRoot!");
            }
            else
            {
                Debug.Log("❌ AutoTowerDefenseDemo script not found on GameRoot!");
            }
        }
        else
        {
            Debug.Log("❌ GameRoot not found in scene!");
        }
    }
} 