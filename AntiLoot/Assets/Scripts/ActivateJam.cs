using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateJam : MonoBehaviour
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
    public void Jam()
    {
        player.GetComponent<PlayerController>().jamChosen = true;
    }
}