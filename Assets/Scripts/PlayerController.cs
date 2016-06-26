﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    // Player related
    public float moveSpeed;
    public float jumpSpeed;

    // For checking with collision
    public Transform groundCheck;
    public float groundCheckRadius;
    // To check what the player lands on, Ground, Enemy, Water, and so on
    public LayerMask whatIsGround;
    public bool isGrounded;
    public Vector3 respawnPosition;
    public LevelManager theLevelManager;

    private Rigidbody2D myRigidbody;
    private Animator myAnim;

	// Use this for initialization
	void Start () {
        // Gets the components which this script is attached to
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        respawnPosition = transform.position;

        theLevelManager = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {

        IsGroundedMethod(groundCheck, groundCheckRadius, whatIsGround);
        MoveLeftandRight(moveSpeed);
        Jump(jumpSpeed);
        Animate(myRigidbody, isGrounded);  

    }

    // Triggers have 3 phases, Enter, In, Exit
    void OnTriggerEnter2D(Collider2D other)
    {
        // If the Player enters the KillPlane zone it will deactivate the player
        if (other.tag == "KillPlane")
        {
            //gameObject.SetActive(false);
            //transform.position = respawnPosition;

            theLevelManager.Respawn();
        }

        // If the player enters Checkpoint zone it will set the new respawn point
        if (other.tag == "Checkpoint")
        {
            // Simply set the respawnPoisition to be Checkpoints position
            respawnPosition = other.transform.position;
        }
    }

    // While we collide with MovingPlatform Players position will be equal to Platform
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = other.transform;
        }
    }

    // Simply remove players parent :'( so, it doesn't get dragged around by the parent (MovingPlatform)
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
        }
    }

    private void MoveLeftandRight(float moveSpeed)
    {
        // Right and Left movement using Axes from Input
        if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            myRigidbody.velocity = new Vector3(moveSpeed, myRigidbody.velocity.y, 0f);

            // When player moving right, it will face right
            transform.localScale= new Vector3(1f, 1f, 1f);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            myRigidbody.velocity = new Vector3(-moveSpeed, myRigidbody.velocity.y, 0f);

            // When player moving left, it will face left
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        // To prevent the player from sliding
        else
        {
            myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
        }
    }

    private void Jump(float jumpSpeed)
    {
        if(Input.GetButtonDown("Jump") && isGrounded || Input.GetKeyDown(KeyCode.W) && isGrounded || Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpSpeed, 0);
        }
    }

    private bool IsGroundedMethod(Transform groundCheck, float groundCheckRadius, LayerMask whatIsGround)
    {
        // Creates overlap circle to check whether on ground or not.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        return isGrounded;
    }

    private void Animate(Rigidbody2D myRigidbody, bool isGrounded)
    {
        // Animation, get the absolute value of x
        myAnim.SetFloat("Speed", Mathf.Abs(myRigidbody.velocity.x));
        myAnim.SetBool("Grounded", isGrounded);
    }

}
