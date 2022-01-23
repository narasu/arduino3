using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameState
{
    protected GameFSM owner;
    protected GameManager gameManager;

    public void Initialize(GameFSM _owner)
    {
        owner = _owner;
        gameManager = _owner.Owner;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();

}

public class BeginState : GameState
{
    private float timer = 1.0f;

    public override void Enter()
    {
        gameManager.beginOverlay.SetActive(true);
        Debug.Log("BeginState");
    }
    public override void Update()
    {
        if (timer>0.0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (Vector3.Distance(gameManager.StartPosition, gameManager.arduino.Position) <= 0.25f) 
            {
                owner.GotoState(GameStateType.STATE_PLAYING);
            }
        }
    }
    public override void Exit()
    {
        timer = 1.0f;
        gameManager.beginOverlay.SetActive(false);
    }
}
public class WaitState : GameState
{
    private float timer = 3.0f;

    public override void Enter()
    {
        gameManager.waitText.SetActive(true);
        Debug.Log("WaitState");
    }
    public override void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (Vector3.Distance(gameManager.StartPosition, gameManager.arduino.Position) <= 0.25f)
            {
                owner.GotoState(GameStateType.STATE_PLAYING);
            }
        }
        gameManager.countdownText.text = Mathf.Ceil(timer).ToString();
    }
    public override void Exit()
    {
        timer = 3.0f;
        gameManager.waitText.SetActive(false);
    }
}
public class PlayingState : GameState
{
    public override void Enter()
    {
        gameManager.playingOverlay.SetActive(true);
        gameManager.player.GetComponent<PlayerMovement>().enabled = true;
        Debug.Log("PlayingState");
    }
    public override void Update()
    {
    }
    public override void Exit()
    {
        gameManager.playingOverlay.SetActive(false);
        gameManager.player.GetComponent<PlayerMovement>().enabled = false;
    }
}
public class DeadState : GameState
{
    private float timer = 2.0f;
    public override void Enter()
    {
        gameManager.deadText.SetActive(true);
        Debug.Log("DeadState");
    }
    public override void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            owner.GotoState(GameStateType.STATE_BEGIN);
        }
    }
    public override void Exit()
    {
        timer = 2.0f;
        gameManager.deadText.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
public class WinState : GameState
{
    private float timer = 1.0f;

    public override void Enter()
    {
        gameManager.winText.SetActive(true);
        Debug.Log("WinState");
    }
    public override void Update()
    {
        //if (timer > 0.0f)
        //{
        //    timer -= Time.deltaTime;
        //}
        //else
        //{
        //    //go to next scene
        //}
    }
    public override void Exit()
    {
        timer = 1.0f;
        gameManager.winText.SetActive(false);
    }
}
