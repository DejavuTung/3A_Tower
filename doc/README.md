# 3A_Tower - Unity塔防游戏项目

## 📋 项目概述

3A_Tower是一个基于Unity引擎开发的塔防游戏项目，采用自动化的游戏机制，玩家无需手动操作，系统会自动进行防御塔攻击和敌人生成。

### 🎮 游戏特色

- **自动化游戏机制**：防御塔自动攻击最近的敌人
- **波次系统**：敌人分波次来袭，难度递增
- **血量系统**：敌人具有血量，需要多次攻击才能消灭
- **动画系统**：敌人具有行走动画效果
- **UI系统**：实时显示金币、波次、基地血量等信息
- **路径寻路**：敌人具有智能路径寻找，会绕过防御塔

### 🏗️ 项目架构

```
3A_Tower/
├── Assets/
│   ├── Scripts/           # 核心脚本
│   │   ├── AutoTowerDefenseDemo.cs    # 主游戏逻辑
│   │   ├── EnemyHealth.cs             # 敌人血量系统
│   │   ├── SimpleAnimation.cs         # 简单动画系统
│   │   ├── EnemyHealthBar.cs         # 敌人血条UI
│   │   ├── AnimatedEnemy.cs          # 敌人动画
│   │   └── Managers/                  # 管理器脚本
│   │       ├── GameManager.cs         # 游戏管理器
│   │       └── TowerDefenseManager.cs # 塔防管理器
│   ├── Scenes/           # 游戏场景
│   │   ├── CleanTest.unity           # 完整测试场景
│   │   ├── SampleScene.unity         # 示例场景
│   │   └── UltraSimpleTest.unity     # 简化测试场景
│   ├── Prefabs/          # 预制体
│   │   ├── Enemy/        # 敌人预制体
│   │   ├── Tower/        # 防御塔预制体
│   │   └── UI/           # UI预制体
│   └── Sprites/          # 图像资源
└── doc/                  # 项目文档
```

## 🚀 快速开始

### 环境要求

- Unity 2022.3 LTS 或更高版本
- .NET Framework 4.8
- 至少 4GB RAM

### 安装步骤

1. **克隆项目**
   ```bash
   git clone https://github.com/DejavuTung/3A_Tower.git
   ```

2. **打开Unity项目**
   - 启动Unity Hub
   - 点击"Open" → 选择项目文件夹
   - 等待Unity导入项目

3. **运行游戏**
   - 在Unity编辑器中打开 `Assets/Scenes/CleanTest.unity`
   - 点击Play按钮开始游戏

### 游戏操作

- **无需手动操作**：游戏完全自动化
- **观察游戏进程**：关注UI显示的金币、波次、基地血量
- **游戏目标**：尽可能长时间保护基地不被敌人摧毁

## 📚 详细文档

- [核心脚本文档](./scripts/README.md) - 详细说明所有脚本的功能和用法
- [游戏机制文档](./gameplay/README.md) - 游戏规则和机制说明
- [开发指南](./development/README.md) - 开发者指南和最佳实践
- [故障排除](./troubleshooting/README.md) - 常见问题和解决方案

## 🛠️ 技术栈

- **游戏引擎**: Unity 2022.3 LTS
- **编程语言**: C#
- **版本控制**: Git
- **包管理**: Unity Package Manager

## 📄 许可证

本项目采用 MIT 许可证 - 详见 [LICENSE](../LICENSE) 文件

## 🤝 贡献指南

欢迎贡献代码！请查看 [CONTRIBUTING.md](./CONTRIBUTING.md) 了解如何参与项目开发。

## 📞 联系方式

- 项目维护者: DejavuTung
- GitHub: https://github.com/DejavuTung/3A_Tower
- 问题反馈: 请在GitHub Issues中提交

---

**最后更新**: 2024年8月2日
**版本**: 1.0.0 