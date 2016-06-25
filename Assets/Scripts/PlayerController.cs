using UnityEngine;
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

    private Rigidbody2D myRigidbody;

	// Use this for initialization
	void Start () {

        // Gets the component which this script is attached to
        myRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        isGroundedMethod(groundCheck, groundCheckRadius, whatIsGround);
        moveLeftandRight(moveSpeed);
        jump(jumpSpeed);

    }

    private void moveLeftandRight(float moveSpeed)
    {
        // Right and Left movement using Axes from Input
        if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            myRigidbody.velocity = new Vector3(moveSpeed, myRigidbody.velocity.y, 0f);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            myRigidbody.velocity = new Vector3(-moveSpeed, myRigidbody.velocity.y, 0f);
        }

        // To prevent the player from sliding
        else
        {
            myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
        }
    }

    private void jump(float jumpSpeed)
    {
        if(Input.GetButtonDown("Jump") && isGrounded || Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpSpeed, 0);
        }
    }

    private bool isGroundedMethod(Transform groundCheck, float groundCheckRadius, LayerMask whatIsGround)
    {
        // Creates overlap circle to check whether on ground or not.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        return isGrounded;
    }

}
