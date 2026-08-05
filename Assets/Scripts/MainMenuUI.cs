using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameModePanel;
    [SerializeField] private GameObject packSelectionPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Settings")]
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject controlsPanel;

    [Header("Scenes")]
    [SerializeField] private string gameScene = "Bootstrap";

    private void Start()
    {
        ShowMainMenu();
    }

    #region Main Menu

    public void Play()
    {
        ShowGameModes();
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        gameModePanel.SetActive(false);
        packSelectionPanel.SetActive(false);

        settingsPanel.SetActive(true);

        ShowAudio();
    }

    public void ExitSettings()
    {
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);

        gameModePanel.SetActive(false);
        packSelectionPanel.SetActive(false);

        settingsPanel.SetActive(false);
    }

    #endregion

    #region Game Modes

    public void ShowGameModes()
    {
        mainMenuPanel.SetActive(false);

        gameModePanel.SetActive(true);
        packSelectionPanel.SetActive(false);

        settingsPanel.SetActive(false);
    }

    public void ShowPackSelection()
    {
        gameModePanel.SetActive(false);
        packSelectionPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        ShowMainMenu();
    }

    public void BackToGameModes()
    {
        gameModePanel.SetActive(true);
        packSelectionPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    #endregion

    #region Settings

    public void ShowAudio()
    {
        audioPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    public void ShowControls()
    {
        audioPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    #endregion

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}