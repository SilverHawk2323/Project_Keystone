using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Deploy,
    Battle,
    Pause,
}

public class GameManager : MonoBehaviour
{
    public GameState state;

    private float timer;
    public List<UnitBase> units = new List<UnitBase>();
    public static GameManager gm;

    public bool isGameOver = false;
    public PlayerManager playerManager;

    private void Awake()
    {
        gm = gm == null ? gm: null;
        playerManager = FindFirstObjectByType<PlayerManager>();
    }

    void Start()
    {
        state = GameState.Deploy;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver || state != GameState.Pause)
        {
            TimerCountDown();
        }

        
    }

    public void StartGame()
    {
        SetGameStateToDeploy();
    }

    public void SetGameStateToDeploy()
    {
        state = GameState.Deploy;
        timer = 60f;
    }

    public void SetGameStateToBattle()
    {
        state = GameState.Battle;
        timer = 30f;
    }

    public void SetGameStateToPause()
    {
        state = GameState.Pause;
        timer = 0f;
    }

    public void TimerCountDown()
    {
        timer += Time.deltaTime;
    }
}
