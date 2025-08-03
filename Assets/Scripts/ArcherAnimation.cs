using UnityEngine;
using System.Collections;

public class ArcherAnimation : MonoBehaviour
{
    [Header("Archer休息状态图片")]
    public Sprite archerRest1_1;       // Archer休息状态图片1-1
    public Sprite archerRest1_2;       // Archer休息状态图片1-2
    public Sprite archerRest2_1;       // Archer休息状态图片2-1
    public Sprite archerRest2_2;       // Archer休息状态图片2-2
    
    [Header("Archer射击动画资源")]
    public Sprite archerShoot1_1;      // Archer射击动画1-1 (右下角)
    public Sprite archerShoot1_2;      // Archer射击动画1-2 (右下角)
    public Sprite archerShoot1_3;      // Archer射击动画1-3 (右下角)
    public Sprite archerShoot1_4;      // Archer射击动画1-4 (右下角)
    public Sprite archerShoot2_1;      // Archer射击动画2-1 (右上角)
    public Sprite archerShoot2_2;      // Archer射击动画2-2 (右上角)
    public Sprite archerShoot2_3;      // Archer射击动画2-3 (右上角)
    public Sprite archerShoot2_4;      // Archer射击动画2-4 (右上角)
    
    [Header("动画设置")]
    public float restAnimationSpeed = 1.0f; // 休息动画速度（秒/帧）
    public float shootAnimationSpeed = 0.1f; // 射击动画速度（秒/帧）
    
    private SpriteRenderer spriteRenderer;
    private float nextFrameTime;
    private int currentRestFrame = 0;  // 当前休息动画帧索引 (0-3)
    private bool isShooting = false;
    private Vector3 originalScale;
    
    // 新增：跟踪最后射击方向
    private Vector3 lastShootDirection = Vector3.zero;
    private bool hasShotBefore = false; // 是否已经射击过
    
    // 新增：获取游戏速度的方法
    private float GetGameSpeed()
    {
        AutoTowerDefenseDemo gameManager = FindFirstObjectByType<AutoTowerDefenseDemo>();
        if (gameManager != null)
        {
            var gameSpeedField = typeof(AutoTowerDefenseDemo).GetField("gameSpeed", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (gameSpeedField != null)
            {
                return (float)gameSpeedField.GetValue(gameManager);
            }
        }
        return 1f; // 默认1倍速
    }
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null)
        {
            Debug.LogError("ArcherAnimation: 未找到SpriteRenderer组件");
            return;
        }
        
        // 保存原始缩放值
        originalScale = transform.localScale;
        Debug.Log($"ArcherAnimation: 保存原始缩放值: {originalScale}");
        
        // 设置初始Sprite（默认使用镜像的rest1组）
        if (archerRest1_1 != null)
        {
            spriteRenderer.sprite = archerRest1_1;
            // 设置默认镜像状态（rest1组镜像）
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            Debug.Log("ArcherAnimation: 设置初始Sprite为archerRest1_1（镜像状态）");
        }
        else
        {
            Debug.LogWarning("ArcherAnimation: archerRest1_1未分配，使用当前Sprite");
        }
        
        nextFrameTime = Time.time + (restAnimationSpeed / GetGameSpeed());
        
