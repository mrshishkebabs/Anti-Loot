using UnityEngine.Audio;
using UnityEngine;


/*
Here, we are creating a class for  an Audio Object that will store audio to be played and adjusted by volume 
*/

[System.Serializable]//makes objects of this type appear in editor
public class Sound 
{
    //the name of the audio clip
    public string name;
    
    //the clip itself
    public AudioClip clip;

    //adjustable volume and pitch 
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    //an audio source to store the clip
    [HideInInspector]//We hide this because we'll set it in the AudioMan script, but it still needs to be public for accessability
    public AudioSource source;

    //a bool to loop the Sound
    public bool loop;

}
