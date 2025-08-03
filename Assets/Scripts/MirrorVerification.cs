using UnityEngine;

public class MirrorVerification : MonoBehaviour
{
    [Header("验证设置")]
    public GameObject testObject; // 要测试的对象
    
    void Start()
    {
        if (testObject == null)
        {
            Debug.LogError("MirrorVerification: 请分配测试对象");
            return;
        }
        
        Debug.Log($"MirrorVerification: 测试对象初始缩放: {testObject.transform.localScale}");
        
        // 测试镜像功能
        TestMirror();
    }
    
    void TestMirror()
    {
        Vector3 originalScale = testObject.transform.localScale;
        
        Debug.Log("=== 镜像测试开始 ===");
        Debug.Log($"原始缩放: {originalScale}");
        
        // 测试镜像
        Vector3 mirroredScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        testObject.transform.localScale = mirroredScale;
        
        Debug.Log($"镜像后缩放: {testObject.transform.localScale}");
        Debug.Log($"预期镜像缩放: {mirroredScale}");
        Debug.Log($"镜像是否生效: {testObject.transform.localScale == mirroredScale}");
        
        // 恢复原始缩放
        testObject.transform.localScale = originalScale;
        Debug.Log($"恢复后缩放: {testObject.transform.localScale}");
        
        Debug.Log("=== 镜像测试结束 ===");
    }
    
    [ContextMenu("手动测试镜像")]
    public void ManualTestMirror()
    {
        if (testObject == null)
        {
            Debug.LogError("MirrorVerification: 请分配测试对象");
            return;
        }
        
        Vector3 originalScale = testObject.transform.localScale;
        Vector3 mirroredScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        
        Debug.Log($"手动测试 - 原始: {originalScale}, 镜像: {mirroredScale}");
        
        testObject.transform.localScale = mirroredScale;
        
        Debug.Log($"手动测试 - 当前缩放: {testObject.transform.localScale}");
    }
    
    [ContextMenu("恢复原始缩放")]
    public void RestoreOriginalScale()
    {
        if (testObject == null)
        {
            Debug.LogError("MirrorVerification: 请分配测试对象");
            return;
        }
        
        Vector3 originalScale = new Vector3(1, 1, 1); // 假设原始缩放是(1,1,1)
        testObject.transform.localScale = originalScale;
        
        Debug.Log($"恢复原始缩放: {testObject.transform.localScale}");
    }
} 