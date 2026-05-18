using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Instance singleton")]
    public static GameManager Instance { get; private set; }

    public GameDifficulty difficulty;

    [Header("Player info")]
    [SerializeField] private PlayerScoreSO scoreSO;
    [SerializeField] private Pendulum player;

    [Header("Player ingame info")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI strikesText;
    [SerializeField] private TextMeshProUGUI towerHeightText;

    [SerializeField] private GameObject defeatPannel;

    private int towerHeight = 0;

    private IEventBus _eventBus;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        TowerSlop.ResetTowerState();

        scoreSO.LoadSettings();
    }
    private void Start()
    {
        Application.wantsToQuit += WantsToQuit;

        _eventBus = ServiceLoader.GetService<IEventBus>();

        _eventBus.Subscribe<ExitToMenuEvent>(OnBackToMenu);
        _eventBus.Subscribe<PerfectLandEvent>(OnPerfectLand);
        _eventBus.Subscribe<BlockLandedEvent>(OnBlockLand);
        _eventBus.Subscribe<EndGameEvent>(OnGameEnd);

        PrintHighscore();
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.Unsubscribe<ExitToMenuEvent>(OnBackToMenu);
            _eventBus.Unsubscribe<PerfectLandEvent>(OnPerfectLand);
            _eventBus.Unsubscribe<BlockLandedEvent>(OnBlockLand);
            _eventBus.Unsubscribe<EndGameEvent>(OnGameEnd);
        }
        Application.wantsToQuit -= WantsToQuit;
    }

    private void PrintHighscore()
    {
        int savedRecord = scoreSO.GetHighScore(difficulty);

        if (player.score > savedRecord)
        {
            highScoreText.text = player.score.ToString();
        }
        else
        {
            highScoreText.text = savedRecord.ToString();
        }
    }

    private void OnBlockLand(BlockLandedEvent eventData)
    {
        if (eventData.groundLand || eventData.blockLand)
            player.strikes = 0;

        towerHeight++;
        player.score += eventData.points;

        PrintHighscore();

        scoreText.text = player.score.ToString();
        towerHeightText.text = towerHeight.ToString();
        strikesText.text = player.strikes.ToString();
    }

    private void OnPerfectLand(PerfectLandEvent eventData)
    {
        player.strikes++;
        player.score += eventData.points;
    }
    private void OnBackToMenu(ExitToMenuEvent eventData)
    {
        Debug.Log("OnBackToMenu called");
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("Menu");
    }

    public void OnGameEnd(EndGameEvent eventData)
    {
        scoreSO.CheckAndSaveHighScore(difficulty, player.score);
        Debug.Log("OnGameEnd called");

#if UNITY_EDITOR
        Time.timeScale = 1f;
        Debug.Log("quitting...");
        SceneManager.LoadSceneAsync("Menu");
#elif UNITY_WEBGL
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("Menu");
#else
        Application.Quit();
#endif
    }

    private bool WantsToQuit()
    {
        scoreSO.CheckAndSaveHighScore(difficulty, player.score);
        return true;
    }
}
