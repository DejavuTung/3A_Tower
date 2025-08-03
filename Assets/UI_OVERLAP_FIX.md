# UI重叠问题修复

## 问题描述
用户报告：点击"再来一次"后，上面的游戏信息重叠了。

## 问题原因
在`RestartGame()`方法中，代码调用了`CreateUIForRestart()`来创建新的UI，但是没有销毁旧的Canvas。这导致：
1. 新的Canvas被创建
2. 旧的Canvas仍然存在
3. 两个Canvas同时显示，造成UI重叠

## 解决方案
在`RestartGame()`方法中添加了销毁旧Canvas的逻辑：

```csharp
// 销毁旧的Canvas（防止UI重叠）
GameObject oldCanvas = GameObject.Find("Canvas");
if (oldCanvas != null)
{
    Destroy(oldCanvas);
    Debug.Log("销毁旧的Canvas以防止UI重叠");
}
```

## 修改位置
- 文件：`Assets/Scripts/AutoTowerDefenseDemo.cs`
- 方法：`RestartGame()`
- 位置：在销毁速度控制按钮之后，清除Archer历史记录之前

## 修复效果
- 确保每次重新开始游戏时，旧的UI被完全清理
- 避免多个Canvas同时存在导致的UI重叠
- 保持游戏重启功能的完整性

## 测试方法
1. 开始游戏
2. 让游戏结束（基地血量降为0）
3. 点击"再来一次"按钮
4. 验证游戏信息不再重叠，UI显示正常 