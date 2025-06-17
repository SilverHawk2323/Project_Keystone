using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Deploy,
    Battle,
    Pause,
}

public class GameManager : MonoBehaviour
{
    public GameState state;
    private GameState lastGameState;
    public EnemyAI enemyAI;
    private float timer;
    public List<UnitBase> friendlyUnits = new List<UnitBase>();
    public static GameManager gm;
    

    public bool isGameOver = false;
    private bool gamePaused;
    public PlayerManager playerManager;

    [Header("UI Variables")]
    public TMP_Text timerText;
    public TMP_Text gameStateText;
    public TMP_Text currentResourcesText;
    public Button endTurnButton;
    public GameObject pauseMenu;
    public GameObject loseScreen;
    public GameObject winScreen;
    public GameObject GUI;
    public Camera mc;

    private void Awake()
    {
        gm = gm == null ? this: gm;
        mc = Camera.main;
        playerManager = playerManager == null ? FindFirstObjectByType<PlayerManager>() : playerManager;
    }

    void Start()
    {
        SetGameStateToDeploy();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!gamePaused)//!isGameOver || state != GameState.Pause)
        {
            TimerCountDown();
            timerText.text = "Timer: " + Mathf.Round(timer); //* 100) / 100;
        }
        if (timer <= 0f)
        {
            switch (state)
            {
                case GameState.Deploy:
                    SetGameStateToBattle();
                    break;
                case GameState.Battle:
                    SetGameStateToDeploy();
                    break;
                case GameState.Pause:
                    SetGameStateToPause();
                    break;
                default:
                    SetGameStateToDeploy();
                    break;
            }
            return;
        }
        if(state == GameState.Pause)
        {
            SetGameStateToPause();
        }
    }

    public void StartGame()
    {
        SetGameStateToDeploy();
    }

    public void SetGameStateToDeploy()
    {
        state = GameState.Deploy;
        gamePaused = false;
        gameStateText.text = "State: " + state;
        timer = 60f;
        endTurnButton.gameObject.SetActive(true);
        playerManager.DeployPhase();
        currentResourcesText.text = "Resources: " + playerManager.currentResources;
        enemyAI.SpawnEnemy();
    }

    public void SetGameStateToBattle()
    {
        state = GameState.Battle;
        gamePaused = false;
        gameStateText.text = "State: " + state;
        timer = 15f;
        endTurnButton.gameObject.SetActive(false);
    }

    public void SetGameStateToPause()
    {
        /*if(state == GameState.Pause)
        {
            state = lastGameState;
            pauseMenu.SetActive(false);
            SetGUI(true);
            return;
        }
        lastGameState = state;
        state = GameState.Pause;*/
        pauseMenu.SetActive(true);
        gamePaused = true;
        SetGUI(false);
        SetCursor(true);
        //timer = 0f;
    }

    public void TimerCountDown()
    {
        timer -= Time.deltaTime;
        
    }

    public void SetGameOverScreen(int baseTeamNumber)
    {
        SetGUI(false);
        if(baseTeamNumber == 2)
        {
            winScreen.SetActive(true);
            SetCursor(true);
            state = GameState.Pause;
            isGameOver = true;
        }
        else
        {
            loseScreen.SetActive(true);
            SetCursor(true);
            state = GameState.Pause;
            isGameOver = true;
        }
    }

    public void SetCursor(bool cursorIsOn)
    {
        if (cursorIsOn)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SetGUI(bool visibility)
    {
        GUI.SetActive(visibility);
        SetCursor(true);
    }
}
