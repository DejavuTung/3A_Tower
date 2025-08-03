# ArcherAnimation 镜像功能说明

## 功能概述
ArcherAnimation 脚本已经实现了完整的镜像功能，可以根据射击方向自动调整弓箭手的朝向和动画。

## 镜像逻辑

### 角度划分
- **右下角** (-90° 到 0°): 使用 shoot1 组动画，不镜像
- **右上角** (0° 到 90°): 使用 shoot2 组动画，不镜像  
- **左上角** (90° 到 180°): 使用 shoot2 组动画，镜像
- **左下角** (-180° 到 -90°): 使用 shoot1 组动画，镜像

### 镜像实现
```csharp
// 设置镜像
if (needMirror)
{
    transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
}
else
{
    transform.localScale = originalScale;
}
```

## 使用方法

### 1. 在塔防脚本中调用
```csharp
// 计算射击方向
Vector3 shootDirection = (enemy.transform.position - archer.transform.position).normalized;

// 触发射击动画（自动处理镜像）
ArcherAnimation archerAnim = archer.GetComponent<ArcherAnimation>();
if (archerAnim != null)
{
    archerAnim.TriggerShootAnimation(shootDirection);
}
```

### 2. 测试镜像功能
使用 `ArcherMirrorTest.cs` 脚本可以测试镜像功能：
1. 将 `ArcherMirrorTest` 组件添加到场景中的任意GameObject
2. 在 Inspector 中分配弓箭手对象
3. 运行游戏，脚本会自动测试四个方向的射击

## 动画资源要求

### 必需的动画帧
- `archerShoot1_1` 到 `archerShoot1_4`: 右下角射击动画
- `archerShoot2_1` 到 `archerShoot2_4`: 右上角射击动画

### 镜像处理
- 左上角射击：使用 shoot2 组动画 + 水平镜像
- 左下角射击：使用 shoot1 组动画 + 水平镜像

## 调试信息
脚本会输出详细的调试信息，包括：
- 射击方向的角度计算
- 使用的动画组和镜像设置
- 动画播放状态

## 注意事项
1. 确保所有动画帧都已正确分配
2. 镜像功能会自动恢复原始缩放状态
3. 射击动画期间会暂停休息动画
4. 支持连续射击，但会忽略正在进行的射击请求 