using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainLightOn : MonoBehaviour
{
    void Start()
    {
        GetComponent<Light>().color = Color.red;

        GameEventSystem.Instance.OnKeyCardScanned += ChangeColor;
    }

    private void ChangeColor()
    {
        GetComponent<Light>().color = Color.green;
    }
}
