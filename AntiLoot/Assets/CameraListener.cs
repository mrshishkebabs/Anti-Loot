using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraListener : MonoBehaviour
{
    public AudioListener aListener;

    // Start is called before the first frame update
    void Start()
    {
        aListener = GetComponent<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        //during the escape phase, switch to the audio listener in the player
        if (FindObjectOfType<GameManager>().escapePhaseStarted)
            aListener.enabled = false;

        else
            aListener.enabled = true;

    }
}
