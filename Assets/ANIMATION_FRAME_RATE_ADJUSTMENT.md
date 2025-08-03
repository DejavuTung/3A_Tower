# 动画帧率调整文档

## 问题描述

用户要求：**游戏速度改变时，动画帧率也要改变，如果速度提升一倍，敌人行走的4帧动画循环时间要缩短一半，其它的动画也要相应更改，包括射出的弓箭**

## 解决方案

### 1. 敌人行走动画调整

**修改文件**: `Assets/Scripts/SimpleAnimation.cs`

**修改内容**:
- 添加了 `GetGameSpeed()` 方法来获取当前游戏速度
- 修改了 `Update()` 方法中的动画计时逻辑：
  ```csharp
  // 获取当前游戏速度
  float currentGameSpeed = GetGameSpeed();
  
  // 根据游戏速度调整动画速度
  frameTimer += Time.deltaTime * currentGameSpeed;
  ```

**效果**:
- 当游戏速度为1倍速时，敌人行走动画按正常速度播放
- 当游戏速度为2倍速时，敌人行走动画速度也提升2倍
- 当游戏速度为4倍速时，敌人行走动画速度也提升4倍

### 2. 弓箭手动画调整

**文件**: `Assets/Scripts/ArcherAnimation.cs`

**现状**: 已经完整应用了游戏速度调整
- 休息动画帧切换时间：`nextFrameTime = Time.time + (restAnimationSpeed / GetGameSpeed());`
- 射击动画帧间隔：`yield return new WaitForSeconds(shootAnimationSpeed / GetGameSpeed());`
- 测试方法中的等待时间：`yield return new WaitForSeconds(2f / GetGameSpeed());`

**效果**:
- 弓箭手的休息动画和射击动画都会根据游戏速度调整
- 动画播放速度与游戏速度成正比

### 3. 箭矢移动动画调整

**文件**: `Assets/Scripts/AutoTowerDefenseDemo.cs`

**现状**: 已经正确应用了游戏速度调整
- 箭矢移动时间：`elapsed += Time.deltaTime * gameSpeed;`

**效果**:
- 箭矢的飞行速度会根据游戏速度调整
- 在高速游戏模式下，箭矢飞行更快

## 技术实现细节

### GetGameSpeed() 方法

所有动画组件都使用相同的 `GetGameSpeed()` 方法来获取当前游戏速度：

```csharp
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
```

### 动画速度调整原理

1. **敌人行走动画**: 将 `Time.deltaTime` 乘以 `gameSpeed`，使动画帧率与游戏速度成正比
2. **弓箭手动画**: 将动画间隔时间除以 `gameSpeed`，使动画播放速度与游戏速度成正比
3. **箭矢移动**: 将移动时间乘以 `gameSpeed`，使箭矢飞行速度与游戏速度成正比

## 测试验证

### 测试步骤

1. **启动游戏**，选择 `SampleScene.unity` 场景
2. **点击"开始游戏"** 按钮
3. **观察不同游戏速度下的动画效果**：
   - 1倍速：正常动画速度
   - 2倍速：动画速度提升2倍
   - 4倍速：动画速度提升4倍

### 验证要点

1. **敌人行走动画**：
   - 在1倍速下，敌人行走动画循环时间正常
   - 在2倍速下，敌人行走动画循环时间缩短一半
   - 在4倍速下，敌人行走动画循环时间缩短为1/4

2. **弓箭手动画**：
   - 休息动画帧切换速度与游戏速度成正比
   - 射击动画播放速度与游戏速度成正比

3. **箭矢动画**：
   - 箭矢飞行速度与游戏速度成正比

## 预期效果

- **1倍速游戏**：所有动画按正常速度播放
- **2倍速游戏**：所有动画速度提升2倍
- **4倍速游戏**：所有动画速度提升4倍
- **0.5倍速游戏**：所有动画速度降低一半

这样确保了游戏速度的改变不会影响游戏结果，同时所有动画都会相应地调整，保持视觉上的一致性。 