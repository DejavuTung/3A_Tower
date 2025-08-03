# 攻击范围圆圈样式更新

## 修改内容

根据用户要求，对攻击范围指示器进行了以下修改：

### 1. 颜色和透明度调整
- **之前**：白色圆圈，30%透明度
- **现在**：浅灰色圆圈，5%透明度
- **颜色值**：`new Color(0.8f, 0.8f, 0.8f, 0.05f)`

### 2. 笔触宽度调整
- **之前**：3像素宽的轮廓
- **现在**：1像素宽的轮廓（之前的1/3）
- **修改位置**：`CreateCircleSprite()`方法中的`innerRadius`计算

### 3. 简化显示
- **移除了**：精确攻击范围指示器（红色圆圈）
- **保留**：单一的攻击范围圆圈
- **原因**：用户要求只显示一种颜色的圆圈

## 代码修改详情

### CreateTowerRangeIndicator() 方法
```csharp
// 修改前
rangeRenderer.color = new Color(1f, 1f, 1f, 0.3f); // 30%透明度

// 修改后
rangeRenderer.color = new Color(0.8f, 0.8f, 0.8f, 0.05f); // 浅灰色，5%透明度
```

### CreateCircleSprite() 方法
```csharp
// 修改前
float innerRadius = outerRadius - 3f; // 内半径，形成3像素宽的轮廓

// 修改后
float innerRadius = outerRadius - 1f; // 内半径，形成1像素宽的轮廓（之前的1/3）
```

## 视觉效果

修改后的攻击范围圆圈具有以下特点：
- 更加细腻的轮廓线条
- 更低的透明度，不会干扰游戏视觉
- 统一的浅灰色，与游戏风格更协调
- 简化的显示，避免视觉混乱

## 测试方法

1. 启动游戏
2. 观察防御塔周围的攻击范围圆圈
3. 确认圆圈为浅灰色，透明度很低
4. 确认圆圈轮廓线条细腻（1像素宽）

## 文件位置

修改的文件：`Assets/Scripts/AutoTowerDefenseDemo.cs`
- `CreateTowerRangeIndicator()` 方法（第424-445行）
- `CreateCircleSprite()` 方法（第456-508行） 