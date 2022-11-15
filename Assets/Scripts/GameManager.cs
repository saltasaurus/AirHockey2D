using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerController PlayerOne;
    public PlayerController PlayerTwo;
    public GameObject puck;

    private int PointsToWinGame = 7;

    private Vector2 playerOneStartPos = new(0, -10);
    private Vector2 playerOneBoundsMinMax = new(-15, -1);

    private Vector2 playerTwoStartPos = new(0, 10);
    private Vector2 playerTwoBoundsMinMax = new(1, 15);

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerSetup();
        resetAfterPoint();
    }

    void resetAfterPoint()
    {
        playersCanMove(false);
        PlayerOne.setupAfterPoint();
        PlayerTwo.setupAfterPoint();
        puck.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        puck.transform.position = Vector2.zero;
        playersCanMove(true);
    }

    private void playersCanMove(bool move)
    {
        PlayerOne.allowMovement(move);
        PlayerTwo.allowMovement(move);
    }

    private void playerSetup()
    {
        PlayerOne.setup(1, playerOneStartPos, playerOneBoundsMinMax);
        PlayerTwo.setup(2, playerTwoStartPos, playerTwoBoundsMinMax);
    }

    private void Update()
    {
        float puckY = puck.transform.position.y;
        if (puckY > 16)
        {
            PlayerOne.addPoint();
            resetAfterPoint();
        }
        else if (puckY < -16)
        {
            PlayerTwo.addPoint();
            resetAfterPoint();
        }

        if (PlayerOne.playerScore > PointsToWinGame)
        {
            WinGame(1);
        }
        if (PlayerTwo.playerScore > PointsToWinGame)
        {
            WinGame(2);
        }


    }

    void WinGame(int playerNumber)
    {
        playersCanMove(false);
        // Replace with Text.TextMeshPro
        Debug.Log("Player " + playerNumber + " has won!");
    }
}
