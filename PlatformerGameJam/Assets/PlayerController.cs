using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Declaring basic variables
    public Rigidbody player;
    public bool touchingGround = false;
    public float facingDirection;
    public Camera mainCam;

    //Movement speed values
    public float walkSpeed = 2f;
    public float sprintSpeed = 4f; //Not implemented yet
    public float jumpSpeed = 5f;

    void Update()
    {
        //Camera follows player
        mainCam.transform.position = new Vector3(player.position.x, player.position.y + 1, -8);

        //Player movement
        float facingDirection = 0f;

        if (Input.GetKey(KeyCode.A))
            facingDirection = -1;

        else if (Input.GetKey(KeyCode.D))
            facingDirection = 1;

        player.velocity = new Vector3(walkSpeed * facingDirection, player.velocity.y, 0);

        if (Input.GetKeyDown(KeyCode.Space) && touchingGround)
        {
            player.velocity = new Vector3(player.velocity.x, jumpSpeed, 0);
            touchingGround = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Check floor collision
        if (collision.collider.CompareTag("Floor"))
        {
            touchingGround = true;
        }
    }
}
