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

    public static GameManager gm;

    public bool isGameOver = false;

    private void Awake()
    {
        gm = gm == null ? gm: null;
    }

    void Start()
    {
        state = GameState.Pause;
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
