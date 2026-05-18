using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameDifficulty difficulty;

    [SerializeField] private PlayerScoreSO scoreSO;

    [SerializeField] private Pendulum player;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI strikesText;
    [SerializeField] private TextMeshProUGUI towerHeightText;

    private int towerHeight = 0;

    private IEventBus _eventBus;

    private void Awake()
    {     
        scoreSO.LoadSettings();
    }
    private void Start()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();

        _eventBus.Subscribe<ExitToMenuEvent>(OnBackToMenu);
        _eventBus.Subscribe<PerfectLandEvent>(OnPerfectLand);
        _eventBus.Subscribe<BlockLandedEvent>(OnBlockLand);
        _eventBus.Subscribe<EndGameEvent>(OnGameEnd);

        PrintHighscore();
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
        SceneManager.LoadScene("Menu");
    }

    public void OnGameEnd(EndGameEvent eventData)
    {
        Debug.Log("saving score...");
        scoreSO.CheckAndSaveHighScore(difficulty, player.score);
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
    }
}
