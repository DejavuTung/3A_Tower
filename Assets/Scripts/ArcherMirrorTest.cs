using UnityEngine;

public class ArcherMirrorTest : MonoBehaviour
{
    [Header("测试设置")]
    public GameObject archerObject; // 弓箭手对象
    public float testInterval = 2f; // 测试间隔
    
    private ArcherAnimation archerAnimation;
    private float nextTestTime;
    private int testDirection = 0; // 0:右下, 1:右上, 2:左上, 3:左下
    
    void Start()
    {
        if (archerObject == null)
        {
            Debug.LogError("ArcherMirrorTest: 请分配弓箭手对象");
            return;
        }
        
        archerAnimation = archerObject.GetComponent<ArcherAnimation>();
        if (archerAnimation == null)
        {
            Debug.LogError("ArcherMirrorTest: 弓箭手对象上没有ArcherAnimation组件");
            return;
        }
        
        nextTestTime = Time.time + testInterval;
        Debug.Log("ArcherMirrorTest: 镜像测试开始");
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
        Vector3[] testDirections = {
            new Vector3(1, -1, 0).normalized,   // 右下角
            new Vector3(1, 1, 0).normalized,    // 右上角
            new Vector3(-1, 1, 0).normalized,   // 左上角
            new Vector3(-1, -1, 0).normalized   // 左下角
        };
        
        string[] directionNames = { "右下角", "右上角", "左上角", "左下角" };
        
        Vector3 direction = testDirections[testDirection];
        string directionName = directionNames[testDirection];
        
        Debug.Log($"ArcherMirrorTest: 测试 {directionName} 射击，方向: {direction}");
        archerAnimation.TriggerShootAnimation(direction);
        
        // 切换到下一个测试方向
        testDirection = (testDirection + 1) % 4;
    }
} 