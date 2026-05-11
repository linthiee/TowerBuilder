using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    private IEventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();

        _eventBus.Subscribe<StartGameEvent>(OnGameStarted);
        _eventBus.Subscribe<ExitToMenuEvent>(OnBackToMenu);
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.Unsubscribe<StartGameEvent>(OnGameStarted);
            _eventBus.Unsubscribe<ExitToMenuEvent>(OnBackToMenu);
        }
    }

    private void OnGameStarted(StartGameEvent eventData)
    {
        SceneManager.LoadScene("Game");
    }

    private void OnBackToMenu(ExitToMenuEvent eventData)
    {
        SceneManager.LoadScene("Menu");
    }
}