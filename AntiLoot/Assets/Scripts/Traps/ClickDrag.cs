using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDrag : MonoBehaviour
{
    // public GameObject trap;    
    public int counter = 3;

    Vector3 origPos;
    Vector3 difference = Vector2.zero;
    bool inValid = false;

    //Vector2 mousePos;

    private void Start()
    {
        //mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //to make sure the clones have the trap tag
        this.tag = "trap";

        origPos = transform.position;
        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground" || 
            collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "trap"   ||
            collision.gameObject.tag == "Wall") 
            {
                inValid = true;
                Debug.Log("invalid");
            }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground" ||
            collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "trap"   ||
            collision.gameObject.tag == "Wall")
            {
                inValid = false;
                Debug.Log("valid");
            }
    }

    // --- moved to TrapUI ---
    // private void OnMouseDown()
    // {
        /*
        okay imma be real with you I don't fully understand this but imma try my best. kay here goes:
        difference represents the difference between object pos and mouse pos.
        we convert the mouse position from screen relative to world relative, then subtract
        the object position from it. this gives us the distance between the two.

        huh, that was easier than I thought to pick up:)

        */
        // if (gameObject.GetComponent<Collider2D>().isTrigger) 
        // {
        //     // Instantiate(trap, origPos, Quaternion.identity);
        //     GameObject newTrap = Instantiate(trap, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity, trap.transform.parent);
        //     newTrap.SetActive(true);
        //     difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // }
    // }

    private void Update()
    {
        if (Input.GetMouseButton(0)) MouseDrag();
        if (Input.GetMouseButtonUp(0)) MouseUp();
    }

    // private void OnMouseDrag()
    private void MouseDrag()
    {
        // while dragging the mouse, move the object with the mouse by subtracting the mouse pos
        // from the distance btw the object and mouse

        if (gameObject.GetComponent<Collider2D>().isTrigger) {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
        }

    }

    // private void OnMouseUp() 
    private void MouseUp()
    {
        if (inValid) {
            Destroy(gameObject);
        }
        else if (gameObject.GetComponent<Collider2D>().isTrigger && gameObject != null) {
            Vector3 tilePos = Tiles.currentTile.transform.position;
            transform.position = new Vector3(tilePos.x, tilePos.y, transform.position.z);

            gameObject.GetComponent<Collider2D>().isTrigger = false;
            counter--;
            if(gameObject.GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().enabled = true;
            }
        }
    }

}
