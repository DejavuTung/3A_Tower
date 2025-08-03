using UnityEngine;

public class GameRestartTest : MonoBehaviour
{
    [Header("测试设置")]
    public GameObject archerObject; // 弓箭手对象

    private ArcherAnimation archerAnimation;
    private AutoTowerDefenseDemo gameManager;

    void Start()
    {
        if (archerObject == null)
        {
            Debug.LogError("GameRestartTest: 请分配弓箭手对象");
            return;
        }

        archerAnimation = archerObject.GetComponent<ArcherAnimation>();
        if (archerAnimation == null)
        {
            Debug.LogError("GameRestartTest: 弓箭手对象上没有ArcherAnimation组件");
            return;
        }

        gameManager = FindFirstObjectByType<AutoTowerDefenseDemo>();
        if (gameManager == null)
        {
            Debug.LogError("GameRestartTest: 未找到AutoTowerDefenseDemo组件");
            return;
        }

        Debug.Log("GameRestartTest: 游戏重新开始测试准备就绪");
    }

    [ContextMenu("测试游戏重新开始功能")]
    public void TestGameRestart()
    {
        if (archerObject == null)
        {
            Debug.LogError("GameRestartTest: 请分配弓箭手对象");
            return;
        }

        Debug.Log("=== 开始游戏重新开始测试 ===");
        Debug.Log("测试步骤：");
        Debug.Log("1. 检查Archer初始状态");
        Debug.Log("2. 触发几次射击");
        Debug.Log("3. 检查射击后的状态");
        Debug.Log("4. 模拟游戏重新开始");
        Debug.Log("5. 检查重新开始后的状态");

        StartCoroutine(TestRestartSequence());
    }

    private System.Collections.IEnumerator TestRestartSequence()
    {
        // 步骤1：检查Archer初始状态
        Debug.Log("\n--- 步骤1：检查Archer初始状态 ---");
        CheckArcherStatus("初始状态");

        // 步骤2：触发几次射击
        Debug.Log("\n--- 步骤2：触发几次射击 ---");
        Vector3[] testDirections = {
            new Vector3(1, -1, 0).normalized,   // 右下角
            new Vector3(-1, -1, 0).normalized,  // 左下角
            new Vector3(1, 1, 0).normalized,    // 右上角
        };

        string[] directionNames = { "右下角", "左下角", "右上角" };

        for (int i = 0; i < testDirections.Length; i++)
        {
            Debug.Log($"触发 {directionNames[i]} 射击");
            archerAnimation.TriggerShootAnimation(testDirections[i]);
            
            // 等待射击动画完成
            yield return new WaitForSeconds(2f);
            
            CheckArcherStatus($"射击 {directionNames[i]} 后");
            
            // 等待一段时间观察休息动画
            yield return new WaitForSeconds(2f);
        }

        // 步骤3：检查射击后的状态
        Debug.Log("\n--- 步骤3：检查射击后的状态 ---");
        CheckArcherStatus("多次射击后");

        // 步骤4：模拟游戏重新开始
        Debug.Log("\n--- 步骤4：模拟游戏重新开始 ---");
        Debug.Log("调用ClearArcherHistory()方法");
        
        // 使用反射调用私有方法
        var clearArcherHistoryMethod = gameManager.GetType().GetMethod("ClearArcherHistory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (clearArcherHistoryMethod != null)
        {
            clearArcherHistoryMethod.Invoke(gameManager, null);
            Debug.Log("ClearArcherHistory()方法调用成功");
        }
        else
        {
            Debug.LogWarning("未找到ClearArcherHistory()方法，直接调用ResetArcherState()");
            archerAnimation.ResetArcherState();
        }

        // 等待重置完成
        yield return new WaitForSeconds(1f);

        // 步骤5：检查重新开始后的状态
        Debug.Log("\n--- 步骤5：检查重新开始后的状态 ---");
        CheckArcherStatus("重新开始后");

        Debug.Log("=== 游戏重新开始测试完成 ===");
    }

    [ContextMenu("检查Archer状态")]
    public void CheckArcherStatus(string statusName = "当前状态")
    {
        if (archerObject == null)
        {
            Debug.LogError("GameRestartTest: 请分配弓箭手对象");
            return;
        }

        Debug.Log($"=== {statusName}检查 ===");
        Debug.Log($"Archer位置: {archerObject.transform.position}");
        Debug.Log($"Archer缩放: {archerObject.transform.localScale}");

        SpriteRenderer sr = archerObject.GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            Debug.Log($"当前Sprite: {sr.sprite.name}");
        }
        else
        {
            Debug.LogWarning("SpriteRenderer或Sprite为空");
        }

        if (archerAnimation != null)
        {
            Debug.Log("ArcherAnimation组件存在");
            
            // 使用反射获取私有字段来检查状态
            var lastShootDirection = archerAnimation.GetType().GetField("lastShootDirection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(archerAnimation) as Vector3?;
            var hasShotBefore = archerAnimation.GetType().GetField("hasShotBefore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(archerAnimation) as bool?;
            
            if (lastShootDirection.HasValue)
            {
                Debug.Log($"最后射击方向: {lastShootDirection.Value}");
                if (lastShootDirection.Value != Vector3.zero)
                {
                    float angle = Mathf.Atan2(lastShootDirection.Value.y, lastShootDirection.Value.x) * Mathf.Rad2Deg;
                    Debug.Log($"最后射击角度: {angle}°");
                }
            }
            
            if (hasShotBefore.HasValue)
            {
                Debug.Log($"是否已射击过: {hasShotBefore.Value}");
            }
        }
        else
        {
            Debug.LogError("ArcherAnimation组件不存在");
        }

        Debug.Log($"=== {statusName}检查完成 ===");
    }

    [ContextMenu("手动重置Archer状态")]
    public void ManualResetArcher()
    {
        if (archerAnimation != null)
        {
            Debug.Log("手动重置Archer状态");
            archerAnimation.ResetArcherState();
            CheckArcherStatus("手动重置后");
        }
        else
        {
            Debug.LogError("ArcherAnimation组件不存在");
        }
    }

    [ContextMenu("测试射击后重置")]
    public void TestShootAndReset()
    {
        if (archerAnimation == null)
        {
            Debug.LogError("ArcherAnimation组件不存在");
            return;
        }

        Debug.Log("=== 测试射击后重置 ===");
        
        // 先检查初始状态
        CheckArcherStatus("重置前");
        
        // 触发一次射击
        Debug.Log("触发右下角射击");
        archerAnimation.TriggerShootAnimation(new Vector3(1, -1, 0).normalized);
        
        // 等待射击完成
        StartCoroutine(WaitAndReset());
    }

    private System.Collections.IEnumerator WaitAndReset()
    {
        // 等待射击动画完成
        yield return new WaitForSeconds(2f);
        
        // 检查射击后状态
        CheckArcherStatus("射击后");
        
        // 重置状态
        Debug.Log("重置Archer状态");
        archerAnimation.ResetArcherState();
        
        // 等待重置完成
        yield return new WaitForSeconds(1f);
        
        // 检查重置后状态
        CheckArcherStatus("重置后");
        
        Debug.Log("=== 射击后重置测试完成 ===");
    }
} 