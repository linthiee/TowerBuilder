using UnityEngine;
using UnityEngine.UI;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private Button buttonPlay;

    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonSettingsExit;

    [SerializeField] private Button buttonCredits;
    [SerializeField] private Button buttonCreditsExit;

    [SerializeField] private Button buttonExit;

    [SerializeField] private GameObject panelMain;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject panelCredits;

    private IEventBus _eventBus;

    private void Awake()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();

        buttonPlay.onClick.AddListener(OnPlayClicked);

        buttonSettings.onClick.AddListener(OnSettingsClicked);
        buttonSettingsExit.onClick.AddListener(OnPanelExitClicked);

        buttonCredits.onClick.AddListener(OnCreditsClicked);
        buttonCreditsExit.onClick.AddListener(OnPanelExitClicked);

        buttonExit.onClick.AddListener(OnExitClicked);
    }

    private void OnPanelExitClicked()
    {
        panelSettings.SetActive(false);
        panelCredits.SetActive(false);
        panelMain.SetActive(true);
    }

    private void OnPlayClicked()
    {
        _eventBus.Publish(new StartGameEvent());

        gameObject.SetActive(false);
    }

    private void OnSettingsClicked()
    {
        panelMain.SetActive(false);
        panelSettings.SetActive(true);
    }

    private void OnCreditsClicked()
    {
        panelMain.SetActive(false);
        panelCredits.SetActive(true);
    }

    private void OnExitClicked()
    {
        Debug.Log("quitting");
        Application.Quit();
    }
    private void OnDestroy()
    {
        buttonPlay.onClick.RemoveAllListeners();
        buttonSettings.onClick.RemoveAllListeners();
        buttonSettingsExit.onClick.RemoveAllListeners();
        buttonCreditsExit.onClick.RemoveAllListeners();
        buttonExit.onClick.RemoveAllListeners();
    }
}
