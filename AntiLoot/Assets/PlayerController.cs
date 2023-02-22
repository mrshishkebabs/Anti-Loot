using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //variables to be adjusted in Unity editor need to be public
    //anything else can be private, as to not crowd the Unity editor
    //movement
    public float speed;
    
    //jumping
    public Rigidbody2D rb;
    public float jumpForce = 10;
    public LayerMask ground;
    public float groundCheckDistance;
    public Vector3 groundCheckOffset;
    private bool grounded;
    private bool jumping = false;
    public bool doubleJumpActive = false;
    private bool doubleJumpUsed = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        SIMPLE MOVEMENT
        Horizontal is an input built into Unity. It starts at 0.(access Horizontal with Input.GetAxisRaw())
        When the 'a' key is pressed, it's -1
        When the 'd' key is pressed, it's 1
        When nothing is pressed, it's 0
        We can plug a variable set to Horizontal into the movement calculation
        to automate left/right movement.

        Use transfrom.Translate to move the player. It takes in a vector3 (x, y, z) for where the player is being moved.
        Here, we're only moving on the x axis, so the speed calculation will only be in the x position, and the y and z
        stay unchanged at 0.
        
        For the speed calculation, xVel determines left or right movement, speed is the speed, and 
        Time.deltaTime ensures that movement in time-based and not frame rate based(so that a pc
        with a lower frame rate can move the same way).
         */


        float xVel = Input.GetAxisRaw("Horizontal");
        transform.Translate(xVel * speed * Time.deltaTime, 0, 0);

        /*
        JUMPING(this requires different components in different places, but I'll do the whole explanation here)
        Here we're using a raycast to check if the player is on the ground. When we use the raycast, it'll shoot a ray
        downwards(we set it to go down) a certain distance(groundCheckDistance). When it shoots, it'll look specifically for the ground,
        using the "ground" layer(I don't remember why we use layer over tag tho). If the ray hits the ground within the distance, player is 
        grounded. If false, player is in the air.

        The rays will shoot down from the player sprite. I also have groundCheckOffset, which will be added and substracted from the 
        center point of the player sprite. I'm doing this so the player hand be hanging off the edge of a platform and still jump,
        allowing for more precise jumping.

        The rest of the process is kinda simple: 
            if player is grounded, apply an upward force to the player to jump
            if not grounded, dont jump.
        */

        grounded = GroundCheck();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if(grounded)
            {
                doubleJumpUsed = false;
                jumping = true;
            }

            else if (doubleJumpActive && !doubleJumpUsed)
            {
                doubleJumpUsed = true;
                jumping = true;
            }
        }

    }

    private void FixedUpdate()
    {
        if (jumping)
        {
            rb.velocity = Vector3.up * jumpForce;
            jumping = false;
        }
    }

    private bool GroundCheck()
    {
        //params for raycast: start pos, direction, distance, target(ground layer)
        bool check = (
            Physics2D.Raycast(transform.position + groundCheckOffset, Vector3.down, groundCheckDistance, ground) ||
            Physics2D.Raycast(transform.position - groundCheckOffset, Vector3.down, groundCheckDistance, ground));

        return check;
    }

    
}
