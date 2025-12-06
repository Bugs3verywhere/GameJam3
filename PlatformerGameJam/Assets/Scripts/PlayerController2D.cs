using UnityEngine;

public class PlayerController2D : MonoBehaviour
{

    //Declaring basic variables
    public Rigidbody2D player;
    public SpriteRenderer playerSprite;
    public Animator animator;
    public bool touchingGround = false;
    public float facingDirection;
    public Camera mainCam;

    //Movement speed values
    public float walkSpeed = 2f;
    public float sprintSpeed = 4f; //Not implemented yet
    public float jumpSpeed = 5f;
    public float cameraDistance = 10f;

    void Update()
    {
        //Camera follows player
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, new Vector3(player.position.x, player.position.y + 1, -cameraDistance), 0.03f);
        
        //Player movement
        float facingDirection = 0f;

        if (Input.GetKey(KeyCode.A) & !Input.GetKey(KeyCode.D))
        {
            facingDirection = -1;
            playerSprite.flipX = true;
        }

        else if (Input.GetKey(KeyCode.D) & !Input.GetKey(KeyCode.A))
        {
            facingDirection = 1;
            playerSprite.flipX = false;
        }

        player.velocity = new Vector3(walkSpeed * facingDirection, player.velocity.y, 0);

        if (facingDirection != 0)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && touchingGround)
        {
            player.velocity = new Vector3(player.velocity.x, jumpSpeed, 0);
            touchingGround = false;
        }

        if (touchingGround)
        {
            animator.SetBool("Grounded", true);
        }
        else
        {
            animator.SetBool("Grounded", false);
            animator.SetBool("Rising", (player.velocity.y > 0));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Check floor collision
        if (collision.collider.CompareTag("Floor"))
        {
            touchingGround = true;
        }
    }
}
