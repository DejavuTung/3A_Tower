using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏管理器 - 负责管理游戏的整体状态和资源
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("游戏设置")]
    public bool isGamePaused = false;
    public int currentScore = 0;
    public int highScore = 0;
    
    [Header("音频管理")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    [Header("UI引用")]
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    
    // 单例模式
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        // 确保只有一个GameManager实例
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // 初始化游戏
        InitializeGame();
    }
    
    /// <summary>
    /// 初始化游戏
    /// </summary>
    private void InitializeGame()
    {
        // 加载最高分
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        
        // 设置音频源
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();
            
        // 设置音频源属性
        musicSource.loop = true;
        sfxSource.loop = false;
    }
    
    /// <summary>
    /// 暂停/恢复游戏
    /// </summary>
    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
        
        if (pauseMenu != null)
            pauseMenu.SetActive(isGamePaused);
    }
    
    /// <summary>
    /// 增加分数
    /// </summary>
    public void AddScore(int points)
    {
        currentScore += points;
        
        // 检查是否破纪录
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }
    
    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource != null && clip != null)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }
    
    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void RestartGame()
    {
        currentScore = 0;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    /// <summary>
    /// 游戏结束
    /// </summary>
    public void GameOver()
    {
        if (gameOverMenu != null)
            gameOverMenu.SetActive(true);
    }
} 