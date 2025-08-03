# Archer 条件休息动画功能说明

## 功能概述

Archer的休息动画现在会根据最后射击的方向来决定使用哪组休息动画资源以及是否需要镜像。

## 实现逻辑

### 1. 默认休息状态
- **使用资源**: `Archer_rest_1_1` / `Archer_rest_1_2`
- **镜像状态**: 启用镜像（`transform.localScale.x = -originalScale.x`）
- **触发条件**: 游戏开始时，Archer还未进行过任何射击

### 2. 射击后的休息动画规则

根据最后射击的角度，休息动画会按以下规则选择：

| 射击方向 | 角度范围 | 使用资源组 | 镜像状态 | 说明 |
|---------|---------|-----------|---------|------|
| 右下角 | -90° 到 0° | `Archer_rest_1_1` / `Archer_rest_1_2` | 不镜像 | 正常显示 |
| 左下角 | -180° 到 -90° | `Archer_rest_1_1` / `Archer_rest_1_2` | 启用镜像 | 水平翻转 |
| 右上角 | 0° 到 90° | `Archer_rest_2_1` / `Archer_rest_2_2` | 不镜像 | 正常显示 |
| 左上角 | 90° 到 180° | `Archer_rest_2_1` / `Archer_rest_2_2` | 启用镜像 | 水平翻转 |

### 3. 角度计算

```csharp
float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
```

- **右下角**: `angle >= -90f && angle <= 0f`
- **左下角**: `angle < -90f && angle >= -180f`
- **右上角**: `angle > 0f && angle <= 90f`
- **左上角**: `angle > 90f && angle <= 180f`

## 代码实现

### 核心变量

```csharp
private Vector3 lastShootDirection = Vector3.zero;  // 最后射击方向
private bool hasShotBefore = false;                 // 是否已经射击过
```

### 主要方法

#### 1. `UpdateRestAnimation()`
根据最后射击方向确定休息动画：

```csharp
private void UpdateRestAnimation()
{
    if (!hasShotBefore)
    {
        // 默认休息动画：使用rest1组镜像
        SetRestAnimation(true, true);
        return;
    }
    
    // 计算最后射击角度并确定休息动画
    float lastAngle = Mathf.Atan2(lastShootDirection.y, lastShootDirection.x) * Mathf.Rad2Deg;
    // ... 根据角度确定 useRest1Group 和 needMirror
    SetRestAnimation(useRest1Group, needMirror);
}
```

#### 2. `SetRestAnimation(bool useRest1Group, bool needMirror)`
设置休息动画的镜像状态和Sprite：

```csharp
private void SetRestAnimation(bool useRest1Group, bool needMirror)
{
    // 设置镜像
    if (needMirror)
    {
        transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
    }
    else
    {
        transform.localScale = originalScale;
    }
    
    // 设置对应的Sprite
    if (useRest1Group)
    {
        // 使用 rest1 组资源
        switch (currentRestFrame)
        {
            case 0: spriteRenderer.sprite = archerRest1_1; break;
            case 1: spriteRenderer.sprite = archerRest1_2; break;
            case 2: spriteRenderer.sprite = archerRest1_1; break;
            case 3: spriteRenderer.sprite = archerRest1_2; break;
        }
    }
    else
    {
        // 使用 rest2 组资源
        switch (currentRestFrame)
        {
            case 0: spriteRenderer.sprite = archerRest2_1; break;
            case 1: spriteRenderer.sprite = archerRest2_2; break;
            case 2: spriteRenderer.sprite = archerRest2_1; break;
            case 3: spriteRenderer.sprite = archerRest2_2; break;
        }
    }
}
```

#### 3. `TriggerShootAnimation(Vector3 shootDirection)`
保存射击方向：

```csharp
public void TriggerShootAnimation(Vector3 shootDirection)
{
    // 保存射击方向
    lastShootDirection = shootDirection;
    hasShotBefore = true;
    
    StartCoroutine(PlayShootAnimation(shootDirection));
}
```

## 测试方法

### 1. 使用内置测试方法

在Unity编辑器中：
1. 选择包含 `ArcherAnimation` 组件的弓箭手对象
2. 在Inspector中右键点击 `ArcherAnimation` 组件
3. 选择 "测试休息动画条件逻辑"

### 2. 使用专用测试脚本

使用 `RestAnimationConditionalTest.cs` 脚本：

1. 将脚本添加到场景中的任意GameObject
2. 将弓箭手对象拖拽到 `Archer Object` 字段
3. 在Inspector中右键点击脚本组件，选择：
   - "测试条件休息动画逻辑" - 开始自动测试
   - "检查当前状态" - 检查当前状态
   - "重置测试" - 重置测试环境

## 测试步骤

### 手动测试步骤

1. **打开游戏场景**
   - 打开 `SampleScene.unity`
   - 确保场景中有弓箭手对象

2. **观察默认状态**
   - 游戏开始时，Archer应该显示 `Archer_rest_1_1` 镜像状态
   - 检查 `transform.localScale.x` 是否为负值

3. **测试不同方向射击**
   - 触发右下角射击：观察休息动画是否使用 `rest1` 组且不镜像
   - 触发左下角射击：观察休息动画是否使用 `rest1` 组且镜像
   - 触发右上角射击：观察休息动画是否使用 `rest2` 组且不镜像
   - 触发左上角射击：观察休息动画是否使用 `rest2` 组且镜像

4. **验证角度计算**
   - 右下角: -45° (应该使用 rest1 组，不镜像)
   - 左下角: -135° (应该使用 rest1 组，镜像)
   - 右上角: 45° (应该使用 rest2 组，不镜像)
   - 左上角: 135° (应该使用 rest2 组，镜像)

## 调试信息

脚本会输出详细的调试信息：

```
ArcherAnimation: 最后射击角度: -45°
ArcherAnimation: 最后射击为右下角，使用rest1组，不镜像
ArcherAnimation: 休息动画不设置镜像 - 缩放: (2, 2, 2)
ArcherAnimation: 休息动画切换到archerRest1_1
```

## 注意事项

1. **资源分配**: 确保所有4个休息动画资源都已正确分配
2. **初始状态**: 游戏开始时Archer默认使用镜像的 `rest1` 组
3. **角度精度**: 角度计算使用 `Mathf.Atan2` 确保准确性
4. **状态跟踪**: 使用 `hasShotBefore` 标志区分默认状态和射击后状态

## 故障排除

### 常见问题

1. **休息动画不切换**
   - 检查 `archerRest1_1`, `archerRest1_2`, `archerRest2_1`, `archerRest2_2` 是否已分配
   - 确认 `ArcherAnimation` 组件存在且正常工作

2. **镜像效果不正确**
   - 检查 `originalScale` 是否正确保存
   - 确认 `transform.localScale` 设置是否正确

3. **角度判断错误**
   - 检查 `lastShootDirection` 是否正确保存
   - 验证角度计算逻辑

### 调试命令

使用 `RestAnimationConditionalTest.cs` 的 "检查当前状态" 功能来诊断问题。 