        Debug.Log($"ArcherAnimation: 初始化完成 - 位置: {transform.position}, 尺寸: {transform.localScale}");
    }
    
    void Update()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("ArcherAnimation: spriteRenderer为空");
            return;
        }
        
        // 如果正在射击，不执行休息动画
        if (isShooting)
        {
            return;
        }
        
        if (archerRest1_1 == null || archerRest1_2 == null || archerRest2_1 == null || archerRest2_2 == null)
        {
            Debug.LogWarning("ArcherAnimation: 休息动画资源未完全分配，无法进行休息动画");
            return;
        }
        
        // 检查是否到了切换帧的时间
        if (Time.time >= nextFrameTime)
        {
            // 切换到下一帧
            currentRestFrame = (currentRestFrame + 1) % 4; // 循环播放4帧 (0-3)
            
            // 根据最后射击方向确定休息动画
            UpdateRestAnimation();
            
            // 设置下一帧时间
            nextFrameTime = Time.time + (restAnimationSpeed / GetGameSpeed());
        }
    }
    
    // 新增：根据最后射击方向更新休息动画
    private void UpdateRestAnimation()
    {
        if (!hasShotBefore)
        {
            // 默认休息动画：使用rest1组镜像
            SetRestAnimation(true, true); // rest1组，镜像
            return;
        }
        
        // 计算最后射击角度
        float lastAngle = Mathf.Atan2(lastShootDirection.y, lastShootDirection.x) * Mathf.Rad2Deg;
        Debug.Log($"ArcherAnimation: 最后射击角度: {lastAngle}°");
        
        bool useRest1Group = false;
        bool needMirror = false;
        
        // 根据最后射击角度确定休息动画
        if (lastAngle >= -90f && lastAngle <= 0f)
        {
            // 右下角 (-90° 到 0°) - 使用rest1组，不镜像
            useRest1Group = true;
            needMirror = false;
            Debug.Log("ArcherAnimation: 最后射击为右下角，使用rest1组，不镜像");
        }
        else if (lastAngle > 0f && lastAngle <= 90f)
        {
            // 右上角 (0° 到 90°) - 使用rest2组，不镜像
            useRest1Group = false;
            needMirror = false;
            Debug.Log("ArcherAnimation: 最后射击为右上角，使用rest2组，不镜像");
        }
        else if (lastAngle > 90f && lastAngle <= 180f)
        {
            // 左上角 (90° 到 180°) - 使用rest2组，镜像
            useRest1Group = false;
            needMirror = true;
            Debug.Log("ArcherAnimation: 最后射击为左上角，使用rest2组，镜像");
        }
        else if (lastAngle < -90f && lastAngle >= -180f)
        {
            // 左下角 (-180° 到 -90°) - 使用rest1组，镜像
            useRest1Group = true;
            needMirror = true;
            Debug.Log("ArcherAnimation: 最后射击为左下角，使用rest1组，镜像");
        }
        else
        {
            // 默认情况（angle = 180° 或 angle = -180°）
            useRest1Group = false;
            needMirror = true;
            Debug.Log("ArcherAnimation: 最后射击为默认方向，使用rest2组，镜像");
        }
        
        SetRestAnimation(useRest1Group, needMirror);
    }
    
    // 新增：设置休息动画
    private void SetRestAnimation(bool useRest1Group, bool needMirror)
    {
        // 设置镜像
        if (needMirror)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            Debug.Log($"ArcherAnimation: 休息动画设置镜像 - 缩放: {transform.localScale}");
        }
        else
        {
            transform.localScale = originalScale;
            Debug.Log($"ArcherAnimation: 休息动画不设置镜像 - 缩放: {originalScale}");
        }
        
        // 设置对应的Sprite
        if (useRest1Group)
        {
            // 使用rest1组
            switch (currentRestFrame)
            {
                case 0:
                    spriteRenderer.sprite = archerRest1_1;
                    Debug.Log("ArcherAnimation: 休息动画切换到archerRest1_1");
                    break;
                case 1:
                    spriteRenderer.sprite = archerRest1_2;
                    Debug.Log("ArcherAnimation: 休息动画切换到archerRest1_2");
                    break;
                case 2:
                    spriteRenderer.sprite = archerRest1_1;
                    Debug.Log("ArcherAnimation: 休息动画切换到archerRest1_1");
                    break;
                case 3:
                    spriteRenderer.sprite = archerRest1_2;
                    Debug.Log("ArcherAnimation: 休息动画切换到archerRest1_2");
                    break;
            }
        }
        else
        {
            // 使用rest2组
            switch (currentRestFrame)
            {
                case 0:
                    spriteRenderer.sprite = archerRest2_1;
                    Debug.Log("ArcherAnimation: 休息动画切换到archerRest2_1");
                    break;
                case 1:
                    spriteRenderer.sprite = archerRest2_2;
                    Debug.Log("ArcherAnimation: 休息动画切换到archerRest2_2");
                    break;
                case 2:
                    spriteRenderer.sprite = archerRest2_1;
                    Debug.Log("ArcherAnimation: 休息动画切换到archerRest2_1");
                    break;
                case 3:
                    spriteRenderer.sprite = archerRest2_2;
                    Debug.Log("ArcherAnimation: 休息动画切换到archerRest2_2");
                    break;
            }
        }
        
        // 强制更新变换
        transform.hasChanged = true;
    }
    
    // 触发射击动画
    public void TriggerShootAnimation(Vector3 shootDirection)
    {
        if (isShooting)
        {
            Debug.Log("ArcherAnimation: 正在射击中，忽略新的射击请求");
            return;
        }
        
        // 保存射击方向
        lastShootDirection = shootDirection;
        hasShotBefore = true;
        
        StartCoroutine(PlayShootAnimation(shootDirection));
    }
    
    // 播放射击动画协程
    private IEnumerator PlayShootAnimation(Vector3 shootDirection)
    {
        isShooting = true;
        Debug.Log($"ArcherAnimation: 开始射击动画，方向: {shootDirection}");
        
        // 计算射击角度
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        Debug.Log($"ArcherAnimation: 射击方向: {shootDirection}, 计算角度: {angle}°");
        
        // 确定使用哪组动画和是否需要镜像
        bool useShoot1 = false; // 默认使用shoot2组
        bool needMirror = false;
        
        // 根据角度确定动画组和镜像
        if (angle >= -90f && angle <= 0f)
        {
            // 右下角 (-90° 到 0°) - 使用shoot1组，不镜像
            useShoot1 = true;
            needMirror = false;
            Debug.Log("ArcherAnimation: 右下角射击，使用shoot1组，不镜像");
        }
        else if (angle > 0f && angle <= 90f)
        {
            // 右上角 (0° 到 90°) - 使用shoot2组，不镜像
            useShoot1 = false;
            needMirror = false;
            Debug.Log("ArcherAnimation: 右上角射击，使用shoot2组，不镜像");
        }
        else if (angle > 90f && angle <= 180f)
        {
            // 左上角 (90° 到 180°) - 使用shoot2组，镜像
            useShoot1 = false;
            needMirror = true;
            Debug.Log("ArcherAnimation: 左上角射击，使用shoot2组，镜像");
        }
        else if (angle < -90f && angle >= -180f)
        {
            // 左下角 (-180° 到 -90°) - 使用shoot1组，镜像
            useShoot1 = true;
            needMirror = true;
            Debug.Log("ArcherAnimation: 左下角射击，使用shoot1组，镜像");
        }
        else
        {
            // 默认情况（angle = 180° 或 angle = -180°）
            useShoot1 = false;
            needMirror = true;
            Debug.Log("ArcherAnimation: 默认射击，使用shoot2组，镜像");
        }
        
        Debug.Log($"ArcherAnimation: 最终决定 - useShoot1: {useShoot1}, needMirror: {needMirror}");
        
        // 设置镜像
        Debug.Log($"ArcherAnimation: 原始缩放: {originalScale}, 当前缩放: {transform.localScale}");
        
        if (needMirror)
        {
            Vector3 mirroredScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            transform.localScale = mirroredScale;
            Debug.Log($"ArcherAnimation: 设置镜像 - 新缩放: {mirroredScale}");
        }
        else
        {
            transform.localScale = originalScale;
            Debug.Log($"ArcherAnimation: 不设置镜像 - 恢复原始缩放: {originalScale}");
        }
        
        // 强制更新变换
        transform.hasChanged = true;
        
        // 播放射击动画
        Sprite[] shootSprites = useShoot1 ? 
            new Sprite[] { archerShoot1_1, archerShoot1_2, archerShoot1_3, archerShoot1_4 } :
            new Sprite[] { archerShoot2_1, archerShoot2_2, archerShoot2_3, archerShoot2_4 };
        
        Debug.Log($"ArcherAnimation: 使用动画组: {(useShoot1 ? "shoot1组" : "shoot2组")}");
        Debug.Log($"ArcherAnimation: 镜像状态: {(needMirror ? "启用镜像" : "不镜像")}");
        Debug.Log($"ArcherAnimation: 当前缩放: {transform.localScale}");
        
        // 检查是否有有效的射击精灵
        bool hasValidShootSprites = true;
        foreach (var sprite in shootSprites)
        {
            if (sprite == null)
            {
                hasValidShootSprites = false;
                break;
            }
        }
        
        if (!hasValidShootSprites)
        {
            Debug.LogWarning("ArcherAnimation: 射击精灵未完全分配，跳过射击动画");
            isShooting = false;
            yield break;
        }
        
        // 播放射击动画帧
        for (int i = 0; i < shootSprites.Length; i++)
        {
            spriteRenderer.sprite = shootSprites[i];
            Debug.Log($"ArcherAnimation: 播放射击帧 {i + 1}");
            yield return new WaitForSeconds(shootAnimationSpeed / GetGameSpeed());
        }
        
        // 射击动画结束，恢复休息状态
        isShooting = false;
        Debug.Log("ArcherAnimation: 射击动画结束，恢复休息状态");
        
        // 设置下一帧时间，避免立即切换
        nextFrameTime = Time.time + (restAnimationSpeed / GetGameSpeed());
    }
    
    // 验证镜像功能的公共方法
    [ContextMenu("测试镜像功能")]
    public void TestMirrorFunction()
    {
        Debug.Log("=== 开始镜像功能测试 ===");
        
        // 测试四个方向
        Vector3[] testDirections = {
            new Vector3(1, -1, 0).normalized,   // 右下角
            new Vector3(1, 1, 0).normalized,    // 右上角
            new Vector3(-1, 1, 0).normalized,   // 左上角
            new Vector3(-1, -1, 0).normalized   // 左下角
        };
        
        string[] directionNames = { "右下角", "右上角", "左上角", "左下角" };
        
        for (int i = 0; i < testDirections.Length; i++)
        {
            Debug.Log($"\n--- 测试 {directionNames[i]} ---");
            TriggerShootAnimation(testDirections[i]);
        }
        
        Debug.Log("=== 镜像功能测试完成 ===");
    }
    
    // 新增：重置Archer状态（用于游戏重新开始时）
    public void ResetArcherState()
    {
        Debug.Log("ArcherAnimation: 重置Archer状态");
        
        // 重置射击历史
        lastShootDirection = Vector3.zero;
        hasShotBefore = false;
        
        // 重置为默认休息状态（镜像的rest1组）
        currentRestFrame = 0;
        if (archerRest1_1 != null)
        {
            spriteRenderer.sprite = archerRest1_1;
        }
        
        // 设置默认镜像状态
        transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        
        // 重置动画状态
        isShooting = false;
        nextFrameTime = Time.time + (restAnimationSpeed / GetGameSpeed());
        
        Debug.Log("ArcherAnimation: Archer状态已重置为默认状态");
    }
    
    // 新增：测试休息动画条件逻辑
    [ContextMenu("测试休息动画条件逻辑")]
    public void TestRestAnimationLogic()
    {
        Debug.Log("=== 开始休息动画条件逻辑测试 ===");
        
        // 测试四个方向的射击，然后观察休息动画
        Vector3[] testDirections = {
            new Vector3(1, -1, 0).normalized,   // 右下角 - 应该使用rest1组，不镜像
            new Vector3(1, 1, 0).normalized,    // 右上角 - 应该使用rest2组，不镜像
            new Vector3(-1, 1, 0).normalized,   // 左上角 - 应该使用rest2组，镜像
            new Vector3(-1, -1, 0).normalized   // 左下角 - 应该使用rest1组，镜像
        };
        
        string[] directionNames = { "右下角", "右上角", "左上角", "左下角" };
        string[] expectedResults = { 
            "rest1组，不镜像", 
            "rest2组，不镜像", 
            "rest2组，镜像", 
            "rest1组，镜像" 
        };
        
        StartCoroutine(TestRestAnimationSequence(testDirections, directionNames, expectedResults));
    }
    
    private IEnumerator TestRestAnimationSequence(Vector3[] directions, string[] directionNames, string[] expectedResults)
    {
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.Log($"\n--- 测试 {directionNames[i]} 射击 ---");
            Debug.Log($"期望的休息动画: {expectedResults[i]}");
            
            TriggerShootAnimation(directions[i]);
            
            // 等待射击动画完成
            yield return new WaitForSeconds(2f / GetGameSpeed());
            
            Debug.Log($"射击完成，现在应该显示 {expectedResults[i]} 的休息动画");
            Debug.Log($"当前缩放: {transform.localScale}");
            Debug.Log($"当前Sprite: {spriteRenderer.sprite?.name}");
            
            // 等待一段时间观察休息动画
            yield return new WaitForSeconds(3f / GetGameSpeed());
        }
        
        Debug.Log("=== 休息动画条件逻辑测试完成 ===");
    }
} 