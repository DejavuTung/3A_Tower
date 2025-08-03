using UnityEngine;

public class RestAnimationConditionalTest : MonoBehaviour
{
    [Header("测试设置")]
    public GameObject archerObject; // 弓箭手对象

    private ArcherAnimation archerAnimation;

    void Start()
    {
        if (archerObject == null)
        {
            Debug.LogError("RestAnimationConditionalTest: 请分配弓箭手对象");
            return;
        }

        archerAnimation = archerObject.GetComponent<ArcherAnimation>();
        if (archerAnimation == null)
        {
            Debug.LogError("RestAnimationConditionalTest: 弓箭手对象上没有ArcherAnimation组件");
            return;
        }

        Debug.Log("RestAnimationConditionalTest: 条件休息动画测试准备就绪");
    }

    [ContextMenu("测试条件休息动画逻辑")]
    public void TestConditionalRestAnimation()
    {
        if (archerObject == null)
        {
            Debug.LogError("RestAnimationConditionalTest: 请分配弓箭手对象");
            return;
        }

        Debug.Log("=== 开始条件休息动画测试 ===");
        Debug.Log("测试规则：");
        Debug.Log("1. 默认休息：使用Archer_rest_1_1/Archer_rest_1_2镜像");
        Debug.Log("2. 右下角射击后：使用Archer_rest_1_1/Archer_rest_1_2（不镜像）");
        Debug.Log("3. 左下角射击后：使用Archer_rest_1_1/Archer_rest_1_2镜像");
        Debug.Log("4. 右上角射击后：使用Archer_rest_2_1/Archer_rest_2_2（不镜像）");
        Debug.Log("5. 左上角射击后：使用Archer_rest_2_1/Archer_rest_2_2镜像");

        StartCoroutine(TestAllDirections());
    }

    private System.Collections.IEnumerator TestAllDirections()
    {
        Vector3[] testDirections = {
            new Vector3(1, -1, 0).normalized,   // 右下角
            new Vector3(-1, -1, 0).normalized,  // 左下角
            new Vector3(1, 1, 0).normalized,    // 右上角
            new Vector3(-1, 1, 0).normalized    // 左上角
        };

        string[] directionNames = { "右下角", "左下角", "右上角", "左上角" };
        string[] expectedResults = { 
            "rest1组，不镜像", 
            "rest1组，镜像", 
            "rest2组，不镜像", 
            "rest2组，镜像" 
        };

        for (int i = 0; i < testDirections.Length; i++)
        {
            Debug.Log($"\n--- 测试 {directionNames[i]} 射击 ---");
            Debug.Log($"期望的休息动画: {expectedResults[i]}");

            // 触发射击动画
            archerAnimation.TriggerShootAnimation(testDirections[i]);

            // 等待射击动画完成
            yield return new WaitForSeconds(2f);

            Debug.Log($"射击完成，现在应该显示 {expectedResults[i]} 的休息动画");
            
            // 检查当前状态
            CheckCurrentStatus();

            // 等待一段时间观察休息动画
            yield return new WaitForSeconds(3f);
        }

        Debug.Log("=== 条件休息动画测试完成 ===");
    }

    [ContextMenu("检查当前状态")]
    public void CheckCurrentStatus()
    {
        if (archerObject == null)
        {
            Debug.LogError("RestAnimationConditionalTest: 请分配弓箭手对象");
            return;
        }

        Debug.Log("=== 当前状态检查 ===");
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
            
            // 使用反射获取私有字段来检查最后射击方向
            var lastShootDirection = archerAnimation.GetType().GetField("lastShootDirection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(archerAnimation) as Vector3?;
            var hasShotBefore = archerAnimation.GetType().GetField("hasShotBefore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(archerAnimation) as bool?;
            
            if (lastShootDirection.HasValue)
            {
                Debug.Log($"最后射击方向: {lastShootDirection.Value}");
                float angle = Mathf.Atan2(lastShootDirection.Value.y, lastShootDirection.Value.x) * Mathf.Rad2Deg;
                Debug.Log($"最后射击角度: {angle}°");
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

        Debug.Log("=== 状态检查完成 ===");
    }

    [ContextMenu("重置测试")]
    public void ResetTest()
    {
        if (archerObject == null)
        {
            Debug.LogError("RestAnimationConditionalTest: 请分配弓箭手对象");
            return;
        }

        Debug.Log("=== 重置测试 ===");
        
        // 重新获取组件
        archerAnimation = archerObject.GetComponent<ArcherAnimation>();
        if (archerAnimation != null)
        {
            Debug.Log("✓ ArcherAnimation组件重新获取成功");
        }
        else
        {
            Debug.LogError("✗ ArcherAnimation组件获取失败");
        }

        Debug.Log("=== 重置完成 ===");
    }
} 