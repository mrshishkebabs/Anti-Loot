using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;


//Here we are storing Sounds, and creating function to play and adjust Sounds from other scripts.
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    //here we will loop thru the list of Sounds and add a source to each Sounds
    private void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //function to play Sounds
    public void Play(string name)
    {
        //not 100% on the syntax, but this loops thru the sounds function to find a Sounds such that sound.name = name
        Sound s = Array.Find(sounds, sounds => sounds.name == name);

        //if the sound by name wasn't found, exit the function(so it doesnt play an empty sound and throw and error
        if(s == null)
        {
            Debug.LogWarning(name + "aint here bud");
            return;
        }
            

        s.source.Play();
    }

    public void Stop(string name)
    {
        //still not 100% on the syntax, but this loops thru the sounds function to find a Sounds such that sound.name = name
        Sound s = Array.Find(sounds, sounds => sounds.name == name);

        //if the sound by name wasn't found, exit the function(so it doesnt play an empty sound and throw and error
        if (s == null)
        {
            Debug.LogWarning(name + "aint here bud");
            return;
        }

        s.source.Stop();
    }

    public void Taunt()
    {
        string n = UnityEngine.Random.Range(1, 4).ToString();
        Sound s = Array.Find(sounds, sounds => sounds.name == "taunt" + n);

        if (s == null)
        {
            Debug.LogWarning(name + "aint here bud");
            return;
        }
        s.loop = false;
        s.source.Play();
    }

    public void Spike()
    {
        string n = UnityEngine.Random.Range(1, 4).ToString();
        Sound s = Array.Find(sounds, sounds => sounds.name == "Spike" + n);

        if (s == null)
        {
            Debug.LogWarning(name + "aint here bud");
            return;
        }
        s.loop = false;
        s.source.Play();
    }
}
