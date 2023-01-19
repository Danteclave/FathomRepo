using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

[Obsolete("Deprecated: use REDoorScript")]
public class TestDoorScript : MonoBehaviour
{
    Camera cam;
    RaycastHit hit;
    Image image;
    public float DoorWeight;
    private float MouseDelta;

    // Start is called before the first frame update
    void Start()
    {
        MouseDelta = 0;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MouseDelta = Input.GetAxis("Mouse Y");
        Ray ray = cam.ScreenPointToRay(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2));
        Physics.Raycast(ray, out hit);
        TestDoorScript tds = null;
        if (hit.collider is not null)
        {
            tds = hit.collider.gameObject.GetComponent<TestDoorScript>();
        }
        if (hit.collider is not null && tds is not null && hit.distance < 5.0f)
        {
            if(!HandHandler.HandVisible)
            {
               HandHandler.EnableHand();
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                //Debug.Log(Input.GetAxis("Mouse Y"));
                cam.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                //hit.collider.GetComponent<Rigidbody>().velocity = new Vector3(MouseDelta * (100 - DoorWeight),0,0);
            }
            if(Input.GetKeyUp(KeyCode.Mouse0))
            {
                //Cursor.lockState = CursorLockMode.None;
            }
        }
        else if(HandHandler.HandVisible)
        {
            HandHandler.DisableHand();
        }
    }
}
