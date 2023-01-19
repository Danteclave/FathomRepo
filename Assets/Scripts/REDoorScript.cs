using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REDoorScript : MonoBehaviour
{
    InteractionScript interactionScript;
    public bool doorLocked;
    private Rigidbody rb;
    private Vector3 playerRelativePosition;
    private GameObject initialPosition;
    private short sign;
    public bool flip;

    void Start()
    {
        initialPosition = new GameObject();
        playerRelativePosition = new Vector3(0, 0, 0);

        rb = GetComponent<Rigidbody>();
        interactionScript = GetComponent<InteractionScript>();

        doorLocked = true;
        initialPosition.transform.position = transform.position;
        initialPosition.transform.rotation = transform.rotation;
    }

    void Update()
    {
        playerRelativePosition = initialPosition.transform.InverseTransformPoint(FindObjectOfType<PlayerController>().transform.position);

        if (interactionScript.InteractableState)
        {
            if (transform.rotation.eulerAngles.y == 0)
            {
                if (rb.velocity.x == 0)
                {
                    sign = 1;
                }
            }
            if (transform.rotation.eulerAngles.y != 270)
            {
                if (rb.velocity.x == 0)
                {
                    sign = -1;
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (playerRelativePosition.x > 0)
                {
                    var locVel = transform.InverseTransformDirection(rb.velocity);
                    locVel.x = sign * (-15);
                    rb.velocity = transform.TransformDirection(locVel) * (flip ? 1 : -1);
                }
                else
                {
                    var locVel = transform.InverseTransformDirection(rb.velocity);
                    locVel.x = sign * (15);
                    rb.velocity = transform.TransformDirection(locVel) * (flip ? 1 : -1);
                }

            }
        }
    }
}
