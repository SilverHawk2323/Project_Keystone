using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MenuButtons : MonoBehaviour
{
    
    public void RestartLevel()
    {
        SceneManager.LoadScene("BattleLevel");
        Time.timeScale = 1.0f;
        GameManager.gm.SetCursor(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ResumeGame()
    {
        GameManager.gm.pauseMenu.SetActive(false);
        GameManager.gm.GUI.SetActive(true);
        Time.timeScale = 1.0f;
        GameManager.gm.SetCursor(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1.0f;
        GameManager.gm.SetCursor(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("BattleLevel");
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1.0f;
        Cursor.visible = true;
    }
}
