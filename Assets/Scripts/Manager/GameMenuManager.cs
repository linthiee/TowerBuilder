using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    private IEventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();

        _eventBus.Subscribe<EasyLevelEvent>(OnEasyLevelSelected);
        _eventBus.Subscribe<MediumLevelEvent>(OnMediumLevelSelected);
        _eventBus.Subscribe<HardLevelEvent>(OnHardLevelSelected);

        _eventBus.Subscribe<ExitToMenuEvent>(OnBackToMenu);
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.Unsubscribe<EasyLevelEvent>(OnEasyLevelSelected);
            _eventBus.Unsubscribe<MediumLevelEvent>(OnMediumLevelSelected);
            _eventBus.Unsubscribe<HardLevelEvent>(OnHardLevelSelected);
            _eventBus.Unsubscribe<ExitToMenuEvent>(OnBackToMenu);
        }
    }

    private void OnEasyLevelSelected(EasyLevelEvent eventData)
    {
        SceneManager.LoadScene("LevelEasy");
    }

    private void OnMediumLevelSelected(MediumLevelEvent eventData)
    {
        SceneManager.LoadScene("LevelMedium");
    }

    private void OnHardLevelSelected(HardLevelEvent eventData)
    {
        SceneManager.LoadScene("LevelHard");
    }

    private void OnBackToMenu(ExitToMenuEvent eventData)
    {
        SceneManager.LoadScene("Menu");
    }
}