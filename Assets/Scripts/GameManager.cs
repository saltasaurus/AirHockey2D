using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool PlayersCanMove = false;

    #region Inspector GameObjects
    [Header("Managers")]
    [SerializeField] MenuManager Menu;
    [SerializeField] PlayerController PlayerOne;
    [SerializeField] PlayerController PlayerTwo;
    [SerializeField] GameObject puck;

    [Header("Menus")]
    [SerializeField] GameObject _startMenu;

    [Header("GUI Objects")]
    [SerializeField] TextMeshProUGUI PlayerOneScore;
    [SerializeField] TextMeshProUGUI PlayerTwoScore;
    [SerializeField] GameObject WinScreen;
    #endregion
    private TextMeshProUGUI _winText;

    private float _waitTimeAfterReset = 1.5f;
    private float _timeSinceScored = 3f;

    private bool _pointScored = true;
    private int _pointsToWinGame = 3;
    private int _winner = 0;

    private Vector2 playerOneStartPos = new(0, -10);
    private Vector2 playerOneBoundsMinMax = new(-15f, -1.25f);
    private Vector2 playerTwoStartPos = new(0, 10);
    private Vector2 playerTwoBoundsMinMax = new(1.25f, 15f);

    private State gameState;
    private enum State
    {
        Start,
        Reset,
        Playing,
        Winner
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; }
    }
    void Start()
    {
        gameState = State.Start;
        _winText = WinScreen.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update() {
        switch (gameState) {
            case State.Start:
                // MenuHandler
                Debug.Log("In Start");
                StartGame();
                break;
            case State.Reset:
                Debug.Log("In Reset");
                resetAfterPoint();
                break;
            case State.Playing:
                Debug.Log("In Playing");
                scoreHandler(Time.deltaTime);
                checkForGoal();
                _winner = checkIfWinner();
                renderScore();
                break;
            case State.Winner:
                // WinHandler
                winGame(_winner);
                break;
        }
    }
    #region State.Start
    void StartGame()
    {
        Debug.Log(Menu.ButtonState);
        switch (Menu.ButtonState)
        {
            case MenuManager.ButtonStates.Menu:
                if (_startMenu.activeSelf == false) {
                    _startMenu.SetActive(true);
                }
                return;
            case MenuManager.ButtonStates.OnePlayer:
                // Activate AI
                break;
            case MenuManager.ButtonStates.TwoPlayer:
                // Activate Player Two 
                PlayerTwo.gameObject.SetActive(true);
                break;
        }
        // Always activate PlayerOne
        PlayerOne.gameObject.SetActive(true);
        playerSetup();
        // Disable Menu once players are set up
        _startMenu.SetActive(false);
        gameState = State.Reset;
    }
    private void playerSetup()
    {
        PlayerOne.setup(1, playerOneStartPos, playerOneBoundsMinMax);
        PlayerTwo.setup(2, playerTwoStartPos, playerTwoBoundsMinMax);
    }
    #endregion
    #region State.Reset
    void resetAfterPoint() {
        PuckAndPlayerReset();
        _timeSinceScored = 0;
        gameState = State.Playing;
    }

    void PuckAndPlayerReset() {
        // Reset player and puck positions
        PlayerOne.ResetPosition();
        PlayerTwo.ResetPosition();
        puck.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        puck.transform.position = Vector2.zero;
    }

    #endregion
    #region State.Playing
    void renderScore() {
        // Update TextMeshUGUI text with player scores
        PlayerOneScore.text = PlayerOne.playerScore.ToString();
        PlayerTwoScore.text = PlayerTwo.playerScore.ToString();
    }
    int checkIfWinner() {
        // Check if either player has reached the game winning score
        if (PlayerOne.playerScore >= _pointsToWinGame) {
            gameState = State.Winner;
            return 1;
        }
        else if (PlayerTwo.playerScore >= _pointsToWinGame) {
            gameState = State.Winner;
            return 2;
        }
        return 0;
    }
    void scoreHandler(float deltatime) {
        // Wait 1 second after goal scored before players can move
        if (_pointScored) {
            _timeSinceScored += Time.deltaTime;
            if (_timeSinceScored > _waitTimeAfterReset) {
                UpdatePlayerMove(true);
                _pointScored = false;
            }
        }
    }
    void playerScored(PlayerController player) {
        // Update players score, tell manager to reset positions and prevent movement
        _pointScored = true;
        player.addPoint(1);
        UpdatePlayerMove(false);
        gameState = State.Reset;
    }

    void checkForGoal() {
        // If puck is passing top or bottom goal, add point to other player
        float puckY = puck.transform.position.y;
        if (puckY >= 16) { playerScored(PlayerOne); }
        else if (puckY <= -16) { playerScored(PlayerTwo); }
    }
    void UpdatePlayerMove(bool move) {
        // Change whether player inputs are active or inactive
        PlayersCanMove = move;
    }
    #endregion
    #region State.Winner
    void winGame(int playerNumber) {
        // TODO -> Create Win Screen script
        PuckAndPlayerReset();
        WinScreen.SetActive(true);
        _winText.text = "Player " + playerNumber + " has won!";
        // Add Menu with buttons [Menu, Replay]
        // gameState = State.Start;
    }
    #endregion
}
