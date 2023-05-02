using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerStates { Idle, Walk, JumpUp, JUptoDown, JumpDown, WallSlide, WallCling };
public class PlayerController : MonoBehaviour
{
    public PlayerStates playerState = PlayerStates.Idle;

    //variables to be adjusted in Unity editor need to be public
    //anything else can be private, as to not crowd the Unity editor
    //movement
    public float speed;
    float xVel;

    //jumping
    public Rigidbody2D rb;
    public int jumpForce;
    public LayerMask ground;
    private float groundCheckDistance = 0.6f;
    private Vector3 groundCheckOffset = new Vector3(0.5f, 0, 0);
    private bool grounded;
    private bool jumping = false;
    private bool doubleJumping = false;

    //dash
    private bool dashing = false;
    private bool dashUsed = false;
    private float dashTime = 0.2f;
    private float dashForce = 4;
    public TrailRenderer trail;

    //wall jump
    public LayerMask wall;
    private float wallCheckDistance = 0.3f;
    private Vector3 wallCheckOffset;

    public Transform wallCheck;
    [SerializeField] private bool onWall;
    private float wallSlideSpeed = 2f;
    public Vector2 wallJumpForce;
    private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private float wallJumpDuration = 0.4f;
    [SerializeField] private bool wallJumping;

    private bool wallSliding = false;

    public int hitsTillDead = 3;
    private float hitCooldown = 0;
    private float hitCooldownReset = 200f;
    public GameObject hitCooldownIndicator;
    public GameObject lastLifeText;

    //win condition
    public bool reachedGoal = false;
    public bool canMove = true;        //only false when player is ded

    //debugging
    //public Vector3 testLineLength;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //use this to visualize raycasts for debugging
        //Debug.DrawLine(transform.position, transform.position + testLineLength);
        //Debug.Log(rb.velocity);

        /*
        FOR THE DASH:
        we dont want any other movements to interrupt the dash while it's happening, 
        so if the player is dashing, skip the rest of the update function so the player
        can't do anything else
        */
        if (dashing)
            return;


        xVel = Input.GetAxisRaw(PlayerInput.HORIZONTAL);
        //transform.Translate(xVel * speed * Time.deltaTime, 0, 0);


        grounded = GroundCheck();
        onWall = onWallCheck();
        wallSliding = WallSlide();


        //reset double jump when on the ground
        if (grounded && !Input.GetKey(KeyCode.Space))
        {
            doubleJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((grounded || (doubleJumping && !onWall)))
            {
                jumping = true;
                doubleJumping = !doubleJumping;
                //for doubleJumpUsed, set it to it's opposite. on first jump it'll switch to true, allowing the double jump
                //on the second jump, it'll switch to false to lock it out again
                //it'll stay false until the player hits the ground and it resets
                dashUsed = false;
                Debug.Log("jump");
            }

        }

        /*
         CHECKING MOVEMENT DIRECTION 
         if xVel is positive(less than 0), player is moving right
         if negative, moving left
         checking for phasing and animatons
         (might swtich direction check to check for pressing a or d later, for more accuracy.)
         we dont want to flip the sprite while the player is wall jumping, so check only happens
         if the player is not wall jumping
        */

        if (xVel < 0)//moving left
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (xVel > 0)//moving right
        {
            transform.localScale = new Vector3(1, 1, 1);
        }



        if (Input.GetKeyDown(KeyCode.W) && !dashUsed && !grounded)
        {
            //call the coroutine
            StartCoroutine(Dash());
        }


        //Animation State
        if (xVel != 0)
        {
            playerState = PlayerStates.Walk;
        }
        else if (rb.velocity.y > 0)
        {
            playerState = PlayerStates.JumpUp;
        }
        else if (rb.velocity.y < 0)
        {
            playerState = PlayerStates.JumpDown;
            if (onWall)
                playerState = PlayerStates.WallSlide;
        }
        else
            playerState = PlayerStates.Idle;

