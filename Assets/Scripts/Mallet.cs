using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mallet : MonoBehaviour
{
    public float movespeed = 15f;

    private Vector2 direction;
    private float minBoundary;
    private float maxBoundary;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void setInput(Vector2 dir)
    {
        direction = dir;
    }

    private void FixedUpdate()
    {
        rb.velocity = 5000 * movespeed * direction * Time.deltaTime;
        // rb.MovePosition(rb.position + movespeed * direction * Time.deltaTime);
        Debug.Log(rb.velocity);
        Vector2 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minBoundary, maxBoundary);
        transform.position = pos;
    }

    public void moveMallet(Vector2 dir)
    {
        transform.Translate(dir * movespeed);
    }

    public void setBounds(float min, float max)
    {
        minBoundary = min;
        maxBoundary = max;
    }

    public void setSpeed(float speed)
    {
        movespeed = speed;
    }

    public void setPosition(Vector2 position)
    {
        transform.position = position; 
    }




}
