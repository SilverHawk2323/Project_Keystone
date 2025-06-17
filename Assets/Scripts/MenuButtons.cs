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
        GameManager.gm.SetCursor(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        GameManager.gm.SetCursor(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("BattleLevel");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
