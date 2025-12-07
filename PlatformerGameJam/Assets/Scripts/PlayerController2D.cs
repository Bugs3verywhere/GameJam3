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

    float lastGrounded = 0f;

    //Movement speed values
    public float baseSpeed = 3.5f;
    public float sprintSpeed = 5f;
    float walkSpeed = 3.5f;
    public float jumpSpeed = 10f;

    public float cameraDistance = 20f;

    void Update()
    {
        //Camera follows player
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, new Vector3(player.position.x, player.position.y + 1, -cameraDistance), 0.03f);
        
        //Player movement
        float facingDirection = 0f;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            walkSpeed = sprintSpeed;
            animator.speed = 1.4f;
        }
        else
        {
            walkSpeed = baseSpeed;
            animator.speed = 1;
        }

        //Player facing direction
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

        //Move player
        player.velocity = new Vector3(walkSpeed * facingDirection, player.velocity.y, 0);

        //Movement animation
        if (facingDirection != 0)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && (touchingGround||Time.time - lastGrounded <= 0.125f))//Coyote time
        {
            player.velocity = new Vector3(player.velocity.x, jumpSpeed, 0);
            touchingGround = false;
            lastGrounded = 0;
        }

        //Landing
        if (player.Cast(new Vector2(0, -1), new RaycastHit2D[1], 0.1f) > 0)
        {
            touchingGround = true;
            lastGrounded = Time.time;
        }
        //Falling
        else
        {
            touchingGround = false;
        }

        //Animation for grounded/falling/rising
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
}
