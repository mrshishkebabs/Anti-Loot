using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //variables to be adjusted in Unity editor need to be public
    //anything else can be private, as to not crowd the Unity editor
    public float speed;
    
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
    }
}
