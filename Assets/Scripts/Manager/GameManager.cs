using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Pendulum player;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI strikesText;
    [SerializeField] private TextMeshProUGUI towerHeightText;

    private int towerHeight = 0;

    private IEventBus _eventBus;
    private void Start()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();

        _eventBus.Subscribe<ExitToMenuEvent>(OnBackToMenu);
        _eventBus.Subscribe<PerfectLandEvent>(OnPerfectLand);
        _eventBus.Subscribe<BlockLandedEvent>(OnBlockLand);
        _eventBus.Subscribe<UpdateCanvasEvent>(OnUpdateCanvas);
    }

    private void OnBlockLand(BlockLandedEvent eventData)
    {
        if (eventData.groundLand || eventData.blockLand)
            player.strikes = 0;

        towerHeight++;
        player.score += eventData.points;

        scoreText.text = player.score.ToString();
        towerHeightText.text = towerHeight.ToString();
        strikesText.text = player.strikes.ToString();
    }

    private void OnPerfectLand(PerfectLandEvent eventData)
    {
        player.strikes++;
        player.score += eventData.points;
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.Unsubscribe<ExitToMenuEvent>(OnBackToMenu);
            _eventBus.Unsubscribe<PerfectLandEvent>(OnPerfectLand);
            _eventBus.Unsubscribe<BlockLandedEvent>(OnBlockLand);
            _eventBus.Unsubscribe<UpdateCanvasEvent>(OnUpdateCanvas);
        }
    }

    private void OnUpdateCanvas(UpdateCanvasEvent eventData)
    {

    }

    private void OnBackToMenu(ExitToMenuEvent eventData)
    {
        SceneManager.LoadScene("Menu");
    }
}
