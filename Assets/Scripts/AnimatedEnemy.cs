using UnityEngine;
using System.Collections;

public class AnimatedEnemy : MonoBehaviour
{
    [Header("动画设置")]
    public Sprite[] walkSprites;     // 行走动画帧
    public float animationSpeed = 0.2f;  // 动画速度
    
    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float animationTimer = 0f;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (walkSprites.Length > 0)
        {
            spriteRenderer.sprite = walkSprites[0];
        }
    }
    
    void Update()
    {
        // 播放行走动画
        if (walkSprites.Length > 1)
        {
            animationTimer += Time.deltaTime;
            if (animationTimer >= animationSpeed)
            {
                animationTimer = 0f;
                currentFrame = (currentFrame + 1) % walkSprites.Length;
                spriteRenderer.sprite = walkSprites[currentFrame];
            }
        }
    }
} 