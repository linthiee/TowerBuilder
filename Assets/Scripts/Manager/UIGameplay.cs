using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class UIGameplay : MonoBehaviour
{
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject panelSettings;

    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonExit;

    [SerializeField] private Button buttonSettingsExit;

    private IEventBus _eventBus;

    private bool isPaused = false;
    private void Awake()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();

        buttonSettings.onClick.AddListener(OnSettingsClicked);
        buttonExit.onClick.AddListener(OnExitClicked);

        buttonSettingsExit.onClick.AddListener(OnBackFromSettingsClicked);

        panelPause.SetActive(false);
        panelSettings.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }
    private void OnDestroy()
    {
        buttonSettings.onClick.RemoveAllListeners();
        buttonExit.onClick.RemoveAllListeners();
        buttonSettingsExit.onClick.RemoveAllListeners();
    }
    private void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0.0f : 1.0f;
        panelPause.SetActive(isPaused);

        if (!isPaused)
        {
            panelSettings.SetActive(false);
        }
    }

    private void OnBackFromSettingsClicked()
    {
        panelSettings.SetActive(false);
        panelPause.SetActive(true);
    }

    private void OnExitClicked()
    {
        if (isPaused)
        {
            Time.timeScale = 1.0f;
            _eventBus.Publish(new ExitToMenuEvent());
        }
    }
    private void OnSettingsClicked()
    {
        if (isPaused)
        {
            panelPause.SetActive(false);
            panelSettings.SetActive(true);
        }
    }
}
