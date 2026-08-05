using UnityEngine;
using UnityEngine.SceneManagement;
public class InGameMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject menuPanel;

    [Header("Scenes")]
    [SerializeField] private string mainMenuScene = "MainMenu";

    private bool isOpen;

    private void Start()
    {
        CloseMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (isOpen)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    public void OpenMenu()
    {
        PlayerInput.InputEnabled = false;

        isOpen = true;

        menuPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        isOpen = false;

        PlayerInput.InputEnabled = true;
        
        menuPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void GiveUp()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuScene);
    }
}