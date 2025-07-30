# 团队协作开发指南

## 📋 目录
- [邀请团队成员](#邀请团队成员)
- [分支管理策略](#分支管理策略)
- [日常开发流程](#日常开发流程)
- [代码审查流程](#代码审查流程)
- [发布流程](#发布流程)
- [沟通工具](#沟通工具)

## 👥 邀请团队成员

### 步骤1：添加协作者
1. 访问 https://github.com/DejavuTung/3A_Tower
2. 点击 `Settings` 标签页
3. 在左侧菜单选择 `Collaborators and teams`
4. 点击 `Add people` 按钮
5. 输入团队成员的GitHub用户名或邮箱
6. 选择权限级别：
   - **Write**: 可以推送代码，创建分支
   - **Maintain**: 可以管理Issues和Pull Requests
   - **Admin**: 完全管理权限

### 步骤2：设置团队权限
```bash
# 创建团队（在GitHub网页端操作）
1. 进入仓库设置 -> Teams
2. 创建新团队，如 "3A_Tower_Developers"
3. 添加成员到团队
4. 设置团队对仓库的权限
```

## 🌿 分支管理策略

### 分支命名规范
```
main          # 主分支，稳定版本
develop       # 开发分支，集成所有功能
feature/*     # 功能分支，如 feature/player-movement
bugfix/*      # 修复分支，如 bugfix/collision-detection
hotfix/*      # 紧急修复，如 hotfix/critical-bug
release/*     # 发布分支，如 release/v1.0.0
```

### 分支创建命令
```bash
# 创建功能分支
git checkout -b feature/new-feature

# 创建修复分支
git checkout -b bugfix/issue-description

# 创建发布分支
git checkout -b release/v1.0.0
```

## 🔄 日常开发流程

### 开发前准备
```bash
# 1. 同步最新代码
git checkout main
git pull origin main

# 2. 创建功能分支
git checkout -b feature/your-feature-name

# 3. 开始开发
# ... 编写代码 ...
```

### 开发中
```bash
# 定期提交代码
git add .
git commit -m "feat: 添加新功能描述"

# 推送到远程分支
git push origin feature/your-feature-name
```

### 完成开发
```bash
# 1. 确保代码通过测试
# 2. 提交最终代码
git add .
git commit -m "feat: 完成功能开发"

# 3. 推送到远程
git push origin feature/your-feature-name

# 4. 在GitHub创建Pull Request
```

## 👀 代码审查流程

### 创建Pull Request
1. 在GitHub仓库页面点击 `Pull requests`
2. 点击 `New pull request`
3. 选择源分支（您的功能分支）和目标分支（通常是develop）
4. 填写PR标题和描述：
   ```
   标题: feat: 添加玩家移动系统
   
   描述:
   ## 功能描述
   - 实现了WASD键控制玩家移动
   - 添加了移动动画
   - 实现了碰撞检测
   
   ## 测试
   - [x] 移动功能正常
   - [x] 动画播放正常
   - [x] 碰撞检测正常
   
   ## 截图
   [添加相关截图]
   ```

### 代码审查检查清单
- [ ] 代码符合项目规范
- [ ] 功能按预期工作
- [ ] 添加了必要的注释
- [ ] 更新了相关文档
- [ ] 通过了所有测试
- [ ] 没有引入新的bug

## 🚀 发布流程

### 版本发布步骤
```bash
# 1. 创建发布分支
git checkout -b release/v1.0.0

# 2. 更新版本号
# 修改 ProjectSettings/ProjectVersion.txt

# 3. 提交版本更新
git add .
git commit -m "chore: 更新版本号到v1.0.0"

# 4. 合并到main分支
git checkout main
git merge release/v1.0.0

# 5. 创建标签
git tag -a v1.0.0 -m "Release version 1.0.0"
git push origin v1.0.0

# 6. 删除发布分支
git branch -d release/v1.0.0
git push origin --delete release/v1.0.0
```

## 💬 沟通工具

### GitHub Issues
用于任务管理和问题跟踪：
- **Bug报告**: 描述问题，添加标签 `bug`
- **功能请求**: 描述新功能，添加标签 `enhancement`
- **任务分配**: 分配给特定团队成员

### GitHub Projects
创建看板管理项目进度：
1. 创建新Project
2. 设置列：Backlog → In Progress → Review → Done
3. 将Issues和Pull Requests添加到看板

### 标签系统
```
bug          # 错误修复
enhancement  # 功能增强
documentation # 文档更新
good first issue # 适合新手的任务
help wanted  # 需要帮助
```

## 📝 提交信息规范

### 提交类型
```
feat:     新功能
fix:      修复bug
docs:     文档更新
style:    代码格式调整
refactor: 代码重构
test:     测试相关
chore:    构建过程或辅助工具的变动
```

### 示例
```bash
git commit -m "feat: 添加玩家跳跃功能"
git commit -m "fix: 修复碰撞检测bug"
git commit -m "docs: 更新README文档"
```

## 🔧 开发环境设置

### 新团队成员设置
```bash
# 1. 克隆项目
git clone https://github.com/DejavuTung/3A_Tower.git
cd 3A_Tower

# 2. 安装Unity 2022.3 LTS
# 下载地址: https://unity.com/releases/2022.3

# 3. 打开项目
# 在Unity Hub中添加项目

# 4. 安装必要的包
# 通过Package Manager安装项目依赖
```

## 📊 进度跟踪

### 使用GitHub Insights
- 查看代码贡献统计
- 跟踪Issues解决进度
- 监控Pull Request合并情况

### 定期会议
- **每日站会**: 15分钟，同步进度
- **周会**: 1小时，回顾和规划
- **里程碑会议**: 每个版本发布前

## 🛠️ 工具推荐

### 开发工具
- **Unity 2022.3 LTS**: 游戏开发引擎
- **Visual Studio Code**: 代码编辑器
- **Git**: 版本控制
- **GitHub Desktop**: Git图形界面

### 沟通工具
- **Discord**: 团队语音沟通
- **Slack**: 文字沟通
- **Trello**: 任务管理（可选）

## 📚 学习资源

### Unity学习
- [Unity官方教程](https://learn.unity.com/)
- [Unity中文手册](https://docs.unity.cn/cn/current/)

### Git学习
- [Git官方文档](https://git-scm.com/doc)
- [GitHub Guides](https://guides.github.com/)

### 团队协作
- [GitHub Flow](https://guides.github.com/introduction/flow/)
- [敏捷开发实践](https://www.scrum.org/)

---

**记住**: 良好的沟通是成功协作的关键！保持开放和透明的沟通，及时更新项目状态。 