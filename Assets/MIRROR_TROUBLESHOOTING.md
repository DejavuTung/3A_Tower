# 镜像功能故障排除指南

## 问题描述
用户反馈镜像功能没有生效，射击左边来的敌人还是使用了朝右边的动画。

## 调试步骤

### 1. 检查控制台输出
运行游戏后，查看控制台是否有以下调试信息：
- `ArcherAnimation: 射击方向: xxx, 计算角度: xxx°`
- `ArcherAnimation: 最终决定 - useShoot1: xxx, needMirror: xxx`
- `ArcherAnimation: 设置镜像 - 新缩放: xxx`

### 2. 使用测试脚本
#### 方法1：使用 ArcherMirrorDebugTest
1. 将 `ArcherMirrorDebugTest` 组件添加到场景中的任意GameObject
2. 在 Inspector 中分配弓箭手对象
3. 运行游戏，观察控制台输出
4. 右键点击组件，使用 Context Menu 手动测试特定方向

#### 方法2：使用 MirrorVerification
1. 将 `MirrorVerification` 组件添加到场景中
2. 分配弓箭手对象作为测试对象
3. 运行游戏，观察镜像测试结果
4. 使用 Context Menu 手动测试镜像功能

### 3. 验证角度计算
检查控制台输出中的角度值：
- 右下角：-90° 到 0°
- 右上角：0° 到 90°
- 左上角：90° 到 180°
- 左下角：-180° 到 -90°

### 4. 检查缩放值
观察控制台输出中的缩放值：
- 原始缩放：应该是 (1, 1, 1) 或类似值
- 镜像缩放：应该是 (-1, 1, 1) 或类似值

## 常见问题

### 问题1：角度计算错误
**症状**: 控制台显示的角度值与预期不符
**解决**: 检查射击方向的计算是否正确

### 问题2：镜像设置不生效
**症状**: 缩放值没有改变
**解决**: 
1. 检查 `originalScale` 是否正确保存
2. 确认镜像设置代码是否执行
3. 检查是否有其他脚本覆盖了缩放值

### 问题3：动画帧分配错误
**症状**: 使用了错误的动画组
**解决**: 检查 `archerShoot1_1` 到 `archerShoot1_4` 和 `archerShoot2_1` 到 `archerShoot2_4` 是否正确分配

## 测试用例

### 测试用例1：手动测试四个方向
```csharp
// 右下角
Vector3 direction = new Vector3(1, -1, 0).normalized;
archerAnim.TriggerShootAnimation(direction);

// 右上角
direction = new Vector3(1, 1, 0).normalized;
archerAnim.TriggerShootAnimation(direction);

// 左上角
direction = new Vector3(-1, 1, 0).normalized;
archerAnim.TriggerShootAnimation(direction);

// 左下角
direction = new Vector3(-1, -1, 0).normalized;
archerAnim.TriggerShootAnimation(direction);
```

### 测试用例2：验证镜像效果
1. 记录弓箭手的初始缩放值
2. 触发射击动画
3. 观察缩放值是否改变
4. 检查动画结束后是否恢复原始缩放

## 预期结果

### 正确的行为
- 射击左上角敌人：使用 shoot2 组动画 + 镜像
- 射击左下角敌人：使用 shoot1 组动画 + 镜像
- 射击右上角敌人：使用 shoot2 组动画 + 不镜像
- 射击右下角敌人：使用 shoot1 组动画 + 不镜像

### 控制台输出示例
```
ArcherAnimation: 射击方向: (-0.7, 0.7, 0), 计算角度: 135°
ArcherAnimation: 左上角射击，使用shoot2组，镜像
ArcherAnimation: 最终决定 - useShoot1: False, needMirror: True
ArcherAnimation: 原始缩放: (1, 1, 1), 当前缩放: (1, 1, 1)
ArcherAnimation: 设置镜像 - 新缩放: (-1, 1, 1)
```

如果镜像功能仍然不工作，请提供控制台的完整输出信息，以便进一步诊断问题。 