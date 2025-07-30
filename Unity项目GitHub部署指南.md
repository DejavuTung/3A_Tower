# 🚀 Unity项目GitHub部署指南

## 📋 概述
本指南将帮助您将Unity项目部署到GitHub上，实现团队协同开发。

## 🛠️ 环境准备

### 1. 安装Git
```bash
# 使用winget安装Git
winget install --id Git.Git -e --source winget

# 或者从官网下载：https://git-scm.com/downloads
```

### 2. 配置Git用户信息
```bash
git config --global user.name "您的GitHub用户名"
git config --global user.email "您的邮箱地址"
```

### 3. 安装GitHub Desktop（可选）
- 下载地址：https://desktop.github.com/
- 提供图形界面，更容易操作

## 📁 Unity项目Git配置

### 1. 创建.gitignore文件
在项目根目录创建`.gitignore`文件：

```gitignore
# Unity生成的文件夹
[Ll]ibrary/
[Tt]emp/
[Oo]bj/
[Bb]uild/
[Bb]uilds/
[Ll]ogs/
[Uu]ser[Ss]ettings/

# MemoryCaptures可以获取性能分析数据
[Mm]emoryCaptures/

# Asset meta data应该只在它们被忽略的文件被忽略时被忽略
!/[Aa]ssets/**/*.meta

# 自动生成的VS/MD/Consulo解决方案和项目文件
ExportedObj/
.consulo/
*.csproj
*.unityproj
*.sln
*.suo
*.tmp
*.user
*.userprefs
*.pidb
*.booproj
*.svd
*.pdb
*.mdb
*.opendb
*.VC.db

# Unity3D生成的元文件
*.pidb.meta
*.pdb.meta
*.mdb.meta

# Unity3D生成的崩溃报告文件
sysinfo.txt

# 构建
*.apk
*.aab
*.unitypackage
*.app

# Crashlytics生成的文件
crashlytics-build.properties

# 包管理器
Packages/
Packages/manifest.json
Packages/packages-lock.json

# 插件
Assets/Plugins/Editor/JetBrains*

# 本地历史记录
.vs/

# Gradle缓存目录
.gradle/

# MacOS
.DS_Store
```

### 2. 初始化Git仓库
```bash
# 进入Unity项目目录
cd "D:\3A_Game\3A_Tower"

# 初始化Git仓库
git init

# 添加所有文件到暂存区
git add .

# 提交初始版本
git commit -m "Initial commit: Unity 3A_Tower project"
```

## 🔗 连接GitHub

### 1. 创建GitHub仓库
1. 登录GitHub：https://github.com
2. 点击右上角"+"号 → "New repository"
3. 填写仓库信息：
   - Repository name: `3A_Tower`
   - Description: `Unity塔防游戏项目`
   - 选择Public或Private
   - 不要勾选"Initialize this repository with a README"
4. 点击"Create repository"

### 2. 连接本地仓库到GitHub
```bash
# 添加远程仓库
git remote add origin https://github.com/您的用户名/3A_Tower.git

# 推送到GitHub
git branch -M main
git push -u origin main
```

## 👥 协同开发工作流

### 1. 分支管理策略
```bash
# 主分支：main（稳定版本）
# 开发分支：develop（开发版本）
# 功能分支：feature/功能名称
# 修复分支：hotfix/问题描述

# 创建开发分支
git checkout -b develop
git push -u origin develop

# 创建功能分支
git checkout -b feature/new-tower
git push -u origin feature/new-tower
```

### 2. 日常开发流程
```bash
# 1. 拉取最新代码
git pull origin main

# 2. 创建功能分支
git checkout -b feature/your-feature

# 3. 开发并提交
git add .
git commit -m "feat: 添加新功能"

# 4. 推送到远程
git push origin feature/your-feature

# 5. 创建Pull Request
# 在GitHub网页上创建PR，请求合并到main分支
```

