using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    
    public void RestartLevel()
    {
        SceneManager.LoadScene("BattleLevel");
        GameManager.gm.SetCursor(true);
    }

    public void QuitGame()
    {
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
}
