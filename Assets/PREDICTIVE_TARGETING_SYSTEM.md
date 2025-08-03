# 预测性瞄准系统实现

## 概述
实现了预测性瞄准系统，确保每次攻击都能命中移动的敌人。系统会计算弓箭飞行时间和敌人移动速度，判断是否能命中目标，避免无效攻击。

## 核心功能

### 1. 预测性瞄准检查 (`CanHitMovingTarget`)
- **功能**: 计算弓箭是否能命中移动的敌人
- **参数**: 
  - `archerPosition`: 弓箭手位置
  - `targetEnemy`: 目标敌人
- **返回值**: `bool` - 是否能命中

#### 检查项目：
1. **敌人状态检查**: 如果敌人停顿时，直接瞄准当前位置
2. **预测位置计算**: 根据敌人速度和弓箭飞行时间计算预测位置
3. **攻击范围检查**: 确保预测位置仍在攻击范围内
4. **速度匹配检查**: 确保弓箭速度能追上敌人
5. **精度误差检查**: 考虑敌人移动造成的命中误差

### 2. 预测位置计算 (`CalculatePredictedTargetPosition`)
- **功能**: 计算敌人的预测位置
- **算法**: 
  ```
  预测位置 = 当前位置 + (敌人速度 × 弓箭飞行时间)
  ```

### 3. 改进的攻击逻辑
- 在`AttackEnemy()`方法中集成预测性瞄准检查
- 只有通过预测性瞄准检查的目标才会被攻击
- 弓箭会瞄准预测位置而不是当前位置

## 技术细节

### 弓箭飞行时间
```csharp
float arrowFlightTime = 0.5f / gameSpeed;
```
- 基础飞行时间: 0.5秒
- 根据游戏速度调整

### 敌人速度获取
```csharp
Vector3 enemyVelocity = enemyMovement.GetCurrentVelocity();
```
- 从`EnemyMovement`组件获取当前速度向量
- 考虑停顿状态（停顿时速度为0）

### 预测位置计算
```csharp
Vector3 enemyDisplacement = enemyVelocity * arrowFlightTime;
Vector3 predictedPosition = targetEnemy.transform.position + enemyDisplacement;
```

### 速度匹配检查
```csharp
float relativeSpeed = Vector3.Dot(enemyVelocity, arrowDirection);
if (relativeSpeed > arrowSpeed * 0.8f)
{
    // 敌人移动速度过快，弓箭无法命中
}
```

### 精度误差检查
```csharp
float movementError = enemyVelocity.magnitude * arrowFlightTime * 0.5f;
if (movementError > accuracyThreshold)
{
    // 移动造成的误差过大
}
```

## 新增方法

### EnemyMovement.cs
- `GetCurrentVelocity()`: 获取敌人当前速度向量
  - 停顿时返回零向量
  - 正常移动时返回朝向基地的速度向量

### AutoTowerDefenseDemo.cs
- `CanHitMovingTarget()`: 预测性瞄准检查
- `CalculatePredictedTargetPosition()`: 计算预测位置
- 修改了`AttackEnemy()`和`ShowProjectile()`方法

## 优势

1. **提高命中率**: 通过预测敌人位置，大幅提高攻击命中率
2. **避免无效攻击**: 只攻击能够命中的目标，节省攻击次数
3. **考虑游戏速度**: 系统会根据游戏速度调整预测算法
4. **智能判断**: 综合考虑敌人速度、弓箭速度、攻击范围等因素

## 测试建议

1. **正常移动测试**: 观察弓箭是否能准确命中移动的敌人
2. **停顿状态测试**: 验证停顿时敌人是否被正确瞄准
3. **高速移动测试**: 测试高速移动敌人是否被正确跳过
4. **边界情况测试**: 测试攻击范围边缘的敌人处理
5. **游戏速度测试**: 验证不同游戏速度下的预测准确性

## 调试信息

系统会输出详细的调试信息：
- 预测性瞄准成功/失败的原因
- 敌人当前位置和预测位置
- 移动距离和速度信息
- 攻击范围检查结果

这些信息有助于调试和优化预测算法。 