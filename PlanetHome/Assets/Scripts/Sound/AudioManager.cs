using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioMixerGroup mixerGroup;

    public Sound[] sounds;

    void Awake()
    {
        /*if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        */
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.time = s.time * s.source.clip.length;

            s.source.outputAudioMixerGroup = mixerGroup;
        }
    }

    // returns an instance of Sound from the sound pool 
    // returns a Sound object 
    public Sound returnSound(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        return s;
    }

    // Plays a sound from the sound library 
    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
        s.source.time = s.time * s.source.clip.length;
        s.source.Play();
    }

    // change the seek time as a percentage of the track 
    public void Seek(string sound, float time)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        float length = s.source.clip.length;
        s.time = time;
        Debug.Log("time is: " + time + "*" + length + " = " + s.source.time);

    }
    // chang the seek time in seconds of a sound 
    public void SeekInSeconds(string sound, float time)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        float length = s.source.clip.length;
        s.time = time / length;
        Debug.Log("time is: " + time + "*" + length + " = " + s.source.time);

    }

    // stop the given sound 
    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    // update sound based on current play time 
    public void UpdateTime(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        Debug.Log(sound + " time was " + s.time);
        s.time = s.source.time / s.source.clip.length;
        Debug.Log(sound + " Time is now" + s.source.time + "=>" + s.time);
    }

    // stop sound while preserving play time 
    public void PauseSound(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        UpdateTime(sound);
        s.source.Stop();
    }

    // return the current sound time as a percentage 
    public float GetSoundTime(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return -1;
        }
        return s.time;
    }

    // return sound time in seconds 
    public float GetSoundTimeSeconds(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return -1;
        }
        return s.source.time;
    }
}