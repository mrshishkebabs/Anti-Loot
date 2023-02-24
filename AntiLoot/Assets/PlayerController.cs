using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //variables to be adjusted in Unity editor need to be public
    //anything else can be private, as to not crowd the Unity editor
    //movement
    public float speed;
    bool movingLeft = false;
    bool movingRight = false;
    
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

    //dash
    private bool dashing = false;
    private bool dashUsed = true;
    public bool dashActive = false;
    public float dashForce = 4;
    

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
            if not grounded, but double jump has not been used(false), apply another upward force and mark double jump as used(true).
            if not grounded and double jump has been used, dont jump
        */

        grounded = GroundCheck();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if(grounded)
            {
                doubleJumpUsed = false;
                dashUsed = false; //player can only dash while in the air, so dash will also reset when player is on the ground
                jumping = true;
            }

            else if (doubleJumpActive && !doubleJumpUsed)
            {
                doubleJumpUsed = true;
                jumping = true;
            }
        }

        /*
         CHECKING MOVEMENT DIRECTION 
         if xVel is positive(less than 0), player is moving right
         if negative, moving left
         checking for phasing and animatons
         (might swtich direction check to check for pressing a or d later, for more accuracy.)
        */
        if(xVel < 0)//moving left
        {
            movingLeft = true;
            movingRight = false;
        }
        else if (xVel > 0)//moving right
        {
            movingLeft = false;
            movingRight = true;
        }

        /*
        DASHING
        kinda similar to jumping: 
        -bool in update(). 
        -on button press, bool true
        -on bool true: do physics movement in fixedupdate(), set bool to false
        dun
        
        dash will be set to LEFT CLICK for now

        okay there's a lot of bools so I'll go thru that to for clarity:
        -dashActive enables or disables the ability to dash, depending on if the player chooses dashing as a skill
        -dashing is for the dash itself. when true, do the physics thing
        -dashUsed limits the dash to once per jump. when true, player can't dash again. reset to false when the player jumps again
        
         MIGHT CHANGE IMPLEMENTATION TO MAKE THE DASH MORE LINEAR
         */
        
        if(dashActive && Input.GetMouseButtonDown(0) && !dashUsed)
        {
            dashing = true;
            dashUsed = true;
        }


    }

    private void FixedUpdate()
    {
        if (jumping)
        {
            rb.velocity = Vector3.up * jumpForce;
            jumping = false;
        }

        if(dashing)
        {
            if(movingLeft)
                rb.velocity = Vector3.left * dashForce;

            if (movingRight)
                rb.velocity = Vector3.right * dashForce;
            dashing = false;
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
