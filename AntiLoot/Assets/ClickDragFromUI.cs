using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDragFromUI : MonoBehaviour
{
    [SerializeField] private bool released = false;
    Vector3 difference = Vector2.zero;

    private void OnMouseDrag()
    {

    }

    private void OnMouseOver()
    {
        if (!released)
        {
            difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            released = true;
        }
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }


    private void OnMouseUp()
    {
        released = true;
    }

}
