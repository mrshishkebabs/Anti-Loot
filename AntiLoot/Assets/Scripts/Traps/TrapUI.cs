using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrapUI : MonoBehaviour, IPointerDownHandler
{
    public GameObject trap;   

    public void OnPointerDown(PointerEventData eventData) 
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 trapPos = new Vector3(mousePos.x, mousePos.y, trap.transform.position.z);
        GameObject newTrap = Instantiate(trap, trapPos, Quaternion.identity, trap.transform.parent);
        newTrap.SetActive(true);
    }
}
