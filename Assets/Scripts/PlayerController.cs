using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region default values
    private int player;
    private Vector2 _startPos;
    private float _speed;
    private float _minBoundary;
    private float _maxBoundary;
    #endregion

    public int playerScore;

    private Rigidbody2D rb;
    private AIInput ai;
    private bool _isAI;
    private int difficulty;
    private Vector2 _direction;
    private Vector3 _lastPosition = Vector3.zero;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ai = GetComponent<AIInput>();
        Debug.Log(ai);
    }

    #region Setup
    public void setup(int number, Vector2 pos, Vector2 bounds, float speed = 750, int score = 0)
    {
        player = number;
        _startPos = pos;
        _speed = speed;
        _minBoundary = bounds[0];
        _maxBoundary = bounds[1];
        playerScore = score;
    }
    public void ResetPosition()
    {
        _direction = Vector2.zero;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.Sleep();
        setPosition(_startPos);
    }

    public void isAI(bool ai, int aiDifficulty = 1)
    {
        _isAI = ai;
        difficulty = aiDifficulty;
        if (ai)
        {
            Debug.Log("AI set to " + difficulty + " difficulty");
        }
    }
    #endregion
    #region Update
    private void Update()
    {
        if (GameManager.Instance.PlayersCanMove)
        {
            // If AI, use the AI input system, otherwise player input
            if (_isAI) { _direction = ai.GetAIInput(difficulty); }
            else { _direction = getInput(); }

            // Cache transform
            Vector2 pos = transform.position;

            // Prevent player from moving across boundary
            transform.position = new Vector2(pos.x, Mathf.Clamp(pos.y, _minBoundary, _maxBoundary));


           
        }

    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.PlayersCanMove)
        {
            rb.velocity = _speed * Time.deltaTime * _direction.normalized;
        }
    }
    #endregion

    #region Setters

    void setSpeed(float speed)
    {
        _speed = speed;
    }
    void setPosition(Vector2 position)
    {
        transform.position = position;
    }
    void setBounds(float minBound, float maxBound)
    {
        _minBoundary = minBound;
        _maxBoundary = maxBound;
    }
    #endregion

    #region Score
    public void addPoint(int points)
    {
        setScore(playerScore + points);
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
