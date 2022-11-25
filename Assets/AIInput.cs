using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : MonoBehaviour
{
    private Vector2 _puckPos;
    private Vector2 _lastPuckPos;
    private int _baseFramesBeforeNewInput = 30;
    private int _framesBeforeNewInput;
    private int _framesSinceInput;
    private int _delay = 0;
    private Vector2 _direction;
    private int difficulty;

    private float _consecutiveFramesTooClose = 0;
    // Start is called before the first frame update
    void Start()
    {
        _framesSinceInput = _framesBeforeNewInput - _delay;
    }

    public void setDifficulty(int diff)
    {
        difficulty = Mathf.Clamp(diff, 1, 3);
        _framesBeforeNewInput = (int)(_baseFramesBeforeNewInput / difficulty);

    }

    public Vector2 GetAIInput()
    {
        // Only get new input after a certain number of frames;
        _framesSinceInput += 1;
        if (_framesSinceInput < (_framesBeforeNewInput / difficulty)) { return _direction; }
        _framesSinceInput = 0;
        

        // Get puck's current position
        _puckPos = GameManager.Instance.GetPuckPosition();

        // Get direction towards puck
        _direction = (_puckPos - (Vector2)transform.position);

        // If puck is too close, increment counter, otherwise reset counter
        if (_direction.sqrMagnitude < 7.5f) { _consecutiveFramesTooClose += 1; }
        else { _consecutiveFramesTooClose = 0; }

        // If too close for too many inputs, move backwards
        if (_consecutiveFramesTooClose > 5) {
            _direction *= -1;
        }
        // Convert movement towards puck to cardinal directions like the player (for easy ai mode)
        _direction.y = Mathf.Round(_direction.y);
        _direction.x = Mathf.Round(_direction.x);

        // _lastPuckPos = _puckPos;

        return _direction;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
