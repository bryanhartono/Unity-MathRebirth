using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    float startingX, startingY;
    public float speed = 1f;
    public float leftRange = 4f;
    public float rightRange = 0f;
    public float upRange = 4f;
    public float bottomRange = 0f;
    int dir = -1;

    public bool horizontal = true;

    Vector2 move;

    void Start()
    {
        startingX = gameObject.transform.position.x;
        startingY = gameObject.transform.position.y;
        move = new Vector2(0, 0);
    }
    void FixedUpdate()
    {
        // print(move);
        if(horizontal)
        {
            move = Vector2.right * speed * Time.deltaTime * dir;
            transform.Translate(move);
            if (transform.position.x < startingX - leftRange)
            {
                dir = 1;
            }
            else if (transform.position.x > startingX + rightRange)
            {
                dir = -1;
            }
        }

        if(!horizontal)
        {
            move = Vector2.up * speed * Time.deltaTime * dir;
            transform.Translate(move);
            if (transform.position.y < startingY - bottomRange)
            {
                dir = 1;
            }
            else if (transform.position.y > startingY + upRange)
            {
                dir = -1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}

