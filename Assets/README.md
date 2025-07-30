# 游戏资源文件夹说明

## 📁 推荐文件夹结构

### 图形资源 (Graphics)
- `Sprites/` - 精灵图片
  - `Characters/` - 角色精灵
  - `UI/` - 界面元素
  - `Backgrounds/` - 背景图片
  - `Effects/` - 特效图片
- `Textures/` - 纹理资源
- `Materials/` - 材质文件

### 音频资源 (Audio)
- `Music/` - 背景音乐
- `SFX/` - 音效文件
- `Voice/` - 语音文件

### 预制体 (Prefabs)
- `Characters/` - 角色预制体
- `UI/` - 界面预制体
- `Environment/` - 环境预制体

### 脚本 (Scripts)
- `Player/` - 玩家相关脚本
- `Enemy/` - 敌人相关脚本
- `UI/` - 界面脚本
- `Managers/` - 管理器脚本

### 场景 (Scenes)
- `Levels/` - 关卡场景
- `UI/` - 界面场景

## 🎯 资源获取建议

### 免费资源来源
1. **Unity Asset Store** - 官方资源商店
2. **OpenGameArt.org** - 免费游戏素材
3. **Kenney.nl** - 高质量免费资源
4. **Itch.io** - 独立游戏资源

### 资源要求
- **图片格式**：PNG, JPG (推荐PNG支持透明)
- **音频格式**：MP3, WAV, OGG
- **分辨率**：适配移动设备 (建议512x512以下)
- **文件大小**：控制单个文件大小，优化加载速度

## 📱 移动端优化建议
- 使用适当的压缩格式
- 控制纹理大小
- 使用Sprite Atlas优化内存
- 音频文件压缩
- 考虑使用AssetBundle动态加载 