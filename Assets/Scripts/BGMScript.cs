using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMScript : MonoBehaviour
{
    [SerializeField] AudioClip [] Clips;
    AudioSource m_audio;
    GameObject glob;
    GlobalControl globalcontrol;
    bool prev = false;
    // Start is called before the first frame update
    void Start()
    {
        m_audio = GetComponent<AudioSource>();
        glob = GameObject.Find("GlobalObject");
        globalcontrol = glob.GetComponent<GlobalControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (globalcontrol.inCombat && !prev)
        {
            print("Hello");
            m_audio.clip = Clips[1];
            m_audio.Play(0);
            prev = true;
        }
        else if (!globalcontrol.inCombat && prev)
        {
            m_audio.clip = Clips[0];
            m_audio.Play(0);
            prev = false;
        }
    }
}
