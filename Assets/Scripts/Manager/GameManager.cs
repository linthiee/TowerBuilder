using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private IEventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();

        _eventBus.Subscribe<StartGameEvent>(OnGameStarted);
    }

    private void OnGameStarted(StartGameEvent eventData)
    {
        SceneManager.LoadScene("Game");
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
            _eventBus.Unsubscribe<StartGameEvent>(OnGameStarted);
    }
}