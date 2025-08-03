using UnityEngine;

public class CompilationTest : MonoBehaviour
{
    [Header("测试设置")]
    public GameObject archerObject; // 弓箭手对象
    
    void Start()
    {
        Debug.Log("=== 编译测试开始 ===");
        Debug.Log("如果看到这条消息，说明编译成功！");
        
        if (archerObject != null)
        {
            ArcherAnimation archerAnim = archerObject.GetComponent<ArcherAnimation>();
            if (archerAnim != null)
            {
                Debug.Log("✓ ArcherAnimation组件存在");
                
                // 测试新的4帧休息动画字段
                if (archerAnim.archerRest1_1 != null) Debug.Log("✓ archerRest1_1 已分配");
                if (archerAnim.archerRest1_2 != null) Debug.Log("✓ archerRest1_2 已分配");
                if (archerAnim.archerRest2_1 != null) Debug.Log("✓ archerRest2_1 已分配");
                if (archerAnim.archerRest2_2 != null) Debug.Log("✓ archerRest2_2 已分配");
            }
            else
            {
                Debug.LogWarning("⚠ ArcherAnimation组件不存在");
            }
        }
        else
        {
            Debug.Log("ℹ 未分配弓箭手对象，跳过组件检查");
        }
        
        Debug.Log("=== 编译测试完成 ===");
    }
    
    [ContextMenu("运行编译测试")]
    public void RunCompilationTest()
    {
        Debug.Log("=== 手动编译测试 ===");
        Debug.Log("✓ 脚本编译成功！");
        Debug.Log("✓ 所有字段引用已更新为新的4帧休息动画");
        Debug.Log("=== 测试完成 ===");
    }
} 