using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public TMP_Text countdownText;
    public GameObject playingOverlay;
    public GameObject deadText;
    public GameObject winText;
    

    private void Awake()
    {
        fsm = new GameFSM();
        fsm.Initialize(this);
        countdownText = waitText.GetComponent<TMP_Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        fsm.GotoState(GameStateType.STATE_BEGIN);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.UpdateState();
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
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
