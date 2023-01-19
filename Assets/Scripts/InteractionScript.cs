using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    public bool InteractableState;
    public bool Holdable;
    public bool IsInteractionObject;
    private Camera cam;
    private RaycastHit hit;

    void Start()
    {
        InteractableState = false;
        cam = Camera.main;
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2));
        Physics.Raycast(ray, out hit, 15, ~LayerMask.GetMask("Player"));
        if (hit.collider is not null && hit.distance < 10.0f && hit.collider.gameObject == this.gameObject)
        {
            InteractableState = true;
        }
        else
        {            
            InteractableState = false;
        }
    }
}
