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
    [SerializeField] GameObject _WinScreen;

    [Header("GUI Objects")]
    [SerializeField] TextMeshProUGUI PlayerOneScore;
    [SerializeField] TextMeshProUGUI PlayerTwoScore;
    #endregion
    [SerializeField] private TextMeshProUGUI _winText;

    private float _waitTimeAfterReset = 1.5f;
    private float _timeSinceScored = 3f;

    private bool textNotWritten = true;
    private bool _pointScored = true;
    private int _pointsToWinGame = 3;
    private int _winner = 0;
    // private int gameDifficulty;

    private Vector2 playerOneStartPos = new(0, -10);
    private Vector2 playerOneBoundsMinMax = new(-15f, -1.25f);
    private Vector2 playerTwoStartPos = new(0, 10);
    private Vector2 playerTwoBoundsMinMax = new(1.25f, 15f);

    private int _maxPlayers = 2;
    private int _numPlayers = 2;
    private PlayerController[] _players;
    private Vector2[] _playerStartPositions;
    private Vector2[] _playerBounds;

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
        // _winText = _WinScreen.GetComponentInChildren<TextMeshProUGUI>();
        _players = new PlayerController[_maxPlayers];
        _playerStartPositions = new Vector2[_maxPlayers];
        _playerBounds = new Vector2[_maxPlayers];
        // TODO -> Replace with prefabs?
        _players[0] = PlayerOne;
        _players[1] = PlayerTwo;
        // TODO -> Find better way to instantiate
        _playerStartPositions[0] = playerOneStartPos;
        _playerStartPositions[1] = playerTwoStartPos;
        _playerBounds[0] = playerOneBoundsMinMax;
        _playerBounds[1] = playerTwoBoundsMinMax;
        // Change state to start so menu and player selection begins
        gameState = State.Start;
    }

    private void Update() {
        switch (gameState) {
            case State.Start:
                // MenuManager Logic 
                StartGame();
                break;
            case State.Reset:
                resetAfterPoint();
                break;
            case State.Playing:
                scoreHandler(Time.deltaTime);
                checkForGoal();
                _winner = checkIfWinner();
                renderScore();
                break;
            case State.Winner:
                winGame(_winner);
                break;
        }
    }
    public Vector2 GetPuckPosition() {
        // Returns the puck's position
        return puck.transform.position;
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
                _players[0].isAI(false);
                _players[1].isAI(true, Menu.difficulty);
                break;
            case MenuManager.ButtonStates.TwoPlayer:
                // Activate Player Two 
                _players[0].isAI(false);
                _players[1].isAI(false);
                break;
        }


        // Always activate PlayerOne
        playerSetup();
        // Disable Menus once players are set up
        Menu.ButtonPressed = false;
        _startMenu.SetActive(false);
        gameState = State.Reset;
    }
    private void playerSetup()
    {
        for (int i=0; i < _numPlayers; i++)
        {
            PlayerController currentPlayer = _players[i];
            GameObject currentPlayerGO = currentPlayer.gameObject;
            if (!currentPlayerGO.activeSelf)
            {
                currentPlayerGO.SetActive(true);
            }
            currentPlayer.setup(i + 1, _playerStartPositions[i], _playerBounds[i]);
        }
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
            Debug.Log("Player One has won!");
            return 1;
        }
        else if (PlayerTwo.playerScore >= _pointsToWinGame) {
            gameState = State.Winner;
            Debug.Log("Player Two has won!");
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
        _WinScreen.SetActive(true);
        if (textNotWritten)
        {
            _winText.text = "Player " + playerNumber + " has won!";
            textNotWritten = false;
        }
        if (Menu.ButtonPressed)
        {
            _WinScreen.SetActive(false);
            textNotWritten = true;
            gameState = State.Start;
        }
        // Add Menu with buttons [Menu, Replay]
        // gameState = State.Start;
    }
    #endregion
}
