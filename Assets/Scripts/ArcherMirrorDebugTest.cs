using UnityEngine;

public class ArcherMirrorDebugTest : MonoBehaviour
{
    [Header("测试设置")]
    public GameObject archerObject; // 弓箭手对象
    public float testInterval = 3f; // 测试间隔
    
    private ArcherAnimation archerAnimation;
    private float nextTestTime;
    private int testDirection = 0;
    
    // 测试方向数组
    private Vector3[] testDirections = {
        new Vector3(1, -1, 0).normalized,   // 右下角
        new Vector3(1, 1, 0).normalized,    // 右上角
        new Vector3(-1, 1, 0).normalized,   // 左上角
        new Vector3(-1, -1, 0).normalized   // 左下角
    };
    
    private string[] directionNames = { "右下角", "右上角", "左上角", "左下角" };
    
    void Start()
    {
        if (archerObject == null)
        {
            Debug.LogError("ArcherMirrorDebugTest: 请分配弓箭手对象");
            return;
        }
        
        archerAnimation = archerObject.GetComponent<ArcherAnimation>();
        if (archerAnimation == null)
        {
            Debug.LogError("ArcherMirrorDebugTest: 弓箭手对象上没有ArcherAnimation组件");
            return;
        }
        
        nextTestTime = Time.time + testInterval;
        Debug.Log("ArcherMirrorDebugTest: 详细镜像测试开始");
        
        // 输出初始状态
        Debug.Log($"ArcherMirrorDebugTest: 弓箭手初始位置: {archerObject.transform.position}");
        Debug.Log($"ArcherMirrorDebugTest: 弓箭手初始缩放: {archerObject.transform.localScale}");
    }
    
    void Update()
    {
        if (archerAnimation == null) return;
        
        if (Time.time >= nextTestTime)
        {
            TestMirrorFunction();
            nextTestTime = Time.time + testInterval;
        }
    }
    
    void TestMirrorFunction()
    {
        Vector3 direction = testDirections[testDirection];
        string directionName = directionNames[testDirection];
        
        // 计算角度用于验证
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Debug.Log($"=== 测试 {directionName} 射击 ===");
        Debug.Log($"ArcherMirrorDebugTest: 测试方向: {direction}");
        Debug.Log($"ArcherMirrorDebugTest: 计算角度: {angle}°");
        Debug.Log($"ArcherMirrorDebugTest: 弓箭手当前缩放: {archerObject.transform.localScale}");
        
        // 触发射击动画
        archerAnimation.TriggerShootAnimation(direction);
        
        // 切换到下一个测试方向
        testDirection = (testDirection + 1) % 4;
        
        Debug.Log($"ArcherMirrorDebugTest: 切换到下一个测试方向: {directionNames[testDirection]}");
        Debug.Log("=== 测试结束 ===\n");
    }
    
    // 手动测试特定方向
    [ContextMenu("测试右下角")]
    public void TestBottomRight()
    {
        TestSpecificDirection(0, "右下角");
    }
    
    [ContextMenu("测试右上角")]
    public void TestTopRight()
    {
        TestSpecificDirection(1, "右上角");
    }
    
    [ContextMenu("测试左上角")]
    public void TestTopLeft()
    {
        TestSpecificDirection(2, "左上角");
    }
    
    [ContextMenu("测试左下角")]
    public void TestBottomLeft()
    {
        TestSpecificDirection(3, "左下角");
    }
    
    private void TestSpecificDirection(int index, string name)
    {
        if (archerAnimation == null)
        {
            Debug.LogError("ArcherMirrorDebugTest: ArcherAnimation组件未找到");
            return;
        }
        
        Vector3 direction = testDirections[index];
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Debug.Log($"=== 手动测试 {name} ===");
        Debug.Log($"方向: {direction}, 角度: {angle}°");
        Debug.Log($"当前缩放: {archerObject.transform.localScale}");
        
        archerAnimation.TriggerShootAnimation(direction);
        
        Debug.Log("=== 手动测试结束 ===");
    }
} 