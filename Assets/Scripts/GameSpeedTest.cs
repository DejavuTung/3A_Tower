using UnityEngine;
using UnityEngine.UI;

public class GameSpeedTest : MonoBehaviour
{
    private AutoTowerDefenseDemo gameManager;

    void Start()
    {
        // 查找游戏管理器
        gameManager = FindFirstObjectByType<AutoTowerDefenseDemo>();
        if (gameManager == null)
        {
            Debug.LogError("GameSpeedTest: 未找到AutoTowerDefenseDemo组件");
            return;
        }

        Debug.Log("GameSpeedTest: 游戏速度测试准备就绪");
    }

    [ContextMenu("测试0.5X速度功能")]
    void Test05xSpeed()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameSpeedTest: 游戏管理器未找到");
            return;
        }

        Debug.Log("GameSpeedTest: 开始测试0.5X速度功能");

        // 通过反射调用SetGameSpeed方法
        var setGameSpeedMethod = typeof(AutoTowerDefenseDemo).GetMethod("SetGameSpeed", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (setGameSpeedMethod != null)
        {
            setGameSpeedMethod.Invoke(gameManager, new object[] { 0.5f });
            Debug.Log("GameSpeedTest: 已设置游戏速度为0.5X");
            Debug.Log("GameSpeedTest: 请检查：");
            Debug.Log("1. 0.5X按钮应该高亮显示（浅绿色）");
            Debug.Log("2. 其他速度按钮应该为正常颜色（浅灰色）");
            Debug.Log("3. 游戏运行速度应该变慢（敌人移动、攻击等）");
        }
        else
        {
            Debug.LogError("GameSpeedTest: 无法找到SetGameSpeed方法");
        }
    }

    [ContextMenu("测试所有速度按钮")]
    void TestAllSpeedButtons()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameSpeedTest: 游戏管理器未找到");
            return;
        }

        Debug.Log("GameSpeedTest: 开始测试所有速度按钮");

        // 查找所有速度按钮
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        int speedButtonCount = 0;
        
        foreach (Button button in buttons)
        {
            if (button.name.Contains("SpeedButton_"))
            {
                speedButtonCount++;
                Debug.Log($"GameSpeedTest: 发现速度按钮: {button.name}");
            }
        }

        Debug.Log($"GameSpeedTest: 总共发现 {speedButtonCount} 个速度按钮");
        
        if (speedButtonCount == 5)
        {
            Debug.Log("GameSpeedTest: ✓ 速度按钮数量正确（应该有5个：0.5X, 1X, 2X, 3X, 4X）");
        }
        else
        {
            Debug.LogWarning($"GameSpeedTest: ✗ 速度按钮数量不正确，期望5个，实际{speedButtonCount}个");
        }
    }

    [ContextMenu("测试速度按钮布局")]
    void TestSpeedButtonLayout()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameSpeedTest: 游戏管理器未找到");
            return;
        }

        Debug.Log("GameSpeedTest: 检查速度按钮布局");

        // 查找速度控制容器
        GameObject speedContainer = GameObject.Find("SpeedControlContainer");
        if (speedContainer != null)
        {
            RectTransform containerRect = speedContainer.GetComponent<RectTransform>();
            Debug.Log($"GameSpeedTest: 速度控制容器大小: {containerRect.sizeDelta}");
            Debug.Log($"GameSpeedTest: 速度控制容器位置: {containerRect.anchoredPosition}");
            
            if (containerRect.sizeDelta.x >= 120f)
            {
                Debug.Log("GameSpeedTest: ✓ 容器宽度足够容纳5个按钮");
            }
            else
            {
                Debug.LogWarning("GameSpeedTest: ✗ 容器宽度可能不足以容纳5个按钮");
            }
        }
        else
        {
            Debug.LogWarning("GameSpeedTest: ✗ 未找到速度控制容器");
        }
    }

    [ContextMenu("测试游戏速度效果")]
    void TestGameSpeedEffect()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameSpeedTest: 游戏管理器未找到");
            return;
        }

        Debug.Log("GameSpeedTest: 测试游戏速度效果");

        // 通过反射获取gameSpeed字段
        var gameSpeedField = typeof(AutoTowerDefenseDemo).GetField("gameSpeed", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (gameSpeedField != null)
        {
            float currentSpeed = (float)gameSpeedField.GetValue(gameManager);
            Debug.Log($"GameSpeedTest: 当前游戏速度: {currentSpeed}X");
            
            if (currentSpeed == 0.5f)
            {
                Debug.Log("GameSpeedTest: ✓ 游戏速度已设置为0.5X");
            }
            else
            {
                Debug.LogWarning($"GameSpeedTest: ✗ 游戏速度不是0.5X，当前为{currentSpeed}X");
            }
        }
        else
        {
            Debug.LogError("GameSpeedTest: 无法找到gameSpeed字段");
        }
    }

    [ContextMenu("清除测试环境")]
    void ClearTestEnvironment()
    {
        Debug.Log("GameSpeedTest: 清除测试环境");

        // 重置游戏速度为1X
        if (gameManager != null)
        {
            var setGameSpeedMethod = typeof(AutoTowerDefenseDemo).GetMethod("SetGameSpeed", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (setGameSpeedMethod != null)
            {
                setGameSpeedMethod.Invoke(gameManager, new object[] { 1f });
                Debug.Log("GameSpeedTest: 游戏速度已重置为1X");
            }
        }

        Debug.Log("GameSpeedTest: 测试环境已清除");
    }
} 