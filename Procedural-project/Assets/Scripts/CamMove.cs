using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{

    public float moveSpeed;

    //animations
    //private Animator anim;

    //rigidbody
    private Rigidbody2D RigidPlayer;

    //movement check
    private bool Moving;
    public Vector2 lastMove;

    private static bool playerExists;

    public bool canMove;


    // Use this for initialization
    void Start()
    {
        //anim = GetComponent<Animator> ();
        RigidPlayer = GetComponent<Rigidbody2D>();
        canMove = true;
        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {

        Moving = false;

        if (!canMove)
        {
            RigidPlayer.velocity = Vector2.zero;
            return;
        }

        //movements

        //left right
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            //transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal")* moveSpeed * Time.deltaTime , 0, 0));
            RigidPlayer.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, RigidPlayer.velocity.y);
            Moving = true;
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }

        //up down
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            //transform.Translate(new Vector3(0, Input.GetAxisRaw("Vertical")* moveSpeed * Time.deltaTime , 0));
            RigidPlayer.velocity = new Vector2(RigidPlayer.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
            Moving = true;
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }

        //stp moving
        if (Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -0.5f)
        {
            RigidPlayer.velocity = new Vector2(0f, RigidPlayer.velocity.y);
        }
        if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f)
        {
            RigidPlayer.velocity = new Vector2(RigidPlayer.velocity.x, 0f);
        }
    }

}