using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSpikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == Tags.PLAYER)
        {
            EventBroker.CallSpikeHit();
            FindObjectOfType<AudioManager>().Spike();
        }
    }

}