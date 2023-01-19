using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
public class StrAsPair
{
    public string key;
    public AudioSource value;
}

public class SpicySoundSource : MonoBehaviour
{
    public Dictionary<string, AudioSource> snd;
    public StrAsPair[] sounds;

    private void yoink()
    {
        snd = sounds.ToDictionary(e => e.key, e => e.value);

       // if (EditorApplication.isPlaying)
        {
            foreach (var x in snd)
            {
                var yeag = FindObjectOfType<SoundManager>().audioMixerGroup;
                x.Value.outputAudioMixerGroup = yeag;
            }
        }
    }

    private void Awake()
    {
        yoink();

    }

    private void OnValidate()
    {
        //yoink();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Play(string name)
    {
        try
        {
            snd[name].Play();
            return true;
        }
        catch (KeyNotFoundException)
        {
            Debug.LogWarning($"N: Soundfile :\"{name}\" you are trying to start was not found");
            return false;
        }
    }

    public bool Stop(GameObject gameObject, string name)
    {

        try
        {
            snd[name].Stop();
            return true;
        }
        catch (KeyNotFoundException)
        {
            Debug.LogWarning($"N: Soundfile :\"{name}\" you are trying to stop was not found");
            return false;
        }
    }
}
