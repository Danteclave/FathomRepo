using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEventSystem.Instance.OnLightsTurnOn += PlayLightsOn;
        GameEventSystem.Instance.OnLightsTurnOff += PlayLightOff;
    }

    private void PlayLightsOn()
    {
        Debug.Log("click on");
        GetComponent<AudioSource>().Play();
    }

    private void PlayLightOff()
    {
        Debug.Log("click off");
        GetComponent<AudioSource>().Play();
    }
}
