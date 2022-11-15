using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region default values
    private int player;
    private Vector2 startPos;
    private float startSpeed;
    private float minStartBoundary;
    private float maxStartBoundary;
    #endregion

    public int playerScore = 0;
    public float playerSpeed;
    public float minBoundary;
    public float maxBoundary;

    private bool canMove = false;
    private Rigidbody2D rb;
    private Vector2 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    #region Setup
    public void setup(int number, Vector2 pos, Vector2 bounds, float speed = 750, int score = 0)
    {
        player = number;
        startPos = pos;
        startSpeed = speed;
        minStartBoundary = bounds[0];
        maxStartBoundary = bounds[1];
        playerScore = score;
    }
    public void setupAfterPoint()
    {
        setPosition(startPos);
        setScore(playerScore);
        setSpeed(startSpeed);
        setBounds(minStartBoundary, maxStartBoundary);
    }
    #endregion
    #region Update
    private void Update()
    {
        if (!canMove)
        {
            return;
        }
        direction = getInput();

    }

    private void FixedUpdate()
    {
        rb.velocity = playerSpeed * Time.deltaTime * direction.normalized;
    }
    #endregion
    public void allowMovement(bool canPlayerMove)
    {
        canMove = canPlayerMove;
    }
    #region Setters

    void setSpeed(float speed)
    {
        playerSpeed = speed;
    }
    void setPosition(Vector2 position)
    {
        transform.position = position;
    }
    void setBounds(float minBound, float maxBound)
    {
        minBoundary = minBound;
        maxBoundary = maxBound;
    }
    #endregion

    #region Score
    public void addPoint()
    {
        setScore(playerScore + 1);
    }

    public void setScore(int score)
    {
        playerScore = score;
    }
    #endregion

    #region Input
    public Vector2 getInput()
    {
        if (player == 1)
        {
            return playerOneInput();
        }
        else if (player == 2)
        {
            return playerTwoInput();
        }
        return Vector2.zero;

    }

    private Vector2 playerOneInput()
    {
        Vector2 moveDir = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.A))
        {
            moveDir += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir += Vector2.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            moveDir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir += Vector2.down;
        }
        return moveDir;
    }

    private Vector2 playerTwoInput()
    {
        Vector2 moveDir = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir += Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDir += Vector2.right;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDir += Vector2.down;
        }
        return moveDir;
    }
    #endregion
}
