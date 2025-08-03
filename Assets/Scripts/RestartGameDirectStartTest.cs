using UnityEngine;
using UnityEngine.UI;

public class RestartGameDirectStartTest : MonoBehaviour
{
    private AutoTowerDefenseDemo gameManager;

    void Start()
    {
        // 查找游戏管理器
        gameManager = FindFirstObjectByType<AutoTowerDefenseDemo>();
        if (gameManager == null)
        {
            Debug.LogError("RestartGameDirectStartTest: 未找到AutoTowerDefenseDemo组件");
            return;
        }

        Debug.Log("RestartGameDirectStartTest: 重新开始游戏直接开始测试准备就绪");
    }

    [ContextMenu("测试重新开始游戏直接开始")]
    void TestRestartGameDirectStart()
    {
        if (gameManager == null)
        {
            Debug.LogError("RestartGameDirectStartTest: 游戏管理器未找到");
            return;
        }

        Debug.Log("RestartGameDirectStartTest: 开始测试重新开始游戏直接开始功能");

        // 通过反射调用RestartGame方法
        var restartGameMethod = typeof(AutoTowerDefenseDemo).GetMethod("RestartGame", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (restartGameMethod != null)
        {
            restartGameMethod.Invoke(gameManager, null);
            Debug.Log("RestartGameDirectStartTest: 已触发重新开始游戏，请检查：");
            Debug.Log("1. 游戏应该直接开始，不需要点击开始游戏按钮");
            Debug.Log("2. 游戏信息UI应该正常显示（金币、波次、基地血量、敌人数量）");
            Debug.Log("3. 基地和防御塔应该立即创建");
            Debug.Log("4. 敌人应该开始生成");
        }
        else
        {
            Debug.LogError("RestartGameDirectStartTest: 无法找到RestartGame方法");
        }
    }

    [ContextMenu("检查游戏状态")]
    void CheckGameState()
    {
        if (gameManager == null)
        {
            Debug.LogError("RestartGameDirectStartTest: 游戏管理器未找到");
            return;
        }

        Debug.Log("RestartGameDirectStartTest: 检查游戏状态");

        // 检查是否有开始游戏按钮
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        bool foundStartButton = false;
        foreach (Button button in buttons)
        {
            if (button.name.Contains("StartGameButton"))
            {
                foundStartButton = true;
                Debug.LogWarning("RestartGameDirectStartTest: ✗ 发现开始游戏按钮，重新开始后不应该有");
                break;
            }
        }

        if (!foundStartButton)
        {
            Debug.Log("RestartGameDirectStartTest: ✓ 未发现开始游戏按钮，符合预期");
        }

        // 检查游戏是否已开始
        var gameStartedField = typeof(AutoTowerDefenseDemo).GetField("gameStarted", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (gameStartedField != null)
        {
            bool gameStarted = (bool)gameStartedField.GetValue(gameManager);
            Debug.Log($"RestartGameDirectStartTest: 游戏开始状态: {gameStarted}");
            
            if (gameStarted)
            {
                Debug.Log("RestartGameDirectStartTest: ✓ 游戏已开始，符合预期");
            }
            else
            {
                Debug.LogWarning("RestartGameDirectStartTest: ✗ 游戏未开始，不符合预期");
            }
        }

        // 检查基地和防御塔是否存在
        GameObject baseObj = GameObject.Find("Base");
        GameObject towerObj = GameObject.Find("Tower");
        
        if (baseObj != null)
        {
            Debug.Log("RestartGameDirectStartTest: ✓ 基地已创建");
        }
        else
        {
            Debug.LogWarning("RestartGameDirectStartTest: ✗ 基地未创建");
        }

        if (towerObj != null)
        {
            Debug.Log("RestartGameDirectStartTest: ✓ 防御塔已创建");
        }
        else
        {
            Debug.LogWarning("RestartGameDirectStartTest: ✗ 防御塔未创建");
        }

        // 检查游戏信息UI
        Text topRightText = FindFirstObjectByType<Text>();
        if (topRightText != null && !string.IsNullOrEmpty(topRightText.text))
        {
            Debug.Log($"RestartGameDirectStartTest: ✓ 游戏信息UI正常显示: {topRightText.text}");
        }
        else
        {
            Debug.LogWarning("RestartGameDirectStartTest: ✗ 游戏信息UI未正常显示");
        }
    }

    [ContextMenu("清除测试环境")]
    void ClearTestEnvironment()
    {
        Debug.Log("RestartGameDirectStartTest: 清除测试环境");

        // 销毁所有Canvas
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (Canvas canvas in canvases)
        {
            DestroyImmediate(canvas.gameObject);
        }

        // 销毁基地和防御塔
        GameObject baseObj = GameObject.Find("Base");
        if (baseObj != null)
        {
            DestroyImmediate(baseObj);
        }

        GameObject towerObj = GameObject.Find("Tower");
        if (towerObj != null)
        {
            DestroyImmediate(towerObj);
        }

        Debug.Log("RestartGameDirectStartTest: 测试环境已清除");
    }
} 