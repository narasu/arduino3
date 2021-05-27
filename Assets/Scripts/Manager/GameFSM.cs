using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFSM
{
    public GameManager Owner
    {
        get => owner;
    }
    private GameManager owner;

    private Dictionary<GameStateType, GameState> states;
    public GameStateType CurrentStateType
    {
        get => currentStateType;
    }
    private GameStateType currentStateType;
    private GameState currentState;
    private GameState previousState;
    
    public void Initialize(GameManager _owner)
    {
        owner = _owner;
        //states = new Dictionary<GameStateType, GameState>()
        //{
        //    {GameStateType.STATE_BEGIN, new BeginState() },
        //    {GameStateType.STATE_WAIT, new WaitState() },
        //    {GameStateType.STATE_PLAYING, new PlayingState() },
        //    {GameStateType.STATE_DEAD, new DeadState() },
        //    {GameStateType.STATE_WIN, new WinState() }
        //};

        states = new Dictionary<GameStateType, GameState>();
        AddState(GameStateType.STATE_BEGIN, new BeginState());
        AddState(GameStateType.STATE_WAIT, new WaitState());
        AddState(GameStateType.STATE_PLAYING, new PlayingState());
        AddState(GameStateType.STATE_DEAD, new DeadState());
        AddState(GameStateType.STATE_WIN, new WinState());
    }

    public void AddState(GameStateType _newGameStateType, GameState _newGameState)
    {
        states.Add(_newGameStateType, _newGameState);
        states[_newGameStateType].Initialize(this);
    }

    public void UpdateState()
    {
        currentState?.Update();
    }

    public void GotoState(GameStateType _key)
    {
        if (!states.ContainsKey(_key))
        {
            Debug.Log("Dictionary does not contain key " + _key);
            return;
        }
        if (currentStateType == _key)
        {
            Debug.Log("Already in state " + _key);
        }
        currentState?.Exit();
        previousState = currentState;
        currentState = states[currentStateType];
        currentStateType = _key;
        currentState.Enter();
    }

    public GameState GetState(GameStateType _type)
    {
        if (!states.ContainsKey(_type))
        {
            Debug.Log("No state found of type " + _type);
            return null;
        }
        return states[_type];
    }
}
