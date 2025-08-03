# 背景地图功能实现

## 功能描述
为塔防游戏添加背景地图功能，使用MAP.jpg作为游戏背景。

## 实现内容

### 1. 添加背景地图字段
在`AutoTowerDefenseDemo.cs`中添加了背景地图的Sprite字段：
```csharp
[Header("图像资源")]
public Sprite baseSprite;        // 基地图像
public Sprite towerSprite;       // 防御塔图像
public Sprite enemySprite;       // 敌人图像
public Sprite backgroundMap;     // 背景地图
```

### 2. 创建背景地图方法
添加了`CreateBackgroundMap()`方法来创建和设置背景地图：

```csharp
void CreateBackgroundMap()
{
    if (backgroundMap != null)
    {
        // 创建背景地图对象
        GameObject backgroundObj = new GameObject("BackgroundMap");
        
        // 添加SpriteRenderer组件
        SpriteRenderer spriteRenderer = backgroundObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = backgroundMap;
        spriteRenderer.sortingOrder = -10; // 确保背景在最底层
        
        // 设置位置到世界坐标原点
        backgroundObj.transform.position = Vector3.zero;
        
        // 调整背景大小以适应屏幕
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;
        
        // 计算背景的缩放比例
        float scaleX = screenWidth / backgroundMap.bounds.size.x;
        float scaleY = screenHeight / backgroundMap.bounds.size.y;
        float scale = Mathf.Max(scaleX, scaleY); // 使用较大的缩放比例确保覆盖整个屏幕
        
        backgroundObj.transform.localScale = new Vector3(scale, scale, 1f);
        
        Debug.Log($"背景地图已创建 - 屏幕尺寸: {screenWidth}x{screenHeight}, 背景缩放: {scale}");
    }
    else
    {
        Debug.LogWarning("背景地图Sprite未设置，跳过背景创建");
    }
}
```

### 3. 集成到游戏流程
在`StartGame()`方法中调用背景地图创建：
```csharp
void StartGame()
{
    if (!gameStarted)
    {
        gameStarted = true;
        
        // 隐藏开始按钮
        if (startGameButton != null)
        {
            startGameButton.gameObject.SetActive(false);
        }
        
        // 创建背景地图
        CreateBackgroundMap();
        
        // 创建游戏元素
        CreateBase();
        CreateTower();
        
        // 创建速度控制按钮
        CreateSpeedControlButtons(startGameButton.transform.parent);
        
        // 显示第一波即将来袭的提示
        ShowFirstWaveText();
        
        Debug.Log("游戏开始！速度控制按钮已显示");
    }
}
```

## 功能特点

### 1. 自动缩放
- 背景地图会自动根据屏幕尺寸进行缩放
- 使用`Mathf.Max(scaleX, scaleY)`确保背景完全覆盖屏幕
- 保持背景图片的宽高比

### 2. 层级管理
- 设置`sortingOrder = -10`确保背景在所有游戏元素的最底层
- 不会影响其他游戏对象的显示

### 3. 错误处理
- 如果背景地图Sprite未设置，会输出警告信息但不会影响游戏运行
- 提供详细的调试信息帮助开发者了解背景创建过程

## 使用方法

### 1. 在Unity编辑器中设置
1. 选择包含`AutoTowerDefenseDemo`脚本的游戏对象
2. 在Inspector面板中找到"图像资源"部分
3. 将MAP.jpg拖拽到"Background Map"字段中

### 2. 通过代码设置
```csharp
// 获取脚本组件
AutoTowerDefenseDemo gameManager = FindFirstObjectByType<AutoTowerDefenseDemo>();

// 加载背景地图
Sprite backgroundSprite = Resources.Load<Sprite>("Sprites/Map");
gameManager.backgroundMap = backgroundSprite;
```

## 文件位置
- 背景地图文件：`Assets/Sprites/Map.jpg`
- 实现代码：`Assets/Scripts/AutoTowerDefenseDemo.cs`
- 文档：`Assets/BACKGROUND_MAP_IMPLEMENTATION.md`

## 注意事项
1. 确保MAP.jpg文件存在于`Assets/Sprites/`目录中
2. 背景地图会在游戏开始时自动创建
3. 背景地图的缩放会根据当前屏幕分辨率自动调整
4. 在游戏重启时，背景地图会被自动销毁并重新创建

## 游戏重启处理
在`RestartGame()`方法中添加了背景地图的清理逻辑：
```csharp
// 销毁背景地图
GameObject backgroundMapObj = GameObject.Find("BackgroundMap");
if (backgroundMapObj != null)
{
    Destroy(backgroundMapObj);
    Debug.Log("销毁背景地图");
}
```

当调用`StartGame()`方法时，会重新创建背景地图，确保每次游戏重启都有正确的背景显示。 