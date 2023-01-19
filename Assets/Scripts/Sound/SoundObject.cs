using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundObject
{
    public GameObject gameObject;
    public Sound[] sounds;
    public SoundObject(GameObject gameObject, Sound[] sounds)
    {
        this.gameObject = gameObject;
        this.sounds = sounds;
    }
}
