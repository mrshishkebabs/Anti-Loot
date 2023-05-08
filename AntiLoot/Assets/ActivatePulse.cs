using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePulse : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Pulse()
    {
        player.GetComponent<PlayerController>().pulseChosen = true;
    }
}
