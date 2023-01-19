using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FlashlightSound : MonoBehaviour
{
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameEventSystem.Instance.OnFlashlightClick += click;
    }

    private void click()
    {
        audioSource.PlayDelayed(0.1f);
    }
}
