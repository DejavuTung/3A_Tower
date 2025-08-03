# Archer 休息动画更新说明

## 修改内容

### 1. 动画资源变更
**之前：** 2个休息动画资源
- `archerRest1` - Archer休息状态图片1
- `archerRest2` - Archer休息状态图片2

**现在：** 4个休息动画资源
- `archerRest1_1` - Archer休息状态图片1-1
- `archerRest1_2` - Archer休息状态图片1-2
- `archerRest2_1` - Archer休息状态图片2-1
- `archerRest2_2` - Archer休息状态图片2-2

### 2. 动画逻辑变更
**之前：** 2帧循环动画
- 帧1: archerRest1
- 帧2: archerRest2
- 循环播放

**现在：** 4帧循环动画
- 帧1: archerRest1_1
- 帧2: archerRest1_2
- 帧3: archerRest2_1
- 帧4: archerRest2_2
- 循环播放

### 3. 代码变更详情

#### 变量修改
```csharp
// 之前
private bool isFirstFrame = true;

// 现在
private int currentRestFrame = 0;  // 当前休息动画帧索引 (0-3)
```

#### 动画播放逻辑
```csharp
// 之前：简单的布尔切换
if (isFirstFrame)
{
    spriteRenderer.sprite = archerRest2;
    isFirstFrame = false;
}
else
{
    spriteRenderer.sprite = archerRest1;
    isFirstFrame = true;
}

// 现在：4帧循环播放
currentRestFrame = (currentRestFrame + 1) % 4; // 循环播放4帧 (0-3)
switch (currentRestFrame)
{
    case 0: spriteRenderer.sprite = archerRest1_1; break;
    case 1: spriteRenderer.sprite = archerRest1_2; break;
    case 2: spriteRenderer.sprite = archerRest2_1; break;
    case 3: spriteRenderer.sprite = archerRest2_2; break;
}
```

## 使用方法

### 1. 在Unity编辑器中设置
1. 选择包含 `ArcherAnimation` 组件的弓箭手对象
2. 在Inspector中找到 "Archer休息状态图片" 部分
3. 将4个对应的Sprite资源拖拽到相应的字段：
   - `Archer Rest1_1` → `archerRest1_1`
   - `Archer Rest1_2` → `archerRest1_2`
   - `Archer Rest2_1` → `archerRest2_1`
   - `Archer Rest2_2` → `archerRest2_2`

### 2. 测试新动画
使用 `RestAnimationTest.cs` 脚本进行测试：

1. 将 `RestAnimationTest` 脚本添加到场景中的任意GameObject
2. 将弓箭手对象拖拽到 `Archer Object` 字段
3. 在Inspector中右键点击脚本组件，选择：
   - "检查休息动画资源" - 检查资源是否正确分配
   - "测试休息动画" - 开始测试动画播放
   - "检查Archer状态" - 检查Archer的当前状态

### 3. 动画速度调整
可以通过修改 `ArcherAnimation` 组件中的 `Rest Animation Speed` 参数来调整动画播放速度：
- 默认值：1.0秒/帧
- 较小值：动画播放更快
- 较大值：动画播放更慢

## 注意事项

1. **资源分配**：确保所有4个休息动画资源都已正确分配，否则动画将无法播放
2. **向后兼容**：如果未分配新的4帧资源，脚本会显示警告信息
3. **性能影响**：4帧动画相比2帧动画会有更多的Sprite切换，但性能影响很小
4. **动画流畅性**：4帧动画提供了更丰富的视觉表现，使休息状态看起来更生动

## 文件清单

### 修改的文件
- `Assets/Scripts/ArcherAnimation.cs` - 主要修改文件

### 新增的文件
- `Assets/Scripts/RestAnimationTest.cs` - 测试脚本
- `Assets/REST_ANIMATION_UPDATE.md` - 本说明文档

## 测试步骤

1. 打开Unity项目
2. 打开 `SampleScene.unity` 场景
3. 找到弓箭手对象（Archer）
4. 检查 `ArcherAnimation` 组件中的休息动画资源分配
5. 运行游戏，观察弓箭手的休息动画
6. 使用 `RestAnimationTest` 脚本进行详细测试

## 预期效果

修改后，弓箭手在休息状态时会按以下顺序循环播放4帧动画：
1. `archerRest1_1`
2. `archerRest1_2`
3. `archerRest2_1`
4. `archerRest2_2`
然后回到第1帧，如此循环播放，提供更丰富的视觉表现。 