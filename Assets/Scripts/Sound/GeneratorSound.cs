using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorSound : MonoBehaviour
{
    public AudioSource start;
    public AudioSource running;
    public AudioSource stop;
    void Start()
    {
        GameEventSystem.Instance.OnLightsTurnOn += PlayLightsOn;
        GameEventSystem.Instance.OnLightsTurnOff += PlayLightOff;

    }

    private void PlayLightsOn()
    {
        if(!running.isPlaying || !start.isPlaying)
        {
            start.Play();
            running.PlayDelayed(1.3f);
        }
    }

    private void PlayLightOff()
    {
        if (running.isPlaying)
        {
            running.Stop();
            stop.Play();
        }
    }
}
