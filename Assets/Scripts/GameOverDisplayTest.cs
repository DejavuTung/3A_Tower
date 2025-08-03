using UnityEngine;
using UnityEngine.UI;

public class GameOverDisplayTest : MonoBehaviour
{
    private AutoTowerDefenseDemo gameManager;

    void Start()
    {
        // 查找游戏管理器
        gameManager = FindFirstObjectByType<AutoTowerDefenseDemo>();
        if (gameManager == null)
        {
            Debug.LogError("GameOverDisplayTest: 未找到AutoTowerDefenseDemo组件");
            return;
        }

        Debug.Log("GameOverDisplayTest: 游戏结束显示测试准备就绪");
    }

    [ContextMenu("测试游戏结束显示")]
    void TestGameOverDisplay()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameOverDisplayTest: 游戏管理器未找到");
            return;
        }

        Debug.Log("GameOverDisplayTest: 开始测试游戏结束显示功能");

        // 模拟游戏结束
        // 通过反射调用GameOver方法
        var gameOverMethod = typeof(AutoTowerDefenseDemo).GetMethod("GameOver", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (gameOverMethod != null)
        {
            gameOverMethod.Invoke(gameManager, null);
            Debug.Log("GameOverDisplayTest: 已触发游戏结束，请检查：");
            Debug.Log("1. 游戏信息UI应该被隐藏（不显示金币、波次、基地血量、敌人数量）");
            Debug.Log("2. 游戏结束文字应该只显示红色字体");
            Debug.Log("3. 再来一次按钮应该正常显示");
        }
        else
        {
            Debug.LogError("GameOverDisplayTest: 无法找到GameOver方法");
        }
    }

    [ContextMenu("检查游戏结束显示状态")]
    void CheckGameOverDisplayStatus()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameOverDisplayTest: 游戏管理器未找到");
            return;
        }

        Debug.Log("GameOverDisplayTest: 检查游戏结束显示状态");

        // 查找游戏结束Canvas
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        bool foundGameOverCanvas = false;
        bool foundGameOverText = false;
        bool foundRestartButton = false;

        foreach (Canvas canvas in canvases)
        {
            if (canvas.name == "GameOverCanvas")
            {
                foundGameOverCanvas = true;
                Debug.Log("GameOverDisplayTest: 找到游戏结束Canvas");

                // 检查游戏结束文字
                Text[] texts = canvas.GetComponentsInChildren<Text>();
                foreach (Text text in texts)
                {
                    if (text.name == "GameOverText")
                    {
                        foundGameOverText = true;
                        Debug.Log($"GameOverDisplayTest: 游戏结束文字内容: {text.text}");
                        Debug.Log($"GameOverDisplayTest: 游戏结束文字颜色: {text.color}");
                        Debug.Log($"GameOverDisplayTest: 游戏结束文字字体大小: {text.fontSize}");
                    }
                }

                // 检查再来一次按钮
                Button[] buttons = canvas.GetComponentsInChildren<Button>();
                foreach (Button button in buttons)
                {
                    if (button.name == "RestartButton")
                    {
                        foundRestartButton = true;
                        Debug.Log("GameOverDisplayTest: 找到再来一次按钮");
                    }
                }
            }
        }

        if (!foundGameOverCanvas)
        {
            Debug.Log("GameOverDisplayTest: 未找到游戏结束Canvas，可能游戏还未结束");
        }

        if (!foundGameOverText)
        {
            Debug.Log("GameOverDisplayTest: 未找到游戏结束文字");
        }

        if (!foundRestartButton)
        {
            Debug.Log("GameOverDisplayTest: 未找到再来一次按钮");
        }

        // 检查游戏信息UI是否被隐藏
        Text topRightText = FindFirstObjectByType<Text>();
        if (topRightText != null && topRightText.name.Contains("TopRightText"))
        {
            Debug.Log($"GameOverDisplayTest: 游戏信息UI内容: '{topRightText.text}'");
            if (string.IsNullOrEmpty(topRightText.text))
            {
                Debug.Log("GameOverDisplayTest: ✓ 游戏信息UI已正确隐藏");
            }
            else
            {
                Debug.LogWarning("GameOverDisplayTest: ✗ 游戏信息UI未被隐藏");
            }
        }
    }

    [ContextMenu("清除游戏结束显示")]
    void ClearGameOverDisplay()
    {
        Debug.Log("GameOverDisplayTest: 清除游戏结束显示");

        // 查找并销毁游戏结束Canvas
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (Canvas canvas in canvases)
        {
            if (canvas.name == "GameOverCanvas")
            {
                DestroyImmediate(canvas.gameObject);
                Debug.Log("GameOverDisplayTest: 已清除游戏结束Canvas");
            }
        }
    }
} 