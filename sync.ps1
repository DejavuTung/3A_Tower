# 3A_Tower 项目同步脚本
# 使用方法: .\sync.ps1 [push|pull]

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("push", "pull")]
    [string]$Action
)

Write-Host "=== 3A_Tower 项目同步脚本 ===" -ForegroundColor Green

if ($Action -eq "push") {
    Write-Host "正在推送代码到GitHub..." -ForegroundColor Yellow
    
    # 检查是否有未提交的修改
    $status = git status --porcelain
    if ($status) {
        Write-Host "发现未提交的修改:" -ForegroundColor Cyan
        git status
        
        # 添加所有修改
        git add .
        
        # 获取提交信息
        $commitMsg = Read-Host "请输入提交信息"
        if (-not $commitMsg) {
            $commitMsg = "feat: 自动同步修改"
        }
        
        # 提交
        git commit -m $commitMsg
        
        # 推送
        git push origin main
        
        Write-Host "✅ 代码已成功推送到GitHub!" -ForegroundColor Green
    } else {
        Write-Host "没有需要推送的修改" -ForegroundColor Yellow
    }
}
elseif ($Action -eq "pull") {
    Write-Host "正在从GitHub拉取最新代码..." -ForegroundColor Yellow
    
    # 保存当前修改（如果有）
    $status = git status --porcelain
    if ($status) {
        Write-Host "发现本地修改，正在暂存..." -ForegroundColor Cyan
        git stash
    }
    
    # 拉取最新代码
    git pull origin main
    
    # 恢复暂存的修改（如果有）
    if ($status) {
        Write-Host "正在恢复本地修改..." -ForegroundColor Cyan
        git stash pop
    }
    
    Write-Host "✅ 代码已成功从GitHub更新!" -ForegroundColor Green
}

Write-Host "同步完成!" -ForegroundColor Green 