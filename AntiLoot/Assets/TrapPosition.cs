using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPosition : MonoBehaviour
{
    private Vector3 trapPos = new Vector3(25f, 11.9f, -9f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<GameManager>().trapPhase)
            transform.position = trapPos;
    }
}
