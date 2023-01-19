using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonScript : MonoBehaviour
{
    Camera cam;
    RaycastHit hit;

    public void getRayCast<T>(Camera cam, float activationDistance,Action<T> methodToExecute)
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2));
        Physics.Raycast(ray, out hit);
        T script = default(T);
        if (hit.collider is not null)
        {
            script = hit.collider.gameObject.GetComponent<T>();
        }
        if(hit.collider is not null && script is not null && hit.distance < activationDistance)
        {
            if(Input.GetMouseButtonDown(0))
            {
                    methodToExecute(script);
            }
        }
    }
}
