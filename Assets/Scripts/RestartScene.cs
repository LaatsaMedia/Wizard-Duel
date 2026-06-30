using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    [SerializeField] private KeyCode restartKey = KeyCode.R;

    private void Update()
    {
        if (Input.GetKeyDown(restartKey))
            SceneRestart();
    }

    public void SceneRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}