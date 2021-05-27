using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Arduino arduino;
    public GameObject player;
    [SerializeField] private Vector2 startPosition;
    public Vector2 StartPosition { get => startPosition; }

    private GameFSM fsm;

    public GameObject beginOverlay;
    public GameObject waitText;
    public GameObject playingOverlay;
    public GameObject deadText;
    public GameObject winText;
    

    private void Awake()
    {
        fsm = new GameFSM();
        fsm.Initialize(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        fsm.GotoState(GameStateType.STATE_PLAYING);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.UpdateState();
    }

    public void GotoDeadState()
    {
        if (fsm.CurrentStateType == GameStateType.STATE_PLAYING)
        {
            fsm.GotoState(GameStateType.STATE_DEAD);
        }
    }
    public void GotoWinState()
    {
        if (fsm.CurrentStateType == GameStateType.STATE_PLAYING)
        {
            fsm.GotoState(GameStateType.STATE_WIN);
        }
    }
}
