using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShooting : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private Animator animator;
    [SerializeField] private bool active;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
            if (active)
            {
                animator.Play("Turret");
            }
            else
                animator.Play("Idle"); 
    }

    /*private void OnMouseUpAsButton()
    {
        if (currentState == "Idle")
        {
            animator.Play("Turret");
            currentState = "Turret";
        }
        else if (currentState == "Turret")
        {
            animator.Play("Idle");
            currentState = "Idle";
        }
    }*/

    private void OnMouseDown()
    {
        active = true;
    }

    private void OnMouseUp()
    {
        active = false;
    }

    private void Fire()
    {
        GameObject bullet = ObjectPool.instance.GetPooledObject();

        if(bullet != null)
        {
            bullet.transform.position = bulletPosition.position;
            bullet.SetActive(true);
        }
    }

}