### 3. 版本发布流程
```bash
# 1. 确保代码稳定
git checkout main
git pull origin main

# 2. 创建发布标签
git tag -a v1.0.0 -m "Release version 1.0.0"
git push origin v1.0.0

# 3. 在GitHub上创建Release
# 在GitHub仓库页面点击"Releases" → "Create a new release"
```

## 🔧 Unity特定配置

### 1. Unity版本控制设置
1. 打开Unity项目
2. Edit → Project Settings → Editor
3. Version Control Mode: 选择"Visible Meta Files"
4. Asset Serialization Mode: 选择"Force Text"

### 2. 团队协作设置
```bash
# 创建团队开发规范文件
touch TEAM_WORKFLOW.md
```

### 3. 自动化脚本
创建部署脚本：

```bash
# deploy.sh (Linux/Mac)
#!/bin/bash
echo "开始部署Unity项目..."

# 检查Git状态
if [ -n "$(git status --porcelain)" ]; then
    echo "发现未提交的更改，正在提交..."
    git add .
    git commit -m "Auto commit: $(date)"
fi

# 推送到远程仓库
git push origin main
echo "部署完成！"
```

```powershell
# deploy.ps1 (Windows)
Write-Host "开始部署Unity项目..." -ForegroundColor Green

# 检查Git状态
$status = git status --porcelain
if ($status) {
    Write-Host "发现未提交的更改，正在提交..." -ForegroundColor Yellow
    git add .
    git commit -m "Auto commit: $(Get-Date)"
}

# 推送到远程仓库
git push origin main
Write-Host "部署完成！" -ForegroundColor Green
```

## 📊 项目管理工具

### 1. GitHub Issues
- 创建任务和Bug报告
- 使用标签分类：`bug`, `enhancement`, `feature`
- 分配负责人和里程碑

### 2. GitHub Projects
- 创建看板管理任务
- 设置自动化工作流
- 跟踪项目进度

### 3. GitHub Actions（可选）
创建`.github/workflows/unity-build.yml`：

```yaml
name: Unity Build

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v2
    
    - name: Cache Library
      uses: actions/cache@v2
      with:
        path: Library
        key: Library-${{ hashFiles('Assets/**', 'Packages/**', 'ProjectSettings/**') }}
        restore-keys: |
          Library-
    
    - name: Build Unity Project
      run: |
        # 这里添加Unity构建命令
        echo "构建Unity项目..."
```

## 🚨 注意事项

### 1. 文件大小限制
- GitHub单个文件限制100MB
- 大文件使用Git LFS：
```bash
# 安装Git LFS
git lfs install

# 跟踪大文件
git lfs track "*.psd"
git lfs track "*.png"
git lfs track "*.mp3"
git lfs track "*.wav"
```

### 2. 敏感信息保护
- 不要提交API密钥
- 使用环境变量
- 创建`.env.example`文件

### 3. 冲突解决
```bash
# 合并冲突
git merge feature/branch-name

# 解决冲突后
git add .
git commit -m "Resolve merge conflicts"
```

## 📈 最佳实践

### 1. 提交信息规范
```
feat: 新功能
fix: 修复Bug
docs: 文档更新
style: 代码格式调整
refactor: 代码重构
test: 测试相关
chore: 构建过程或辅助工具的变动
```

### 2. 定期备份
```bash
# 创建备份分支
git checkout -b backup/$(date +%Y%m%d)
git push origin backup/$(date +%Y%m%d)
```

### 3. 代码审查
- 所有PR必须经过审查
- 使用GitHub的Review功能
- 设置分支保护规则

## 🎯 快速开始命令

```bash
# 完整的一键部署流程
git init
git add .
git commit -m "Initial commit"
git branch -M main
git remote add origin https://github.com/用户名/3A_Tower.git
git push -u origin main
```

## 📞 团队协作建议

1. **定期同步**：每天至少pull一次最新代码
2. **及时沟通**：使用GitHub Issues和Discussions
3. **代码审查**：所有重要更改都要经过审查
4. **文档维护**：及时更新README和文档
5. **版本管理**：使用语义化版本号

这样设置后，您的Unity项目就可以在GitHub上进行高效的协同开发了！ 