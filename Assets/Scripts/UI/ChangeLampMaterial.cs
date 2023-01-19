using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLampMaterial : MonoBehaviour
{
    public Material OnMaterial;
    public Material OffMaterial;

    void Start()
    {
        GameEventSystem.Instance.OnLightsTurnOn += changeMaterialOn;
        GameEventSystem.Instance.OnLightsTurnOff += changeMaterialOff;
    }

    private void changeMaterialOn()
    {
        gameObject.GetComponent<MeshRenderer>().material = OnMaterial;
    }

    private void changeMaterialOff()
    {
        gameObject.GetComponent<MeshRenderer>().material = OffMaterial;
    }
}
