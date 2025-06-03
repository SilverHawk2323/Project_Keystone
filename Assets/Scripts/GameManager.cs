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
    public EnemyAI enemyAI;
    private float timer;
    public List<UnitBase> friendlyUnits = new List<UnitBase>();
    public static GameManager gm;

    public bool isGameOver = false;
    public PlayerManager playerManager;

    [Header("UI Variables")]
    public TMP_Text timerText;
    public TMP_Text gameStateText;
    public Button endTurnButton;
    private void Awake()
    {
        gm = gm == null ? this: null;
        playerManager = playerManager == null ? FindFirstObjectByType<PlayerManager>() : playerManager;
    }

    void Start()
    {
        SetGameStateToDeploy();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver || state != GameState.Pause)
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
                    break;
                default:
                    SetGameStateToDeploy();
                    break;
            }
        }
        
    }

    public void StartGame()
    {
        SetGameStateToDeploy();
    }

    public void SetGameStateToDeploy()
    {
        state = GameState.Deploy;
        gameStateText.text = "State: " + state;
        timer = 60f;
        endTurnButton.gameObject.SetActive(true);
        playerManager.DrawCards();
        enemyAI.SpawnEnemy();
    }

    public void SetGameStateToBattle()
    {
        state = GameState.Battle;
        gameStateText.text = "State: " + state;
        timer = 30f;
        endTurnButton.gameObject.SetActive(false);
    }

    public void SetGameStateToPause()
    {
        state = GameState.Pause;
        //timer = 0f;
    }

    public void TimerCountDown()
    {
        timer -= Time.deltaTime;
        
    }
}
