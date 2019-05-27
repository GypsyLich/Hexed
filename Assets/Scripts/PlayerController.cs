using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    Animator anim;
    private float horizontalMove;
    private float verticalMove;
    private float vectorOfMovement;
    private Vector3 destination, nextPosition, facingDirection;
    float speed = 3f;
    private bool isMoving;
    public Tilemap blockingLayer;

    void Start()
    {
        anim = GetComponent<Animator>();
        destination = transform.position;
        facingDirection = Vector2.right;
    }

    void Update()
    {
        TakeAction();
    }

    void TakeAction()
    {
        if (Vector2.Distance(destination, transform.position) <= 0.01f && isMoving)
        {
            transform.position = destination;
            isMoving = false;
        }
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * speed);
        }
        else
        {
            CheckForNewInput();
        }
    }

    void CheckForNewInput()
    {

        if (Input.GetButtonDown("BM"))
        {
            switch (anim.GetBool("Battle_mode"))
            {
                case true:
                    anim.SetBool("Battle_mode", false);
                    break;
                case false:
                    anim.SetBool("Battle_mode", true);
                    break;
            }
        }

        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = 3 * Input.GetAxisRaw("Vertical");
        vectorOfMovement = verticalMove + horizontalMove;
        if (vectorOfMovement != 0)
        {
            MakeAMove(verticalMove + horizontalMove);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    bool CanMove(Vector2 nextPosition)
    {
        return !IsColliding(nextPosition) && !IsBlockingAnimationPlaying();
    }

    bool IsBlockingAnimationPlaying()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Player_BD_sword_draw")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_sword_holster")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_BM_attack");
    }

    bool IsColliding(Vector2 nextPosition)
    {
        return blockingLayer.GetTile(new Vector3Int((int)(nextPosition.x - 0.5f), (int)nextPosition.y, 0)) != null ? true : false;
    }

    void MakeAMove(float vectorOfMovement)
    {
        switch (vectorOfMovement)
        {
            case -1:
                facingDirection = new Vector2(0, 180);
                nextPosition = transform.position + new Vector3(-1, 0);
                break;
            case 1:
                facingDirection = new Vector2(0, 0);
                nextPosition = transform.position + new Vector3(1, 0);
                break;
            case -3:
                nextPosition = transform.position + new Vector3(0, -1);
                break;
            case 3:
                nextPosition = transform.position + new Vector3(0, 1);
                break;
            default:
                anim.SetBool("isRunning", false);
                return;
        }
        if (CanMove(nextPosition))
        {
            destination = nextPosition;
            transform.eulerAngles = facingDirection;
            isMoving = true;
            anim.SetBool("isRunning", true);
        }
    }
}