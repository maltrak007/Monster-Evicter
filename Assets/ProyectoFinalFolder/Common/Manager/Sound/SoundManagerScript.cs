using System;
using ProyectoFinalFolder.Common.Manager.Sound;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static SoundManagerScript Instance;
    
    public Sound[] sfxSounds, musicSounds;
    public AudioSource sfxSource, musicSource;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, sound => sound.soundName == name);
        if (s == null) return;
        musicSource.clip = s.audioClip;
        musicSource.Play();
    }
    
    public void PlaySound(string name)
    {
        Sound s = Array.Find(sfxSounds, sound => sound.soundName == name);
        if (s == null) return;
        sfxSource.clip = s.audioClip;
        sfxSource.Play();
    }
}
