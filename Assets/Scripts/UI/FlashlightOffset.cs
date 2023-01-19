using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightOffset : MonoBehaviour
{
    void Update()
    {
        transform.position = Camera.main.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Camera.main.transform.rotation, 10 * Time.deltaTime);
    }
}
