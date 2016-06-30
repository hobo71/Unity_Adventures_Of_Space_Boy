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
    public Vector3 respawnPosition;
    public LevelManager theLevelManager;

    public GameObject stompBox;

    // Knockback variables when player gets hit
    public float knockbackForce;
    public float knockbackLength;
    private float knockbackCounter;

    public float invincibilityLength;
    private float invincibilityCounter;

    private Rigidbody2D myRigidbody;
    private Animator myAnim;

    public AudioSource jumpSound;
    public AudioSource hurtSound;
    public AudioSource checkPointSound;

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
        MoveLeftandRight(moveSpeed, jumpSpeed);
        Animate(myRigidbody, isGrounded);
        StompBoxActivator();


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
            if (enabled)
            {
                checkPointSound.Play();
                other.enabled = false;
            }
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

    public void Knockback()
    {
        hurtSound.Play();
        knockbackCounter = knockbackLength;
        invincibilityCounter = invincibilityLength;
        theLevelManager.invincible = true;
    }

    private void StompBoxActivator()
    {
        if (myRigidbody.velocity.y < 0)
        {
            stompBox.SetActive(true);
        }
        else
        {
            stompBox.SetActive(false);
        }
    }

    private void MoveLeftandRight(float moveSpeed, float jumpSpeed)
    {
        if (knockbackCounter <= 0)
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


            // Jumping
            if (Input.GetButtonDown("Jump") && isGrounded || Input.GetKeyDown(KeyCode.W) && isGrounded || Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            {
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpSpeed, 0);
                jumpSound.Play();
            }


        }

        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;

            if (transform.localScale.x > 0)
            {
                myRigidbody.velocity = new Vector3(-knockbackForce, knockbackForce, 0f);

            }
            else
            {
                myRigidbody.velocity = new Vector3(knockbackForce, knockbackForce, 0f);
            }
        }
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
        }
        if (invincibilityCounter <= 0)
        {

            theLevelManager.invincible = false;
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
