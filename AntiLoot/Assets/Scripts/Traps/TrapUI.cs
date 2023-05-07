using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrapUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private string trapUIName;
    [SerializeField] private int TrapCount;
    [SerializeField] private int Counter;
    public GameObject trap;

    /*public void OnPointerDown(PointerEventData eventData) 
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 trapPos = new Vector3(mousePos.x, mousePos.y, trap.transform.position.z);
        GameObject newTrap = Instantiate(trap, trapPos, Quaternion.identity, trap.transform.parent);
        newTrap.SetActive(true);
    }*/
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 trapPos = new Vector3(mousePos.x, mousePos.y, trap.transform.position.z);
        GameObject newTrap = ObjectPool.instance.GetPooledObject(trapUIName);
        if (newTrap != null)
        {
            newTrap.transform.position = trapPos;
            newTrap.SetActive(true);
        }
        else if(newTrap == null)
        {
            Debug.Log("no more!");
        }
    }
}
