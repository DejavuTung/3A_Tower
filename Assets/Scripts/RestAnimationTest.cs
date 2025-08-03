using UnityEngine;

public class RestAnimationTest : MonoBehaviour
{
    [Header("测试设置")]
    public GameObject archerObject; // 弓箭手对象
    
    private ArcherAnimation archerAnimation;
    
    void Start()
    {
        if (archerObject == null)
        {
            Debug.LogError("RestAnimationTest: 请分配弓箭手对象");
            return;
        }
        
        archerAnimation = archerObject.GetComponent<ArcherAnimation>();
        if (archerAnimation == null)
        {
            Debug.LogError("RestAnimationTest: 弓箭手对象上没有ArcherAnimation组件");
            return;
        }
        
        Debug.Log("RestAnimationTest: 休息动画测试准备就绪");
    }
    
    [ContextMenu("检查休息动画资源")]
    public void CheckRestAnimationResources()
    {
        if (archerAnimation == null)
        {
            Debug.LogError("RestAnimationTest: ArcherAnimation组件未找到");
            return;
        }
        
        Debug.Log("=== 检查休息动画资源 ===");
        
        // 使用反射获取私有字段来检查资源
        var rest1_1 = archerAnimation.GetType().GetField("archerRest1_1", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)?.GetValue(archerAnimation) as Sprite;
        var rest1_2 = archerAnimation.GetType().GetField("archerRest1_2", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)?.GetValue(archerAnimation) as Sprite;
        var rest2_1 = archerAnimation.GetType().GetField("archerRest2_1", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)?.GetValue(archerAnimation) as Sprite;
        var rest2_2 = archerAnimation.GetType().GetField("archerRest2_2", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)?.GetValue(archerAnimation) as Sprite;
        
        Debug.Log($"archerRest1_1: {(rest1_1 != null ? rest1_1.name : "未分配")}");
        Debug.Log($"archerRest1_2: {(rest1_2 != null ? rest1_2.name : "未分配")}");
        Debug.Log($"archerRest2_1: {(rest2_1 != null ? rest2_1.name : "未分配")}");
        Debug.Log($"archerRest2_2: {(rest2_2 != null ? rest2_2.name : "未分配")}");
        
        if (rest1_1 != null && rest1_2 != null && rest2_1 != null && rest2_2 != null)
        {
            Debug.Log("✓ 所有休息动画资源都已分配");
        }
        else
        {
            Debug.LogWarning("⚠ 部分休息动画资源未分配");
        }
        
        Debug.Log("=== 检查完成 ===");
    }
    
    [ContextMenu("测试休息动画")]
    public void TestRestAnimation()
    {
        if (archerObject == null)
        {
            Debug.LogError("RestAnimationTest: 请分配弓箭手对象");
            return;
        }
        
        Debug.Log("=== 开始休息动画测试 ===");
        Debug.Log("请观察弓箭手的休息动画是否按以下顺序循环播放：");
        Debug.Log("1. archerRest1_1");
        Debug.Log("2. archerRest1_2");
        Debug.Log("3. archerRest2_1");
        Debug.Log("4. archerRest2_2");
        Debug.Log("然后回到 archerRest1_1，如此循环...");
        
        // 检查当前Sprite
        SpriteRenderer sr = archerObject.GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            Debug.Log($"当前显示的Sprite: {sr.sprite.name}");
        }
        
        Debug.Log("=== 休息动画测试开始 ===");
    }
    
    [ContextMenu("检查Archer状态")]
    public void CheckArcherStatus()
    {
        if (archerObject == null)
        {
            Debug.LogError("RestAnimationTest: 请分配弓箭手对象");
            return;
        }
        
        Debug.Log("=== Archer状态检查 ===");
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
        }
        else
        {
            Debug.LogError("ArcherAnimation组件不存在");
        }
        
        Debug.Log("=== Archer状态检查完成 ===");
    }
} 