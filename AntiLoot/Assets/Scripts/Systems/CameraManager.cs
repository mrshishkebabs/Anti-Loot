using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private GameObject player;

    private void OnEnable()
    {
        EventBroker.OnTrapPhaseStart += TrapPhaseCamera;
        EventBroker.OnEscapePhaseStart += EscapePhaseCamera;
    }

    private void OnDisable()
    {
        EventBroker.OnTrapPhaseStart -= TrapPhaseCamera;
        EventBroker.OnEscapePhaseStart -= EscapePhaseCamera;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space) && GameManager.instance.state == GameState.TrapPhase)
        {
            cam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            cam.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 8;
        }*/
    }

    void TrapPhaseCamera()
    {
        cam.Follow = GridManager.instance.transform;
        cam.m_Lens.OrthographicSize = 10;
    }

    void EscapePhaseCamera()
    {
        player = GameManager.instance.player;
        cam.Follow = player.transform;
        cam.m_Lens.OrthographicSize = 6;
    }
}
