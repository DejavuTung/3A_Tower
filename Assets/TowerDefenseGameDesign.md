# 🏰 部落冲突风格塔防游戏设计文档

## 🎮 游戏概述

这是一款类似Supercell《部落冲突》的塔防游戏，玩家需要建造防御塔来保护基地免受敌人攻击。

## 🎯 核心玩法

### **防御方（玩家）**
- **建造防御塔**：在指定位置建造不同类型的防御塔
- **升级防御塔**：消耗金币提升防御塔的攻击力、范围和攻击速度
- **出售防御塔**：将不需要的防御塔出售获得金币
- **保护基地**：防止敌人摧毁基地

### **进攻方（AI）**
- **多波次攻击**：敌人分波次进攻，每波敌人数量和强度递增
- **多种敌人类型**：不同类型的敌人有不同的属性和行为
- **路径寻找**：敌人沿着预设路径向基地移动
- **攻击基地**：到达基地后对基地造成伤害

## 🏗️ 防御塔系统

### **1. 弓箭塔 (Archer Tower)**
- **特点**：平衡型，单体攻击
- **攻击范围**：3格
- **攻击速度**：1秒/次
- **伤害**：20
- **费用**：100金币
- **目标策略**：优先攻击血量最低的敌人

### **2. 炮塔 (Cannon Tower)**
- **特点**：范围攻击，高伤害
- **攻击范围**：2.5格
- **攻击速度**：1.5秒/次
- **伤害**：30
- **费用**：150金币
- **目标策略**：优先攻击最近的敌人

### **3. 魔法塔 (Magic Tower)**
- **特点**：减速效果，控制型
- **攻击范围**：4格
- **攻击速度**：0.8秒/次
- **伤害**：15
- **费用**：200金币
- **特殊效果**：攻击时减速敌人50%，持续3秒

### **4. 狙击塔 (Sniper Tower)**
- **特点**：超远距离，高伤害
- **攻击范围**：6格
- **攻击速度**：2秒/次
- **伤害**：50
- **费用**：300金币
- **目标策略**：优先攻击血量最低的敌人

### **5. 支援塔 (Support Tower)**
- **特点**：范围支援，增益效果
- **攻击范围**：5格
- **攻击速度**：3秒/次
- **伤害**：10
- **费用**：250金币
- **特殊效果**：对范围内所有敌人造成伤害并减速

## 👹 敌人系统

### **1. 战士 (Warrior)**
- **血量**：100
- **移动速度**：2
- **伤害**：10
- **金币奖励**：20
- **特点**：平衡型单位

### **2. 弓箭手 (Archer)**
- **血量**：80
- **移动速度**：1.5
- **伤害**：15
- **攻击范围**：3格
- **金币奖励**：25
- **特点**：远程攻击

### **3. 坦克 (Tank)**
- **血量**：200
- **移动速度**：1
- **伤害**：20
- **金币奖励**：30
- **特点**：高血量，慢速

### **4. 快速单位 (Fast)**
- **血量**：50
- **移动速度**：4
- **伤害**：5
- **金币奖励**：15
- **特点**：高速度，低血量

### **5. Boss**
- **血量**：500
- **移动速度**：1.5
- **伤害**：30
- **金币奖励**：100
- **特点**：高属性，每波次出现

## 💰 经济系统

### **金币获取**
- 消灭敌人获得金币
- 不同敌人类型提供不同金币奖励
- 波次越高级，敌人提供的金币越多

### **金币消耗**
- 建造防御塔
- 升级防御塔
- 出售防御塔获得50%建造费用

### **宝石系统**
- 特殊货币，用于购买特殊道具
- 可通过成就或充值获得

## 🎯 游戏进程

### **波次系统**
- 总共10波敌人
- 每波敌人数量递增：5 + 波次 × 2
- 敌人属性随波次增强
- 波次间隔：5秒

### **难度递增**
- 敌人血量：基础血量 × (1 + 波次 × 0.2)
- 敌人伤害：基础伤害 × (1 + 波次 × 0.2)
- 敌人速度：基础速度 × (1 + 波次 × 0.05)

### **胜利条件**
- 成功防御所有10波敌人
- 基地血量保持大于0

### **失败条件**
- 基地血量降至0

## 🎨 视觉设计

### **防御塔外观**
- 根据等级改变颜色
- 等级1：白色
- 等级2：绿色
- 等级3：蓝色
- 等级4：紫色
- 等级5：黄色

### **投射物效果**
- 弓箭塔：绿色箭矢
- 狙击塔：红色子弹
- 魔法塔：蓝色魔法弹
- 支援塔：黄色光球

### **特效系统**
- 建造特效
- 升级特效
- 攻击特效
- 击中特效
- 死亡特效

## 🎵 音频设计

### **音效**
- 建造音效
- 升级音效
- 攻击音效
- 击中音效
- 出售音效
- 胜利音效
- 失败音效

### **背景音乐**
- 游戏进行时：紧张刺激
- 建造模式：轻松愉快
- 胜利时：欢快庆祝
- 失败时：低沉悲伤

## 📱 移动端优化

### **触摸控制**
- 点击建造防御塔
- 长按查看防御塔信息
- 拖拽移动视角
- 双指缩放

### **UI适配**
- 响应式界面设计
- 大按钮易于点击
- 清晰的视觉反馈
- 简洁的信息显示

### **性能优化**
- 对象池管理
- 纹理压缩
- 音频压缩
- 内存管理

## 🚀 扩展功能

### **未来计划**
- 多种地图场景
- 更多防御塔类型
- 特殊技能系统
- 成就系统
- 排行榜
- 多人对战
- 皮肤系统
- 每日任务

### **技术特性**
- 模块化设计
- 事件驱动架构
- 数据驱动配置
- 易于扩展和修改 