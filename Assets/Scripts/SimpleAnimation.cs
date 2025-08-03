using UnityEngine;

public class SimpleAnimation : MonoBehaviour
{
    [Header("动画设置")]
    public Sprite[] animationFrames;  // 动画帧数组
    public float frameRate = 8f;      // 每秒帧数
    
    private SpriteRenderer spriteRenderer;
    private float frameTimer = 0f;
    private int currentFrame = 0;
    
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
        if (animationFrames.Length > 0)
        {
            spriteRenderer.sprite = animationFrames[0];
        }
    }
    
    void Update()
    {
        if (animationFrames.Length <= 1) return;
        
        // 获取当前游戏速度
        float currentGameSpeed = GetGameSpeed();
        
        // 根据游戏速度调整动画速度
        frameTimer += Time.deltaTime * currentGameSpeed;
        float frameInterval = 1f / frameRate;
        
        if (frameTimer >= frameInterval)
        {
            frameTimer = 0f;
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            spriteRenderer.sprite = animationFrames[currentFrame];
            // 确保动画帧也使用正确的颜色
            spriteRenderer.color = Color.white;
        }
    }
    
    // 设置动画帧
    public void SetAnimationFrames(Sprite[] frames)
    {
        animationFrames = frames;
        currentFrame = 0;
        
        // 确保spriteRenderer已初始化
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        if (frames != null && frames.Length > 0 && spriteRenderer != null)
        {
            spriteRenderer.sprite = frames[0];
            // 确保动画使用正确的颜色
            spriteRenderer.color = Color.white;
        }
    }
} 