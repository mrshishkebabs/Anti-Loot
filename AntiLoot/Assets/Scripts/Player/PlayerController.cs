using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerStates { Idle, Walk, JumpUp, JUptoDown, JumpDown, WallSlide, WallCling, Damage, ShotDeath, BallDeath, SpikeDeath};
public class PlayerController : MonoBehaviour
{
    public PlayerStates playerState = PlayerStates.Idle;
    public static PlayerController instance;

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
    private Vector3 wallCheckOffset;

    public Transform wallCheck;
    [SerializeField] private bool onWall;
    private float wallSlideSpeed = 2f;
    private Vector2 wallJumpForce =  new Vector2(8, 16);
    private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private float wallJumpDuration = 0.4f;
    [SerializeField] private bool wallJumping;

    private bool wallSliding = false;

    public int hitsTillDead = 3;
    public bool takingDMG = false;
    [SerializeField] private string lastTrapHit;
    private float hitCooldown = 0;
    private float hitCooldownReset = 200f;
    public GameObject hitCooldownIndicator;
    public GameObject lastLifeText;

    //win condition
    public bool reachedGoal = false;
    private bool canMove = true;        //only false when player is ded

    //escapist abilities
    public bool shieldChosen = false;
    private bool shieldActive = false;
    private float shieldDuration = 3f;
    private bool shieldUsed = false;
    public GameObject shield;

    public bool jamChosen = false;
    private float jamDuration = 1f;
    private bool jamUsed = false;
    private GameObject[] traps;
    private bool trapsStored = false;

    public bool pulseChosen = false;
    private bool pulseUsed = false;
    private float pulseForce = 4f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        EventBroker.OnEscapePhaseStart += EscapePhase;
    }

    private void OnDisable()
    {
        EventBroker.OnEscapePhaseStart -= EscapePhase;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        FOR THE DASH:
        we dont want any other movements to interrupt the dash while it's happening, 
        so if the player is dashing, skip the rest of the update function so the player
        can't do anything else
        */
        if (dashing)
            return;


        xVel = Input.GetAxisRaw(PlayerInput.HORIZONTAL);


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
            }

        }

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
        if (!takingDMG && hitsTillDead != 0)
        {
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
        }


        /////////////////////HIT COOLDOWN FOR INVULNERABILITY BETWEEN HITS//////////////////////////////////////
        HitCooldown();
        
        
        /////////////////////WALL JUMP CALL//////////////////////////////////////
        WallJump();


        /////////////////////SHIELD//////////////////////////////////////////////
        if(Input.GetKeyDown(KeyCode.E) && shieldChosen == true && shieldUsed == false)
        {
            shieldUsed = true;
            ActivateShield();
        }

        /////////////////////JAM//////////////////////////////////////////////
        if(FindObjectOfType<GameManager>().escapePhaseStarted == true && trapsStored == false)
        {
            trapsStored = true;
            traps = GameObject.FindGameObjectsWithTag("trap");
            Debug.Log(traps);
        }

        if (Input.GetKeyDown(KeyCode.E) && jamChosen == true && jamUsed == false)
        {
            jamUsed = true;
            ActivateJam();
        }

        /////////////////////PULSE//////////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.E) && pulseChosen == true && pulseUsed == false)
        {
            pulseUsed = true;
            ActivatePulse();
        }

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
        LESSON LEARNED: KEEP TABS ON *ALL* CODE/FUNCTIONS AND HOW THEY MAY INTERACT WITH EACH OTHER
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
        //while sliding, set the wall jump direciton, and reset the counter
        //(the counter is used to give the player a brief opportunity to wall jump just after leaving the wall
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

        /*
        when the player can wall jump:
        set wallJumping to true to stop other movements from interrupting the jump
        apply wallJumpForce
        disable counter
        flip sprite
        reset Dash
        */
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
            {
                canMove = false;
                if (lastTrapHit == Traps.TURRET)
                    playerState = PlayerStates.ShotDeath;
                else if (lastTrapHit == Traps.BALL)
                    playerState = PlayerStates.BallDeath;
                else if (lastTrapHit == Traps.HAMMER)
                    playerState = PlayerStates.ShotDeath;
                else if (lastTrapHit == Traps.SPIKES)
                    playerState = PlayerStates.SpikeDeath;
                else
                    GameManager.instance.UpdateGameState(GameState.TrapPhase);
            }
            else
            {
                hitCooldown = hitCooldownReset;

            }
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
                playerState = PlayerStates.Damage;
            }

        }
        else
        {
            hitCooldownIndicator.SetActive(false);
            lastLifeText.SetActive(false);
        }


    }

    private void EscapePhase()
    {
        hitsTillDead = 3;
        canMove = true;
        reachedGoal = false;
    }

    /////////////////////ON COLLISION//////////////////////////////////////

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "trap" && shieldActive == false)
        {
            lastTrapHit = col.gameObject.GetComponent<ClickDrag>().TrapType;
            Hit();
        }

        if (col.gameObject.tag == "goal")
        {
            reachedGoal = true;
            canMove = false;
            GameManager.instance.UpdateGameState(GameState.TrapPhase);
        }
    }

    /////////////////////SHIELD//////////////////////////////////////
    //activates shield effect and game object for 3 seconds
    private void ActivateShield()
    {
        shieldActive = true;
        shield.SetActive(true);

        Invoke(nameof(DisableShield), shieldDuration);
    }

    //deactvates shield effect and game object
    private void DisableShield()
    {
        shieldActive = false;
        shield.SetActive(false);
    }

    /////////////////////JAM//////////////////////////////////////////////
    //deactivates all traps for 1 second
    private void ActivateJam()
    {
        foreach (GameObject trap in traps)
        {
            trap.gameObject.SetActive(false);
        }
        Invoke(nameof(DisableJam), jamDuration);
    }
    //turns all the traps back on
    private void DisableJam()
    {
        foreach (GameObject trap in traps)
        {
            trap.gameObject.SetActive(true);
        }
    }

    /////////////////////PULSE//////////////////////////////////////////////
    //push all traps away from the player
    private void ActivatePulse()
    {
        foreach(GameObject trap in traps)
        {
            float xPos = trap.transform.position.x;
            float yPos = trap.transform.position.y;
            float zPos = trap.transform.position.z;
            
            //if trap is to the right of the player, push more right
            if(xPos > this.transform.position.x)
            {
                trap.transform.position = new Vector3(xPos + pulseForce, yPos, zPos);
            }

            //if not, push to the left
            else
                trap.transform.position = new Vector3(xPos - pulseForce, yPos, zPos);
        }
    }
    
    public void InDMGState()
    {
        if (!takingDMG)
            takingDMG = true;
        else if(takingDMG)
            takingDMG = false;
    }

    public void GobackToTrapPhase()
    {
        playerState = PlayerStates.Idle;
        takingDMG = false;
        GameManager.instance.UpdateGameState(GameState.TrapPhase);
    }
}

