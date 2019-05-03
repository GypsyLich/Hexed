using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Animator anim;
    float horizontalMove;
    float verticalMove;
    Vector3 destination, nextPosition, facingDirection;
    float speed = 2f;
    bool canMove, isMoving;
    void Start()
    {
        anim = GetComponent<Animator>();
        destination = transform.position;
        facingDirection = Vector2.right;
    }

    void Update()
    {
        Move();

    }
    void CheckInput()
    {
        if (!isMoving)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal");
            verticalMove = 3 * Input.GetAxisRaw("Vertical");

            switch (verticalMove + horizontalMove)
            {
                case -1:
                    facingDirection = new Vector2(0, 180);
                    nextPosition = transform.position + new Vector3(-1, 0);
                    canMove = true;
                    break;
                case 1:
                    facingDirection = new Vector2(0, 0);
                    nextPosition = transform.position + new Vector3(1, 0);
                    canMove = true;
                    break;
                case -3:
                    nextPosition = transform.position + new Vector3(0, -1);
                    canMove = true;
                    break;
                case 3:
                    nextPosition = transform.position + new Vector3(0, 1);
                    canMove = true;
                    break;
                    /*case 2:
                        facingDirection = new Vector2(0, 180);
                        nextPosition = transform.position + new Vector3(-1, 1);
                        canMove = true;
                        break;
                    case 4:
                        facingDirection = new Vector2(0, 0);
                        nextPosition = transform.position + new Vector3(1, 1);
                        canMove = true;
                        break;
                    case -4:
                        facingDirection = new Vector2(0, 180);
                        nextPosition = transform.position + new Vector3(-1, -1);
                        canMove = true;
                        break;
                    case -2:
                        facingDirection = new Vector2(0, 0);
                        nextPosition = transform.position + new Vector3(1, -1);
                        canMove = true;
                        break;
                    default:
                        break;*/
            }
        }
    }
    void Move()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * speed);
            Debug.Log(Vector2.Distance(destination, transform.position));
        }
        if (Vector2.Distance(destination, transform.position) <= 0.01f)
        {
            transform.position = destination;
            isMoving = false;
            CheckInput();
            if (canMove)
            {
                destination = nextPosition;
                transform.eulerAngles = facingDirection;
                canMove = false;
                isMoving = true;
                anim.SetBool("isRunning", true);
            }
            else { anim.SetBool("isRunning", false); }
        }
    }
}