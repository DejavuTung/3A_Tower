# 游戏重新开始功能说明

## 功能概述

当游戏结束后点击"再来一次"按钮时，系统会完全清除上一局的所有信息，包括Archer的射击历史记录，确保新一局游戏完全重新开始。

## 实现的功能

### 1. 游戏状态重置
- 重置所有游戏变量（金币、波次、基地血量等）
- 清空所有敌人列表
- 销毁基地和防御塔对象
- 销毁游戏结束UI界面

### 2. Archer射击历史记录清除
- 清除Archer的最后射击方向记录
- 重置Archer的射击历史标志
- 重置Archer的动画状态
- 恢复Archer的默认镜像状态

## 代码实现

### RestartGame() 方法
```csharp
void RestartGame()
{
    // 重置游戏状态
    gameStarted = false;
    gold = 0;
    wave = 1;
    baseHealth = 10;
    
    // 清空所有敌人
    for (int i = enemies.Count - 1; i >= 0; i--)
    {
        if (enemies[i] != null) Destroy(enemies[i]);
    }
    enemies.Clear();
    
    // 销毁基地和防御塔
    if (baseObj != null) Destroy(baseObj);
    if (towerObj != null) Destroy(towerObj);
    
    // 清除所有Archer相关的历史记录
    ClearArcherHistory();
    
    // 重新创建UI
    CreateUI();
}
```

### ClearArcherHistory() 方法
```csharp
void ClearArcherHistory()
{
    // 查找所有ArcherAnimation组件并重置其状态
    ArcherAnimation[] archerAnimations = FindObjectsOfType<ArcherAnimation>();
    foreach (ArcherAnimation archerAnim in archerAnimations)
    {
        if (archerAnim != null)
        {
            archerAnim.ResetArcherState();
        }
    }
}
```

### ResetArcherState() 方法
```csharp
public void ResetArcherState()
{
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
    nextFrameTime = Time.time + restAnimationSpeed;
}
```

## 测试方法

使用 `GameRestartTest.cs` 脚本进行测试：

1. 将脚本添加到场景中的任意GameObject
2. 将弓箭手对象拖拽到 `Archer Object` 字段
3. 在Inspector中右键点击脚本组件，选择：
   - "测试游戏重新开始功能" - 完整测试流程
   - "检查Archer状态" - 检查当前状态
   - "手动重置Archer状态" - 手动重置

## 预期效果

重置前：Archer可能有射击历史记录，休息动画使用不同的资源组
重置后：Archer回到默认状态（使用rest1组镜像），所有射击历史记录被清除 