using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public GameObject healthBarCanvas;
    public Component healthBarFill; // 改为Component以支持SpriteRenderer
    public Text healthText;
    
    private EnemyHealth enemyHealth;
    private Camera mainCamera;
    
    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        mainCamera = Camera.main;
        
        Debug.Log($"EnemyHealthBar Start: enemyHealth={enemyHealth != null}, mainCamera={mainCamera != null}");
        
        // 创建血条UI
        CreateHealthBar();
    }
    
    void Awake()
    {
        Debug.Log("EnemyHealthBar Awake被调用");
    }
    
    void Update()
    {
        // 更新血条位置，固定旋转角度
        if (healthBarCanvas != null && mainCamera != null)
        {
            healthBarCanvas.transform.position = transform.position + Vector3.up * 0.8f;
            
            // 固定血条旋转，不跟随摄像机
            healthBarCanvas.transform.rotation = Quaternion.identity; // 无旋转
            
            // 确保血条保持长方体形状
            healthBarCanvas.transform.localScale = new Vector3(1, 1, 1);
            
            // 确保血条在摄像机前面
            Vector3 directionToCamera = mainCamera.transform.position - healthBarCanvas.transform.position;
            float distance = directionToCamera.magnitude;
            if (distance < 1f)
            {
                healthBarCanvas.transform.position += directionToCamera.normalized * 0.1f;
            }
            
            // 调试血条位置
            Debug.Log($"血条位置更新：敌人位置={transform.position}，血条位置={healthBarCanvas.transform.position}");
        }
    }
    
    void CreateHealthBar()
    {
        // 创建血条对象
        healthBarCanvas = new GameObject("HealthBar");
        
        // 创建血条背景
        GameObject bgObj = new GameObject("HealthBarBackground");
        bgObj.transform.SetParent(healthBarCanvas.transform);
        bgObj.transform.localPosition = Vector3.zero;
        bgObj.transform.localScale = new Vector3(1f, 0.2f, 1f); // 增大血条尺寸
        
        SpriteRenderer bgRenderer = bgObj.AddComponent<SpriteRenderer>();
        bgRenderer.sprite = CreateWhiteSprite();
        bgRenderer.color = new Color(0, 0, 0, 1f); // 完全不透明的黑色
        bgRenderer.sortingOrder = 9999; // 确保在最前面
        
        // 创建血条填充
        GameObject fillObj = new GameObject("HealthBarFill");
        fillObj.transform.SetParent(healthBarCanvas.transform);
        fillObj.transform.localPosition = Vector3.zero;
        fillObj.transform.localScale = new Vector3(1f, 0.2f, 1f); // 增大血条尺寸
        
        SpriteRenderer fillRenderer = fillObj.AddComponent<SpriteRenderer>();
        fillRenderer.sprite = CreateWhiteSprite();
        fillRenderer.color = Color.red; // 纯红色
        fillRenderer.sortingOrder = 10000; // 确保在最前面
        
        // 保存引用
        healthBarFill = fillObj.GetComponent<SpriteRenderer>();
        
        Debug.Log($"血条创建：颜色={fillRenderer.color}，排序={fillRenderer.sortingOrder}，位置={healthBarCanvas.transform.position}，尺寸={fillRenderer.transform.localScale}");
        
        // 测试血条是否可见
        Debug.Log($"血条测试：背景颜色={bgRenderer.color}，填充颜色={fillRenderer.color}，背景排序={bgRenderer.sortingOrder}，填充排序={fillRenderer.sortingOrder}");
        
        // 检查血条是否真的被创建
        Debug.Log($"血条对象检查：healthBarCanvas={healthBarCanvas != null}，bgRenderer={bgRenderer != null}，fillRenderer={fillRenderer != null}");
        Debug.Log($"血条位置检查：敌人位置={transform.position}，血条位置={healthBarCanvas.transform.position}");
        
        // 立即更新血条显示
        UpdateHealthBar();
    }
    
    public void UpdateHealthBar()
    {
        Debug.Log("UpdateHealthBar被调用");
        
        if (enemyHealth != null && healthBarFill != null)
        {
            float healthPercent = enemyHealth.GetHealthPercentage();
            
            // 根据血量调整血条宽度
            SpriteRenderer fillRenderer = healthBarFill as SpriteRenderer;
            if (fillRenderer != null)
            {
                // 调整血条宽度，保持高度不变
                Vector3 scale = fillRenderer.transform.localScale;
                scale.x = 1f * healthPercent; // 基础宽度1f乘以血量百分比
                scale.y = 0.2f; // 保持高度
                scale.z = 1f;
                fillRenderer.transform.localScale = scale;
                
                // 保持红色
                fillRenderer.color = Color.red;
                
                Debug.Log($"血条宽度调整：血量百分比={healthPercent:P0}，新宽度={scale.x}");
            }
            
            Debug.Log($"血条更新成功：血量 {enemyHealth.currentHealth:F0}/{enemyHealth.maxHealth:F0}，百分比 {healthPercent:P0}，颜色 {fillRenderer.color}");
        }
        else
        {
            Debug.LogError("血条更新失败：enemyHealth=" + (enemyHealth != null) + ", healthBarFill=" + (healthBarFill != null));
        }
    }
    
    public void DestroyHealthBar()
    {
        if (healthBarCanvas != null)
        {
            Destroy(healthBarCanvas);
        }
    }
    
    // 创建一个白色精灵用于血条
    private Sprite CreateWhiteSprite()
    {
        // 创建一个1x1的白色纹理
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        
        // 从纹理创建精灵
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        return sprite;
    }
} 