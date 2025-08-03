using UnityEngine;

public class LeftBottomMirrorTest : MonoBehaviour
{
    [Header("测试设置")]
    public GameObject archerObject; // 弓箭手对象
    
    private ArcherAnimation archerAnimation;
    
    void Start()
    {
        if (archerObject == null)
        {
            Debug.LogError("LeftBottomMirrorTest: 请分配弓箭手对象");
            return;
        }
        
        archerAnimation = archerObject.GetComponent<ArcherAnimation>();
        if (archerAnimation == null)
        {
            Debug.LogError("LeftBottomMirrorTest: 弓箭手对象上没有ArcherAnimation组件");
            return;
        }
        
        Debug.Log("LeftBottomMirrorTest: 左下角镜像测试准备就绪");
    }
    
    [ContextMenu("测试左下角射击镜像")]
    public void TestLeftBottomMirror()
    {
        if (archerAnimation == null)
        {
            Debug.LogError("LeftBottomMirrorTest: ArcherAnimation组件未找到");
            return;
        }
        
        Debug.Log("=== 开始左下角射击镜像测试 ===");
        
        // 记录测试前的状态
        Vector3 beforeScale = archerObject.transform.localScale;
        Debug.Log($"测试前缩放: {beforeScale}");
        
        // 测试左下角射击
        Vector3 leftBottomDirection = new Vector3(-1, -1, 0).normalized;
        float angle = Mathf.Atan2(leftBottomDirection.y, leftBottomDirection.x) * Mathf.Rad2Deg;
        
        Debug.Log($"左下角方向: {leftBottomDirection}");
        Debug.Log($"计算角度: {angle}°");
        Debug.Log($"预期结果: 使用shoot1组 + 镜像");
        
        // 触发射击动画
        archerAnimation.TriggerShootAnimation(leftBottomDirection);
        
        Debug.Log("=== 左下角射击镜像测试完成 ===");
    }
    
    [ContextMenu("测试四个方向对比")]
    public void TestAllDirections()
    {
        if (archerAnimation == null)
        {
            Debug.LogError("LeftBottomMirrorTest: ArcherAnimation组件未找到");
            return;
        }
        
        Debug.Log("=== 开始四个方向对比测试 ===");
        
        Vector3[] directions = {
            new Vector3(1, -1, 0).normalized,   // 右下角
            new Vector3(1, 1, 0).normalized,    // 右上角
            new Vector3(-1, 1, 0).normalized,   // 左上角
            new Vector3(-1, -1, 0).normalized   // 左下角
        };
        
        string[] directionNames = { "右下角", "右上角", "左上角", "左下角" };
        string[] expectedResults = { "shoot1组+不镜像", "shoot2组+不镜像", "shoot2组+镜像", "shoot1组+镜像" };
        
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.Log($"\n--- 测试 {directionNames[i]} ---");
            Debug.Log($"方向: {directions[i]}");
            Debug.Log($"预期: {expectedResults[i]}");
            
            float angle = Mathf.Atan2(directions[i].y, directions[i].x) * Mathf.Rad2Deg;
            Debug.Log($"角度: {angle}°");
            
            // 记录射击前的缩放
            Vector3 beforeScale = archerObject.transform.localScale;
            Debug.Log($"射击前缩放: {beforeScale}");
            
            // 触发射击
            archerAnimation.TriggerShootAnimation(directions[i]);
        }
        
        Debug.Log("=== 四个方向对比测试完成 ===");
    }
    
    [ContextMenu("验证镜像是否生效")]
    public void VerifyMirrorEffect()
    {
        if (archerObject == null)
        {
            Debug.LogError("LeftBottomMirrorTest: 请分配弓箭手对象");
            return;
        }
        
        Debug.Log("=== 验证镜像效果 ===");
        
        Vector3 originalScale = archerObject.transform.localScale;
        Debug.Log($"原始缩放: {originalScale}");
        
        // 测试镜像设置
        Vector3 mirroredScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        archerObject.transform.localScale = mirroredScale;
        
        Debug.Log($"镜像后缩放: {archerObject.transform.localScale}");
        Debug.Log($"预期镜像缩放: {mirroredScale}");
        Debug.Log($"镜像是否生效: {archerObject.transform.localScale == mirroredScale}");
        
        // 恢复原始缩放
        archerObject.transform.localScale = originalScale;
        Debug.Log($"恢复后缩放: {archerObject.transform.localScale}");
        
        Debug.Log("=== 镜像效果验证完成 ===");
    }
} 