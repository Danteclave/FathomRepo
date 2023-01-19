using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluorescentL1 : MonoBehaviour
{
    private GameObject innerlight;
    public bool islighton
    {
        set => innerlight.SetActive(value);
    }

    private void Start()
    {
        innerlight = GetComponentInChildren<Light>(true).gameObject;
    }
}
