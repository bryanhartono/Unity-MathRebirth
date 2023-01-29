using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClip;
    [SerializeField] int soundToPlay = -1;
    private bool isPlaying;
    public AudioSource sfx;

    void Update ()
    { 

        if (soundToPlay != -1 && !isPlaying)
        {
            sfx.clip = audioClip[soundToPlay];
            sfx.Play();
            isPlaying = true;
        }
        else if(soundToPlay == -1)
        {
            isPlaying = false;
            sfx.clip = null;
        }
    }
}
