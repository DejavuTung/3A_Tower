# 👥 团队协作工作流规范

## 📋 概述

本文档定义了3A_Tower项目的团队协作规范，确保团队成员能够高效、有序地进行开发工作。

## 🏗️ 项目架构

### 分支结构
```
main (主分支)
├── develop (开发分支)
│   ├── feature/new-tower (新防御塔功能)
│   ├── feature/ui-improvement (UI改进)
│   ├── feature/audio-system (音频系统)
│   └── hotfix/critical-bug (紧急修复)
```

### 分支命名规范
- **功能分支**: `feature/功能描述`
- **修复分支**: `hotfix/问题描述`
- **发布分支**: `release/版本号`

## 🔄 开发工作流

### 1. 日常开发流程

#### 开始新功能开发
```bash
# 1. 确保本地代码是最新的
git checkout develop
git pull origin develop

# 2. 创建功能分支
git checkout -b feature/your-feature-name

# 3. 开始开发...
```

#### 提交代码
```bash
# 1. 检查更改
git status

# 2. 添加文件
git add .

# 3. 提交代码（使用规范的提交信息）
git commit -m "feat: 添加新防御塔类型"

# 4. 推送到远程
git push origin feature/your-feature-name
```

#### 创建Pull Request
1. 在GitHub上创建Pull Request
2. 选择从`feature/your-feature-name`到`develop`
3. 填写详细的PR描述
4. 添加相关标签
5. 请求代码审查

### 2. 代码审查流程

#### 审查者职责
- 检查代码质量和规范
- 验证功能实现
- 确保没有引入Bug
- 提供建设性反馈

#### 审查标准
- ✅ 代码符合项目规范
- ✅ 功能实现正确
- ✅ 没有明显的性能问题
- ✅ 添加了必要的注释
- ✅ 更新了相关文档

### 3. 发布流程

#### 准备发布
```bash
# 1. 确保develop分支稳定
git checkout develop
git pull origin develop

# 2. 合并到main分支
git checkout main
git merge develop

# 3. 创建版本标签
git tag -a v1.0.0 -m "Release version 1.0.0"
git push origin v1.0.0
```

## 📝 提交信息规范

### 格式
```
<类型>(<范围>): <描述>

[可选的详细描述]

[可选的脚注]
```

### 类型说明
- `feat`: 新功能
- `fix`: Bug修复
- `docs`: 文档更新
- `style`: 代码格式调整
- `refactor`: 代码重构
- `test`: 测试相关
- `chore`: 构建过程或辅助工具变动

### 示例
```
feat(tower): 添加激光塔防御塔类型

- 实现激光塔的基础攻击逻辑
- 添加激光塔的UI图标
- 更新塔防系统以支持新塔类型

Closes #123
```

## 🎯 任务管理

### GitHub Issues使用规范

#### Issue类型
- 🐛 **Bug**: 程序错误或异常
- ✨ **Feature**: 新功能请求
- 📝 **Documentation**: 文档相关
- 🎨 **Enhancement**: 功能改进
- 🔧 **Maintenance**: 维护任务

#### Issue模板
```markdown
## 问题描述
[详细描述问题或需求]

## 复现步骤
1. 
2. 
3. 

## 预期行为
[描述期望的结果]

## 实际行为
[描述实际发生的情况]

## 环境信息
- Unity版本: 
- 操作系统: 
- 其他相关信息: 
```

### 标签使用
- `bug`: 程序错误
- `enhancement`: 功能改进
- `feature`: 新功能
- `documentation`: 文档
- `help wanted`: 需要帮助
- `good first issue`: 适合新手
- `priority: high`: 高优先级
- `priority: medium`: 中优先级
- `priority: low`: 低优先级

## 📊 代码质量规范

### C#编码规范
- 使用PascalCase命名类和方法
- 使用camelCase命名变量和参数
- 添加XML文档注释
- 遵循SOLID原则

### Unity特定规范
- 脚本文件名与类名一致
- 使用[SerializeField]而不是public
- 合理使用MonoBehaviour生命周期方法
- 避免在Update中执行复杂计算

### 示例代码
```csharp
/// <summary>
/// 防御塔基类
/// </summary>
public abstract class Tower : MonoBehaviour
{
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackSpeed = 1f;
    
    protected virtual void Start()
    {
        InitializeTower();
    }
    
    protected virtual void Update()
    {
        if (CanAttack())
        {
            Attack();
        }
    }
    
    protected abstract void Attack();
    
    private bool CanAttack()
    {
        // 实现攻击条件检查
        return true;
    }
    
    private void InitializeTower()
    {
        // 初始化塔防逻辑
    }
}
```

## 🤝 团队沟通

### 沟通渠道
- **GitHub Issues**: 任务和问题讨论
- **GitHub Discussions**: 技术讨论和决策
- **Pull Request**: 代码审查和反馈
- **README**: 项目文档和说明

### 会议安排
- **每日站会**: 简短的状态更新
- **周例会**: 进度回顾和计划
- **代码审查会**: 重要功能的代码审查

### 决策流程
1. 在GitHub Discussions中提出建议
2. 团队成员讨论和投票
3. 达成共识后实施
4. 更新相关文档

## 📈 项目进度跟踪

### 里程碑管理
- 使用GitHub Milestones跟踪版本进度
- 设置合理的截止日期
- 定期更新进度状态

### 进度报告
- 每周提交进度报告
- 记录完成的功能和遇到的问题
- 更新项目时间线

## 🚨 紧急情况处理

### 热修复流程
```bash
# 1. 从main分支创建热修复分支
git checkout main
git checkout -b hotfix/critical-bug

# 2. 修复问题
# ... 修复代码 ...

# 3. 提交修复
git add .
git commit -m "fix: 修复关键Bug"

# 4. 合并到main和develop
git checkout main
git merge hotfix/critical-bug
git push origin main

git checkout develop
git merge hotfix/critical-bug
git push origin develop

# 5. 删除热修复分支
git branch -d hotfix/critical-bug
```

### 回滚流程
```bash
# 1. 找到要回滚的提交
git log --oneline

# 2. 创建回滚提交
git revert <commit-hash>

# 3. 推送到远程
git push origin main
```

## 📚 学习资源

### 推荐阅读
- [GitHub Flow](https://guides.github.com/introduction/flow/)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Unity Best Practices](https://unity.com/learn/best-practices)

### 工具推荐
- **GitHub Desktop**: 图形化Git客户端
- **Visual Studio Code**: 代码编辑器
- **Unity Hub**: Unity项目管理

## 📞 联系方式

- **项目负责人**: [姓名] - [邮箱]
- **技术负责人**: [姓名] - [邮箱]
- **团队群组**: [群组链接]

---

**记住**: 良好的协作需要每个人的努力，让我们共同努力打造优秀的游戏项目！ 