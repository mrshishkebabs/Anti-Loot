using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDrag : MonoBehaviour
{
    
    Vector3 difference = Vector2.zero;
    //Vector2 mousePos;

    private void Start()
    {
        //mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    private void OnMouseDown()
    {
        /*
        okay imma be real with you I don't fully understand this but imma try my best. kay here goes:
        difference represents the difference between object pos and mouse pos.
        we convert the mouse position from screen relative to world relative, then subtract
        the object position from it. this gives us the distance between the two.

        huh, that was easier than I thought to pick up:)

        */

        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    private void OnMouseDrag()
    {
        //while dragging the mouse, move the object with the mouse by subtracting the mouse pos
        //from the distance btw the object and mouse
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;

    }
}
