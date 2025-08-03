using UnityEngine;

public class GameRootVerification : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== GameRoot Verification Starting ===");
        
        // 查找GameRoot对象
        GameObject gameRoot = GameObject.Find("GameRoot");
        
        if (gameRoot != null)
        {
            Debug.Log("✅ GameRoot found successfully!");
            Debug.Log($"GameRoot position: {gameRoot.transform.position}");
            Debug.Log($"GameRoot active: {gameRoot.activeInHierarchy}");
            
            // 检查组件
            var components = gameRoot.GetComponents<Component>();
            Debug.Log($"GameRoot has {components.Length} components:");
            foreach (var comp in components)
            {
                Debug.Log($"  - {comp.GetType().Name}");
            }
            
            // 检查AutoTowerDefenseDemo脚本
            var towerDefense = gameRoot.GetComponent<AutoTowerDefenseDemo>();
            if (towerDefense != null)
            {
                Debug.Log("✅ AutoTowerDefenseDemo script found!");
                Debug.Log($"Tower sprite: {towerDefense.towerSprite}");
                Debug.Log($"Enemy walk sprites count: {towerDefense.enemyWalkSprites.Length}");
            }
            else
            {
                Debug.Log("❌ AutoTowerDefenseDemo script not found!");
            }
        }
        else
        {
            Debug.Log("❌ GameRoot not found in scene!");
            
            // 列出所有根对象
            var allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            Debug.Log($"Found {allObjects.Length} GameObjects in scene:");
            foreach (var obj in allObjects)
            {
                if (obj.transform.parent == null)
                {
                    Debug.Log($"  Root: {obj.name}");
                }
            }
        }
        
        Debug.Log("=== GameRoot Verification Complete ===");
    }
} 