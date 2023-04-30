using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 8;
        }
    }
}
