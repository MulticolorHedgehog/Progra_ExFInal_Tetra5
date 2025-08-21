using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        foreach(Sound s in sounds)
        {
            
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
        }
    }

    public void Play(string nombre)
    {
        foreach (Sound s in sounds)
        {
            if(s.name == nombre)
            {
                s.source.Play();
                return;
            }
        }

        Debug.Log("Pusiste la cancion equivocada mamahuevaso");
    }

    public void Stop(string nombre)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == nombre)
            {
                s.source.Stop();
                return;
            }
        }

        Debug.Log("Pusiste la cancion equivocada mamahuevaso");
    }
}
