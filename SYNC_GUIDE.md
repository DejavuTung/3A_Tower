# 🔄 代码同步指南

## 📋 同步流程

### 每日工作流程

#### 开始工作前：
```bash
# 1. 拉取最新代码
git pull origin main

# 2. 创建功能分支（如果需要）
git checkout -b feature/your-feature-name
```

#### 工作过程中：
```bash
# 定期提交代码
git add .
git commit -m "feat: 添加新功能"
git push origin feature/your-feature-name
```

#### 完成工作后：
```bash
# 1. 提交最终代码
git add .
git commit -m "feat: 完成功能开发"

# 2. 推送到远程分支
git push origin feature/your-feature-name

# 3. 在GitHub创建Pull Request
```

## 🛠️ 使用同步脚本

### 安装脚本
项目根目录已包含 `sync.ps1` 脚本

### 使用方法
```bash
# 推送代码到GitHub
.\sync.ps1 push

# 从GitHub拉取最新代码
.\sync.ps1 pull
```

## ⚠️ 注意事项

### 1. 冲突处理
如果遇到合并冲突：
```bash
# 1. 查看冲突文件
git status

# 2. 手动解决冲突
# 编辑冲突文件，删除冲突标记

# 3. 添加解决后的文件
git add .

# 4. 完成合并
git commit -m "fix: 解决合并冲突"
```

### 2. 分支管理
```bash
# 查看所有分支
git branch -a

# 切换分支
git checkout branch-name

# 删除本地分支
git branch -d branch-name

# 删除远程分支
git push origin --delete branch-name
```

### 3. 提交信息规范
```
feat:     新功能
fix:      修复bug
docs:     文档更新
style:    代码格式调整
refactor: 代码重构
test:     测试相关
chore:    构建过程或辅助工具的变动
```

## 🔄 自动同步方案

### 方案1：Git Hooks（高级用户）
在 `.git/hooks/` 目录下创建钩子脚本

### 方案2：IDE集成
- **Visual Studio Code**: 安装Git插件
- **Unity**: 使用Unity的版本控制设置

### 方案3：定时任务
设置定时任务自动同步（不推荐，可能推送未完成的代码）

## 📊 同步状态检查

### 检查本地状态
```bash
# 查看修改状态
git status

# 查看提交历史
git log --oneline

# 查看远程分支
git branch -r
```

### 检查远程状态
```bash
# 查看远程仓库信息
git remote -v

# 获取远程更新信息
git fetch origin
```

## 🚨 常见问题

### 1. 推送失败
```bash
# 强制推送（谨慎使用）
git push --force-with-lease origin main
```

### 2. 拉取失败
```bash
# 重置到远程状态
git fetch origin
git reset --hard origin/main
```

### 3. 大文件问题
```bash
# 使用Git LFS
git lfs track "*.psd"
git lfs track "*.png"
git add .gitattributes
```

## 💡 最佳实践

### 1. 频繁提交
- 每完成一个小功能就提交
- 提交信息要清晰明确
- 避免一次性提交大量修改

### 2. 分支管理
- 主分支保持稳定
- 功能开发使用独立分支
- 及时删除已合并的分支

### 3. 沟通协调
- 团队成员及时沟通
- 避免同时修改同一文件
- 使用Pull Request进行代码审查

### 4. 备份策略
- 定期推送到GitHub
- 重要修改及时备份
- 使用标签标记重要版本

---

**记住**: Git是分布式版本控制系统，需要手动操作来同步代码。养成定期同步的习惯，确保团队协作顺畅！ 