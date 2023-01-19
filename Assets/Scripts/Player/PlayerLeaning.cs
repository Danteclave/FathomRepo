using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeaning : MonoBehaviour
{

    public GameObject CameraObject;
    public Transform LeanLeft;
    public Transform LeanRight;
    public Transform defaultPosition;

    private bool leaningRight;
    private bool leaningLeft;

    public bool canLeanLeft;
    public bool canLeanRight;

    void Start()
    {
        leaningLeft = false;
        leaningRight = false;
        canLeanLeft = true;
        canLeanRight = true;
        CameraObject.transform.position = defaultPosition.position;
        CameraObject.transform.rotation = defaultPosition.rotation;
    }

    void Update()
    {
        RaycastHit hitLeft;
        RaycastHit hitRight;
        Physics.Raycast(new Ray(CameraObject.transform.position, CameraObject.transform.TransformDirection(Vector3.left)), out hitLeft);
        Physics.Raycast(new Ray(CameraObject.transform.position, CameraObject.transform.TransformDirection(Vector3.right)), out hitRight);

        if (hitLeft.distance <= 3.0f)
        {
            leaningLeft = false;
            canLeanLeft = false;
        }
        else
        {
            canLeanLeft = true;
        }

        if (hitRight.distance <= 3.0f)
        {
            leaningRight = false;
            canLeanRight = false;
        }
        else
        {
            canLeanRight = true;
        }

        if (canLeanLeft)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                leaningLeft = true;
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                leaningLeft = false;
            }
        }

        if (canLeanRight)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                leaningRight = true;
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                leaningRight = false;
            }
        }

        if (leaningLeft && !leaningRight)
        {
            CameraObject.transform.position = Vector3.Lerp(CameraObject.transform.position, LeanLeft.position, Time.deltaTime * 10);
            CameraObject.transform.rotation = Quaternion.Lerp(CameraObject.transform.rotation, LeanLeft.rotation, Time.deltaTime * 10);
        }

        if (leaningRight && !leaningLeft)
        {
            CameraObject.transform.position = Vector3.Lerp(CameraObject.transform.position, LeanRight.position, Time.deltaTime * 10);
            CameraObject.transform.rotation = Quaternion.Lerp(CameraObject.transform.rotation, LeanRight.rotation, Time.deltaTime * 10);
        }
        else
        {
            CameraObject.transform.position = Vector3.Lerp(CameraObject.transform.position, defaultPosition.position, Time.deltaTime * 10);
            CameraObject.transform.rotation = Quaternion.Lerp(CameraObject.transform.rotation, defaultPosition.rotation, Time.deltaTime * 10);
        }
    }
}
