using UnityEngine;
using UnityEngine.UI;

public class GameSpeedFixTest : MonoBehaviour
{
    private AutoTowerDefenseDemo gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<AutoTowerDefenseDemo>();
        if (gameManager == null)
        {
            Debug.LogError("GameSpeedFixTest: 未找到AutoTowerDefenseDemo组件");
            return;
        }
        Debug.Log("GameSpeedFixTest: 游戏速度修复测试准备就绪");
    }

    [ContextMenu("测试游戏速度修复")]
    void TestGameSpeedFix()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameSpeedFixTest: 游戏管理器未找到");
            return;
        }

        Debug.Log("GameSpeedFixTest: 开始测试游戏速度修复");

        // 测试不同游戏速度下的关键参数
        TestSpeedParameters(0.5f, "0.5X");
        TestSpeedParameters(1f, "1X");
        TestSpeedParameters(2f, "2X");
        TestSpeedParameters(4f, "4X");

        Debug.Log("GameSpeedFixTest: 游戏速度修复测试完成");
        Debug.Log("GameSpeedFixTest: 请检查：");
        Debug.Log("1. 防御塔攻击冷却时间应该随游戏速度变化");
        Debug.Log("2. 敌人移动速度应该随游戏速度变化");
        Debug.Log("3. 波次持续时间应该随游戏速度变化");
        Debug.Log("4. Archer动画速度应该随游戏速度变化");
        Debug.Log("5. 游戏结果应该在不同速度下保持一致");
    }

    void TestSpeedParameters(float speed, string speedName)
    {
        Debug.Log($"GameSpeedFixTest: 测试{speedName}速度参数");

        // 通过反射设置游戏速度
        var setGameSpeedMethod = typeof(AutoTowerDefenseDemo).GetMethod("SetGameSpeed", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (setGameSpeedMethod != null)
        {
            setGameSpeedMethod.Invoke(gameManager, new object[] { speed });
            
            // 获取关键字段值
            var gameSpeedField = typeof(AutoTowerDefenseDemo).GetField("gameSpeed", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var towerCooldownField = typeof(AutoTowerDefenseDemo).GetField("towerCooldown", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var waveDurationField = typeof(AutoTowerDefenseDemo).GetField("waveDuration", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var enemySpeedField = typeof(AutoTowerDefenseDemo).GetField("enemySpeed", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (gameSpeedField != null && towerCooldownField != null && waveDurationField != null && enemySpeedField != null)
            {
                float currentGameSpeed = (float)gameSpeedField.GetValue(gameManager);
                float towerCooldown = (float)towerCooldownField.GetValue(gameManager);
                float waveDuration = (float)waveDurationField.GetValue(gameManager);
                float enemySpeed = (float)enemySpeedField.GetValue(gameManager);

                Debug.Log($"GameSpeedFixTest: {speedName} - 游戏速度: {currentGameSpeed}");
                Debug.Log($"GameSpeedFixTest: {speedName} - 防御塔冷却时间: {towerCooldown}秒");
                Debug.Log($"GameSpeedFixTest: {speedName} - 实际攻击间隔: {towerCooldown / currentGameSpeed:F3}秒");
                Debug.Log($"GameSpeedFixTest: {speedName} - 波次持续时间: {waveDuration}秒");
                Debug.Log($"GameSpeedFixTest: {speedName} - 实际波次时间: {waveDuration / currentGameSpeed:F3}秒");
                Debug.Log($"GameSpeedFixTest: {speedName} - 敌人移动速度: {enemySpeed}");
                Debug.Log($"GameSpeedFixTest: {speedName} - 实际移动速度: {enemySpeed * currentGameSpeed:F3}");
            }
        }
    }

    [ContextMenu("测试游戏结果一致性")]
    void TestGameResultConsistency()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameSpeedFixTest: 游戏管理器未找到");
            return;
        }

        Debug.Log("GameSpeedFixTest: 开始测试游戏结果一致性");
        Debug.Log("GameSpeedFixTest: 请手动测试以下场景：");
        Debug.Log("1. 在1倍速下完成第二波，记录剩余敌人数量");
        Debug.Log("2. 在4倍速下完成第二波，记录剩余敌人数量");
        Debug.Log("3. 两个结果应该相同或非常接近");
        Debug.Log("GameSpeedFixTest: 如果结果不同，说明游戏速度仍然影响游戏结果");
    }

    [ContextMenu("检查关键时间参数")]
    void CheckKeyTimeParameters()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameSpeedFixTest: 游戏管理器未找到");
            return;
        }

        Debug.Log("GameSpeedFixTest: 检查关键时间参数");

        // 获取当前游戏速度
        var gameSpeedField = typeof(AutoTowerDefenseDemo).GetField("gameSpeed", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (gameSpeedField != null)
        {
            float currentGameSpeed = (float)gameSpeedField.GetValue(gameManager);
            Debug.Log($"GameSpeedFixTest: 当前游戏速度: {currentGameSpeed}X");

            // 检查防御塔攻击冷却
            var towerCooldownField = typeof(AutoTowerDefenseDemo).GetField("towerCooldown", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (towerCooldownField != null)
            {
                float towerCooldown = (float)towerCooldownField.GetValue(gameManager);
                float actualCooldown = towerCooldown / currentGameSpeed;
                Debug.Log($"GameSpeedFixTest: 防御塔冷却时间: {towerCooldown}秒 -> 实际间隔: {actualCooldown:F3}秒");
            }

            // 检查波次持续时间
            var waveDurationField = typeof(AutoTowerDefenseDemo).GetField("waveDuration", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (waveDurationField != null)
            {
                float waveDuration = (float)waveDurationField.GetValue(gameManager);
                float actualWaveDuration = waveDuration / currentGameSpeed;
                Debug.Log($"GameSpeedFixTest: 波次持续时间: {waveDuration}秒 -> 实际时间: {actualWaveDuration:F3}秒");
            }

            // 检查敌人移动速度
            var enemySpeedField = typeof(AutoTowerDefenseDemo).GetField("enemySpeed", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (enemySpeedField != null)
            {
                float enemySpeed = (float)enemySpeedField.GetValue(gameManager);
                float actualEnemySpeed = enemySpeed * currentGameSpeed;
                Debug.Log($"GameSpeedFixTest: 敌人移动速度: {enemySpeed} -> 实际速度: {actualEnemySpeed:F3}");
            }
        }
    }

    [ContextMenu("重置游戏速度")]
    void ResetGameSpeed()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameSpeedFixTest: 游戏管理器未找到");
            return;
        }

        Debug.Log("GameSpeedFixTest: 重置游戏速度为1X");
        var setGameSpeedMethod = typeof(AutoTowerDefenseDemo).GetMethod("SetGameSpeed", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (setGameSpeedMethod != null)
        {
            setGameSpeedMethod.Invoke(gameManager, new object[] { 1f });
            Debug.Log("GameSpeedFixTest: 游戏速度已重置为1X");
        }
    }
} 