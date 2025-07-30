using UnityEngine;

public class SimpleGameRootTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== GameRoot Test Starting ===");
        
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
        }
        else
        {
            Debug.Log("❌ GameRoot not found in scene!");
            
            // 列出所有根对象
            var allObjects = FindObjectsOfType<GameObject>();
            Debug.Log($"Found {allObjects.Length} GameObjects in scene:");
            foreach (var obj in allObjects)
            {
                if (obj.transform.parent == null)
                {
                    Debug.Log($"  Root: {obj.name}");
                }
            }
        }
        
        Debug.Log("=== GameRoot Test Complete ===");
    }
} 