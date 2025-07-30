# Unity项目GitHub部署脚本
# 使用方法: .\deploy.ps1

param(
    [string]$CommitMessage = "Auto commit: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')",
    [string]$Branch = "main"
)

Write-Host "🚀 开始部署Unity项目到GitHub..." -ForegroundColor Green

# 检查Git是否安装
try {
    $gitVersion = git --version
    Write-Host "✅ Git已安装: $gitVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Git未安装，请先安装Git" -ForegroundColor Red
    Write-Host "下载地址: https://git-scm.com/downloads" -ForegroundColor Yellow
    exit 1
}

# 检查是否在Git仓库中
if (-not (Test-Path ".git")) {
    Write-Host "📁 初始化Git仓库..." -ForegroundColor Yellow
    git init
}

# 检查Git状态
$status = git status --porcelain
if ($status) {
    Write-Host "📝 发现未提交的更改，正在提交..." -ForegroundColor Yellow
    Write-Host "提交信息: $CommitMessage" -ForegroundColor Cyan
    
    # 添加所有文件
    git add .
    
    # 提交更改
    git commit -m $CommitMessage
    
    Write-Host "✅ 更改已提交" -ForegroundColor Green
} else {
    Write-Host "ℹ️ 没有发现未提交的更改" -ForegroundColor Blue
}

# 检查远程仓库
$remotes = git remote -v
if (-not $remotes) {
    Write-Host "⚠️ 未配置远程仓库" -ForegroundColor Yellow
    Write-Host "请先运行以下命令配置远程仓库:" -ForegroundColor Cyan
    Write-Host "git remote add origin https://github.com/您的用户名/3A_Tower.git" -ForegroundColor White
    exit 1
}

# 推送到远程仓库
Write-Host "📤 推送到远程仓库..." -ForegroundColor Yellow
try {
    git push origin $Branch
    Write-Host "✅ 成功推送到GitHub!" -ForegroundColor Green
} catch {
    Write-Host "❌ 推送失败，请检查网络连接和权限" -ForegroundColor Red
    exit 1
}

Write-Host "🎉 部署完成!" -ForegroundColor Green
Write-Host "📊 查看项目: https://github.com/您的用户名/3A_Tower" -ForegroundColor Cyan 