        HitCooldown();
        /////////////////////WALL JUMP CALL//////////////////////////////////////
        WallJump();

        if(!grounded)
            Debug.Log(rb.velocity);
    }



    private void FixedUpdate()
    {
        /*
        FOR THE DASH:
        we dont want any other movements to interrupt the dash while it's happening, 
        so if the player is dashing, skip the rest of the update function so the player
        can't do anything else
        */
        if (dashing)
            return;

        /////////////////////MOVEMENT//////////////////////////////////////
        /*
        PHYSICS MOVEMENT 
        here, we're adding velocity to the player's rigidbody. to move left or right, we take the xVel
        (which is -1 or 1 based on player input) and multiply it by our speed value.
        we keep y as is to maintain upward/downward force
        */

        if (!wallJumping && canMove)
            rb.velocity = new Vector2(xVel * speed, rb.velocity.y);


        /*
        there wasa bug that allowed the player to stick to the wall in the air by moving into the wall.
        this fixes that by setting the velocity to 0 if the player is on a wall and in the air
        */

        if (!grounded && canMove && onWall && !wallJumping)
        {
            if (rb.velocity.x < 0)
                rb.velocity = new Vector2(0, rb.velocity.y);

            else if (rb.velocity.x > 0)
                rb.velocity = new Vector2(0, rb.velocity.y);

        }

        /*
        JUMPING FLOW SCHEME
        jump
        -after jump, can double jump, dash, or wall jump
        -after double jump, can dash or wall jump
        -after dash, can wall jump
        -after wall jump, can double jump or dash
        -essentially, wall jumping refreshes double jump and dash
        */
        


        /////////////////////JUMPING//////////////////////////////////////
        if (jumping && canMove)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
            jumping = false;
            //Debug.Log(rb.velocity);
        }



        /////////////////////WALL SLIDE MOVEMENT//////////////////////////////////////
        //WallSlide();

        if (wallSliding && canMove)
        {
            if (rb.velocity.y < -wallSlideSpeed)
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            //Debug.Log("sliiiiide");
        }


    }
    /////////////////////GROUND/WALL CHECK//////////////////////////////////////
    private bool GroundCheck()
    {
        //params for raycast: start pos, direction, distance, target(ground layer)
        bool check = (
            Physics2D.Raycast(transform.position + groundCheckOffset, Vector2.down, groundCheckDistance, ground) ||
            Physics2D.Raycast(transform.position - groundCheckOffset, Vector2.down, groundCheckDistance, ground));

        return check;
    }

    private bool WallCheckLeft()
    {
        //params for raycast: start pos, direction, distance, target(wall layer)
        bool left = (
            Physics2D.Raycast(transform.position + wallCheckOffset, Vector2.left, wallCheckDistance, wall) ||
            Physics2D.Raycast(transform.position - wallCheckOffset, Vector2.left, wallCheckDistance, wall));

        return left;
    }


    private bool WallCheckRight()
    {
        //params for raycast: start pos, direction, distance, target(wall layer)
        bool right = (
            Physics2D.Raycast(transform.position + wallCheckOffset, Vector2.right, wallCheckDistance, wall) ||
            Physics2D.Raycast(transform.position - wallCheckOffset, Vector2.right, wallCheckDistance, wall));

        return right;
    }

    private bool onWallCheck()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wall);
    }
    /////////////////////FLIP//////////////////////////////////////

    private void flip()
    {
        if (transform.localScale == new Vector3(1, 1, 1))
            transform.localScale = new Vector3(-1, 1, 1);

        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    /////////////////////WALL SLIDE//////////////////////////////////////
    private bool WallSlide()
    {
        //maybe add an extra check for hoirzontal movement, so player can choose to hold and fall slowly or just drop?
        if ((onWallCheck()) && !grounded && rb.velocity.y != 0 && !wallJumping)
        {
            return true;
            //rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, wallSlideSpeed, float.MaxValue));
        }


        else
            return false;
    }

    /////////////////////WALL JUMP//////////////////////////////////////
    private void WallJump()
    {
        if (wallSliding && !grounded)
        {
            wallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;
            CancelInvoke(nameof(StopWallJump));     //cancels ALL invoke calls in this script, including the one that comes later
        }

        else
        {
            wallJumpCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && wallJumpCounter > 0)
        {
            wallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpForce.x, wallJumpForce.y);
            wallJumpCounter = 0;

            if (transform.localScale.x != wallJumpDirection)
                flip();

            dashUsed = false;
            
        }

        //INVOKE: like calling a function, except you put it on timer and the function is called when timer hits 0
        Invoke(nameof(StopWallJump), wallJumpDuration);
    }

    private void StopWallJump()
    {
        wallJumping = false;
    }

    /////////////////////DASH COROUTINE//////////////////////////////////////
    /*
    THIS IS A COROUTINE. NEW TECH WEEEEE 
    */
    private IEnumerator Dash()
    {
        /*
        The process, line by line: 
        set dashUsed to true
        set dashing to true
        store gravity scale (effect of grav on player) in var
        set gravity scale to 0 so the player dashes in a straight line
        apply dash force based on direction
        -if facing left, apply force to left
        -if right, apply right
        draw the trail on screen
        tell the code to wait a bit while the dash happens before moving to the next line
        -done with 'yield return'
        remove the trail from the screen
        put gravity scale back on player
        set dashing to false
        donezo
        */
        dashUsed = true;
        dashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;


        if (xVel < 0)//moving left
            rb.velocity = Vector2.left * dashForce;

        else if (xVel > 0)//moving right
            rb.velocity = Vector2.right * dashForce;

        trail.emitting = true;
        yield return new WaitForSeconds(dashTime);
        trail.emitting = false;
        rb.gravityScale = originalGravity;
        dashing = false;
    }

    /////////////////////HIT/HIT COOLDOWN//////////////////////////////////////
    private void Hit()
    {
        //call this function when the player takes a hit
        //decrement from the hit counter, if counter at 0, death screen(to be implemented later, restart game for now)
        //after hit, trigger a short cooldown period. if player is hit during cooldown, hit doesnt count

        //if the cooldown timer isnt counting down: take the hit, set the timer, check if the player is dead and reset if so, and set cooldown timer 
        if (hitCooldown == 0)
        {
            hitsTillDead--;
            if (hitsTillDead == 0)
                canMove = false;
            else
                hitCooldown = hitCooldownReset;
        }


    }
    /// <summary>
    /// counts down the cooldown timer. called in Update so cooldown can count with out any dependancies
    /// </summary>
    private void HitCooldown()
    {
        if (hitCooldown > 0)
        {
            hitCooldown--;
            if (hitsTillDead > 0)
            {
                hitCooldownIndicator.SetActive(true);
                if (hitsTillDead == 1)
                    lastLifeText.SetActive(true);
            }

        }
        else
        {
            hitCooldownIndicator.SetActive(false);
            lastLifeText.SetActive(false);
        }


    }

    /////////////////////ON COLLISION//////////////////////////////////////

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "trap")
        {
            Hit();
        }

        if (col.gameObject.tag == "goal")
        {
            reachedGoal = true;
            canMove = false;
        }
    }
}

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

/*
        DASHING
        kinda similar to jumping: 
        -bool in update(). 
        -on button press, bool true
        -on bool true: do physics movement in fixedupdate(), set bool to false
        dun
        
        dash will be set to W for now

        okay there's a lot of bools so I'll go thru that to for clarity:
        -dashActive enables or disables the ability to dash, depending on if the player chooses dashing as a skill
        -dashing is for the dash itself. when true, do the physics thing
        -dashUsed limits the dash to once per jump. when true, player can't dash again. reset to false when the player jumps or wall jumps again
        
         MIGHT CHANGE IMPLEMENTATION TO MAKE THE DASH MORE LINEAR
         */
