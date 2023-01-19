using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public List<SoundObject> soundObjects;
    public Dictionary<GameObject, Sound[]> soundDict;
    public AudioMixerGroup audioMixerGroup;

    void Awake()
    {

        for (int i = 0; i < soundObjects.Count; i++)
        {
            if(soundObjects[i].gameObject is null)
            {
                soundObjects[i].gameObject = GameObject.FindWithTag("Player");
            }

            for(int j=0;j<soundObjects[i].sounds.Length;j++)
            {
                Sound s = soundObjects[i].sounds[j];
                s.source = soundObjects[i].gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.spatialBlend = s.spatialBlend;
                s.source.outputAudioMixerGroup = audioMixerGroup;
                s.source.playOnAwake = false;
            }
        }

        soundDict = new Dictionary<GameObject, Sound[]>();
        for (int i = 0; i < soundObjects.Count; i++)
        {
            if(soundObjects[i].gameObject is null)
            {
                continue;
            }
            soundDict.Add(soundObjects[i].gameObject, soundObjects[i].sounds);
        }
    }

    private string musicOnPlayer = "";

    /// <summary>
    /// if you use this function directly to play music I will shatter your kneecaps; sound effects are fine
    /// </summary>
    /// <param name="gameObject">a game object other than the player for the sound to be played on</param>
    /// <param name="name">name of the soundclip to play</param>
    /// <returns>whether the sound played successfully</returns>
    public bool Play(GameObject gameObject, string name)
    {
        Sound sound = Array.Find(soundDict[gameObject], s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Soundfile :\"{name}\" you are trying to start was not found");
            return false;
        }
        sound.source.Play();
        return true;
    }

    public bool PlayMusic(string name)
    {
        StopMusic();
        return Play(GameObject.FindGameObjectWithTag("Player"), musicOnPlayer = name);
    }

    public bool StopMusic()
    {
        if (musicOnPlayer == "") return true;
        return Stop(GameObject.FindGameObjectWithTag("Player"), musicOnPlayer) && (musicOnPlayer = "").Length == 0;
    }

    public bool Stop(GameObject gameObject, string name)
    {

        Sound sound = Array.Find(soundDict[gameObject], s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Soundfile :\"{name}\" you are trying to stop was not found");
            return false;
        }
        sound.source.Stop();
        return true;
    }

    public void StopAllSounds()
    {
        foreach(KeyValuePair<GameObject, Sound[]> entry in soundDict)
        {
            foreach (Sound s in entry.Value)
            {
                if(s.source.isPlaying)
                {
                    s.source.Stop();
                }
            }
        }

    }

    public bool IsSoundPlaying(GameObject gameObject, string name)
    {
        return Array.Find(soundDict[gameObject], s => s.name == name).source.isPlaying;
    }
}
