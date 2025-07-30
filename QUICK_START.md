# 🚀 快速开始团队协作

## 📋 第一步：邀请团队成员

### 1. 添加协作者
1. 访问您的GitHub仓库：https://github.com/DejavuTung/3A_Tower
2. 点击 `Settings` 标签页
3. 在左侧菜单选择 `Collaborators and teams`
4. 点击 `Add people` 按钮
5. 输入团队成员的GitHub用户名或邮箱
6. 选择权限级别（建议选择 `Write`）

### 2. 创建团队（可选）
1. 在仓库设置中点击 `Teams`
2. 创建新团队，如 "3A_Tower_Developers"
3. 添加成员到团队

## 🔧 第二步：设置开发环境

### 新团队成员需要：
1. **安装Unity 2022.3 LTS**
   - 下载地址：https://unity.com/releases/2022.3
   - 选择 "Unity 2022.3 LTS" 版本

2. **克隆项目**
   ```bash
   git clone https://github.com/DejavuTung/3A_Tower.git
   cd 3A_Tower
   ```

3. **在Unity Hub中打开项目**
   - 打开Unity Hub
   - 点击 "Add" 按钮
   - 选择项目文件夹
   - 等待Unity导入项目

## 🌿 第三步：创建开发分支

### 开始新功能开发：
```bash
# 1. 确保在最新代码上
git checkout main
git pull origin main

# 2. 创建功能分支
git checkout -b feature/your-feature-name

# 3. 开始开发
# 在Unity中编写代码...
```

### 提交代码：
```bash
# 提交更改
git add .
git commit -m "feat: 添加新功能描述"

# 推送到远程
git push origin feature/your-feature-name
```

## 👀 第四步：创建Pull Request

### 完成功能后：
1. 在GitHub仓库页面点击 `Pull requests`
2. 点击 `New pull request`
3. 选择您的功能分支作为源分支
4. 选择 `main` 或 `develop` 作为目标分支
5. 填写PR标题和描述
6. 点击 `Create pull request`

### PR模板示例：
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

## 📝 第五步：代码审查

### 审查检查清单：
- [ ] 代码符合项目规范
- [ ] 功能按预期工作
- [ ] 添加了必要的注释
- [ ] 更新了相关文档
- [ ] 通过了所有测试
- [ ] 没有引入新的bug

### 审查流程：
1. 团队成员审查代码
2. 提出修改建议
3. 作者根据建议修改代码
4. 重新提交代码
5. 审查者批准后合并

## 🎯 第六步：使用GitHub Issues

### 创建任务：
1. 在仓库页面点击 `Issues`
2. 点击 `New issue`
3. 选择Issue模板或自定义
4. 填写标题和描述
5. 添加标签和分配负责人

### Issue模板示例：
```
标题: 实现玩家跳跃功能

描述:
## 需求
- 玩家按空格键可以跳跃
- 跳跃有动画效果
- 跳跃有音效

## 验收标准
- [ ] 按空格键可以跳跃
- [ ] 跳跃动画正常播放
- [ ] 跳跃音效正常播放
- [ ] 跳跃高度合理

## 技术细节
- 使用Unity的Rigidbody2D组件
- 添加跳跃力
- 实现跳跃动画
- 添加跳跃音效

## 标签
- enhancement
- gameplay
```

## 📊 第七步：项目进度跟踪

### 使用GitHub Projects：
1. 在仓库页面点击 `Projects`
2. 创建新Project
3. 设置看板列：Backlog → In Progress → Review → Done
4. 将Issues和Pull Requests添加到看板

### 定期同步：
- **每日站会**: 15分钟，同步进度
- **周会**: 1小时，回顾和规划
- **里程碑会议**: 每个版本发布前

## 🛠️ 常用命令

### Git命令：
```bash
# 查看状态
git status

# 查看分支
git branch

# 切换分支
git checkout branch-name

# 创建并切换分支
git checkout -b new-branch

# 拉取最新代码
git pull origin main

# 推送代码
git push origin branch-name

# 查看提交历史
git log --oneline
```

### Unity相关：
- 在Unity中修改代码后，记得保存场景
- 定期备份重要资源
- 使用Unity的版本控制设置

## 💡 最佳实践

### 代码规范：
- 使用有意义的变量名
- 添加必要的注释
- 遵循Unity的命名约定
- 定期重构代码

### 沟通规范：
- 及时更新任务状态
- 遇到问题及时沟通
- 分享学习心得
- 互相帮助解决问题

### 版本管理：
- 经常提交代码
- 写清晰的提交信息
- 及时处理冲突
- 保持分支整洁

## 🆘 遇到问题？

### 常见问题解决：
1. **代码冲突**: 使用 `git merge` 或 `git rebase` 解决
2. **Unity项目问题**: 检查Unity版本兼容性
3. **Git问题**: 查看Git文档或寻求帮助
4. **团队协作问题**: 及时沟通，寻求共识

### 获取帮助：
- 查看项目文档
- 在GitHub Issues中提问
- 团队内部讨论
- 查阅Unity和Git文档

---

**记住**: 团队协作需要耐心和沟通，保持开放的心态，互相学习，共同进步！ 